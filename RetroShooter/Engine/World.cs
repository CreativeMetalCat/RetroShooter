using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;

namespace RetroShooter.Engine
{
    
    /**
     * Class responsible for loading level layouts
     */
    public class World
    {
        private static float[] VectorStringToArray(string vecString)
        {
            return  vecString.Split(" ").Select(x => float.Parse(x)).ToArray();
        }

        private static Vector3 VectorStringToVec3(string vecString)
        {
            float[] vecStr = VectorStringToArray(vecString);
            return new Vector3(vecStr[0],vecStr[1],vecStr[2]);
        }

        public static List<Actor> LoadWorld(string filename,RetroShooterGame game)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load("./Content/" + filename + ".lvl");
                var actors = doc.DocumentElement.SelectSingleNode("/Level/Actors") ??
                             throw new NullReferenceException("Level file is missing object list");

                List<Actor> result = new List<Actor>();
                foreach (XmlNode node in actors)
                {
                    try
                    {
                        string type = node.Attributes["type"]?.InnerText ??
                                      throw new NullReferenceException("Level file has actor with missing type");

    
                        Vector3 location = VectorStringToVec3(node["Location"]?.InnerText);
                        Vector3 rotation = VectorStringToVec3(node["Rotation"]?.InnerText);
                        
                        //TODO: Note: Ideally each class should be able to load itself from the XmlNode given to it
                        //so when this loading happens, it just finds class with this name, calls that function, tells who are their parent and moves on to the next one
                        switch (type)
                        {
                            //TODO:Add other basic type loading
                            case "Wall":
                                result.Add(
                                    game.AddActor(new RetroShooter.Shooter.Wall(
                                    node.Attributes["name"].InnerText, game.LastActorId,
                                    game, null,location,rotation))
                                );
                                break;
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        game?.AddDebugMessage("Error found in level file: " + e.Message, 5f, Color.Yellow);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                game?.AddDebugMessage("Failed to load level file. Error info: " + e.Message, 5f, Color.Red);
            }

            return new List<Actor>();
        }
    }
}
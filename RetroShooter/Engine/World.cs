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
    public static class World
    {

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
                        
                        string name = node.Attributes["name"]?.InnerText ??
                                      throw new NullReferenceException("Level file has actor with missing name");

    
                        Vector3 location = Helpers.XmlHelpers.VectorStringToVec3(node["Location"]?.InnerText);
                        Vector3 rotation =  Helpers.XmlHelpers.VectorStringToVec3(node["Rotation"]?.InnerText);
                        
                        //Possibly very slow, but it allows to avoid hardcoding every class into the world loader
                        var actorType = Type.GetType(type);
                        if (actorType != null)
                        {
                            try
                            {
                                var actor = Activator.CreateInstance(actorType, node, name,game) ??
                                            throw new NullReferenceException("Failed to create actor of type. Type:" +
                                                                             type);
                                result.Add(game?.AddActor(actor as Actor));
                            }
                            catch (MethodAccessException e)
                            {
                                game?.AddDebugMessage(
                                    "Actor of this type has inaccessible constructor with needed arguments. Given type: " +
                                    type,50f,Color.Yellow);
                            }
                            catch (NullReferenceException e)
                            {
                                game?.AddDebugMessage(e.Message,50f,Color.Yellow);
                            }
                        }
                        else
                        {
                            game?.AddDebugMessage("Failed to find type of actor. Given type: " + type,50f,Color.Yellow);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        game?.AddDebugMessage("Error found in level file: " + e.Message, 50f, Color.Yellow);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                game?.AddDebugMessage("Failed to load level file. Error info: " + e.Message, 50f, Color.Red);
            }

            return new List<Actor>();
        }
    }
}
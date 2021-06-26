using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroShooter.Engine
{
    
    public static class Mesh
    {
        public struct MeshData
        {
            public Model Model;
            public Material.Material Material;
        }
        
        public static MeshData Load(string filename,RetroShooterGame game)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("./Content/Models/" + filename + ".mod");
            try
            {
                var modelName = doc.DocumentElement.SelectSingleNode("/Model/Mesh") ??
                                throw new NullReferenceException (
                                    "Model file does not contain reference to model asset");
                //result of the loading
                MeshData data = new MeshData();
                
                data.Model = game.Content.Load<Model>(modelName.InnerText) ??
                             throw new NullReferenceException (
                                 "Model file contains reference to non-existent model. Given path:" +
                                 modelName.InnerText);
                
                XmlNode matDataNode = doc.DocumentElement.SelectSingleNode("/Model/Materials") ?? throw new NullReferenceException(
                    "Model file does not contain material info");
                
                foreach (XmlNode material in matDataNode.ChildNodes)
                {
                    //because models only support one material for all mesh -> loading only uses the first one
                    data.Material =
                        new Material.Material(
                            material.InnerText ??
                            throw new NullReferenceException("Null value read during material loading"), game);
                    return data;
                }
            }
            catch (Exception e)
            {
                game?.AddDebugMessage("Failed to load model file. Error info: " + e.Message, 5f, Color.Red);
            }
            return new MeshData();
        }
    }
}
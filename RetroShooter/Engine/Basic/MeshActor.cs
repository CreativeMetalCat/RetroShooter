using System;
using System.Xml;
using Microsoft.Xna.Framework;
using RetroShooter.Engine.Components;

namespace RetroShooter.Engine.Basic
{
    /*
     * Basic actor that displays a mesh
     */
    public class MeshActor : Actor
    {
        protected string modelName = "";
        
        public string ModelName => modelName;

        public MeshActor(string name = null, int id = default, RetroShooterGame game = null, Vector3 location = default,
            Vector3 rotation = default, Vector3 scale = default, Actor owner = null, string modelName = null) : base(
            name, id, game, location, rotation, scale, owner)
        {
            this.modelName = modelName ?? throw new ArgumentNullException(nameof(modelName));

            AddComponent(new StaticMeshRenderComponent("staticMesh", this, modelName));
        }

        public MeshActor(XmlNode xmlNode , string name, RetroShooterGame game ) : base(xmlNode, name, game)
        {
            var xml_ModelName = xmlNode["ModelName"];
            this.modelName = xml_ModelName?.InnerText ?? throw new NullReferenceException("Error creating MeshActor. Model name can not be null");
            
            AddComponent(new StaticMeshRenderComponent("staticMesh", this, modelName));
        }
    }
}
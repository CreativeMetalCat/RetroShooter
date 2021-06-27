using System.Xml;
using Microsoft.Xna.Framework;

namespace RetroShooter.Engine.Lighting
{
    public class BaseLight : Actor
    {
        protected Vector4 color;
        
        public Vector4 LightColor => color;

        public void SetLightColor(Color color) => this.color = new Vector4(color.R, color.B, color.G, color.A);
        
        public void SetLightColor(Vector4 color) => this.color = color;
        
        public BaseLight(string name, int id, RetroShooterGame game, Vector3 location = default, Vector3 rotation = default, Vector3 scale = default, Actor owner = null) : base(name, id, game, location, rotation, scale, owner)
        {
        }

        public BaseLight(XmlNode xmlNode, string name, RetroShooterGame game) : base(xmlNode, name, game)
        {
            if (xmlNode["Color"] != null)
            {
                color = Helpers.XmlHelpers.VectorStringToVec4(xmlNode["Color"]?.InnerText);
            }
        }
    }
}
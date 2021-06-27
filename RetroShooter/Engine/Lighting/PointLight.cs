using System.Xml;
using Microsoft.Xna.Framework;

namespace RetroShooter.Engine.Lighting
{
    public class PointLight : BaseLight
    {
        public PointLight(string name, int id, RetroShooterGame game, Vector3 location = default, Vector3 rotation = default, Vector3 scale = default, Actor owner = null) : base(name, id, game, location, rotation, scale, owner)
        {
        }

        public PointLight(XmlNode xmlNode, string name, RetroShooterGame game) : base(xmlNode, name, game)
        {
        }
    }
}
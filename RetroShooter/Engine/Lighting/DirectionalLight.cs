using System.Xml;
using Microsoft.Xna.Framework;

namespace RetroShooter.Engine.Lighting
{
    /*
     * Directional light is positionless type of light useful for representing sun-like light sources
     */
    public class DirectionalLight : BaseLight
    {
        public DirectionalLight(string name, int id, RetroShooterGame game, Vector3 location = default, Vector3 rotation = default, Vector3 scale = default, Actor owner = null) : base(name, id, game, location, rotation, scale, owner)
        {
        }

        public DirectionalLight(XmlNode xmlNode, string name, RetroShooterGame game) : base(xmlNode, name, game)
        {
        }
    }
}
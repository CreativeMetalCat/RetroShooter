using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroShooter.Engine.Lighting
{
    public class PointLight : BaseLight
    {
        protected float radius = 100f;

        public float Radius => radius;
        public PointLight(string name, int id, RetroShooterGame game, Vector3 location = default, Vector3 rotation = default, Vector3 scale = default, Actor owner = null) : base(name, id, game, location, rotation, scale, owner)
        {
        }

        public PointLight(XmlNode xmlNode, string name, RetroShooterGame game) : base(xmlNode, name, game)
        {
            if (xmlNode["Radius"] != null)
            {
                radius = MathF.Abs(float.Parse(xmlNode["Radius"].InnerText));
            }
        }

        public override void ApplyLightData(Effect effect, int lightId)
        {
            base.ApplyLightData(effect, lightId);
            effect.Parameters["pointLightsColor"]?.Elements[lightId].SetValue(LightColor);
            effect.Parameters["pointLightsLocation"]?.Elements[lightId].SetValue(Location);
            effect.Parameters["pointLightsIntensity"]?.Elements[lightId].SetValue(Intensity);
            effect.Parameters["pointLightsValid"]?.Elements[lightId].SetValue(true);
            effect.Parameters["pointLightsRadius"]?.Elements[lightId].SetValue(Radius);
        }
    }
}
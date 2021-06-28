using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroShooter.Engine.Lighting
{
    public class SpotLight : BaseLight
    {
        public float ConeAngle = 30f;

        public float Radius = 100f;
        
        public SpotLight(string name, int id, RetroShooterGame game,float radius = 100f,float coneAngle = 30f, Vector3 location = default, Vector3 rotation = default, Vector3 scale = default, Actor owner = null) : base(name, id, game, location, rotation, scale, owner)
        {
            ConeAngle = coneAngle;
            Radius = radius;
        }

        public SpotLight(XmlNode xmlNode, string name, RetroShooterGame game) : base(xmlNode, name, game)
        {
            if (xmlNode["ConeAngle"] != null)
            {
                ConeAngle = float.Parse(xmlNode["ConeAngle"].InnerText);
            }
            if (xmlNode["Radius"] != null)
            {
                Radius = float.Parse(xmlNode["Radius"].InnerText);
            }
        }

        public override void ApplyLightData(Effect effect, int lightId)
        {
            base.ApplyLightData(effect, lightId);
            effect.Parameters["spotLightsColor"]?.Elements[lightId]?.SetValue(LightColor);
            effect.Parameters["spotLightsLocation"]?.Elements[lightId]?.SetValue(Location);
            effect.Parameters["spotLightsDirection"]?.Elements[lightId]?.SetValue(ForwardVector);
            effect.Parameters["spotLightsIntensity"]?.Elements[lightId]?.SetValue(Intensity);
            effect.Parameters["spotLightsValid"]?.Elements[lightId]?.SetValue(true);
            effect.Parameters["spotLightsRadius"]?.Elements[lightId]?.SetValue(Radius);
            effect.Parameters["spotLightsCutoff"]?.Elements[lightId]?.SetValue(MathF.Cos(ConeAngle));
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            Rotation += new Vector3(10f * deltaTime, 10f * deltaTime, 0);
        }
    }
}
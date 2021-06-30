using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroShooter.Engine.Lighting
{
    public class SpotLight : BaseLight
    {
        public float InnerConeAngle = 30f;

        public float OuterConeAngle = 30f;

        public float Radius = 100f;

        public override Vector3 Location
        {
            get
            {
                return base.Location;
            }
            set
            {
                if (game != null)
                {
                    game.SpotlightsDirty = true;
                }

                base.Location = value;
            }
        }

        public override Vector3 Rotation
        {
            get { return base.Rotation; }
            set
            {
                if (game != null)
                {
                    game.SpotlightsDirty = true;
                }

                base.Rotation = value;
            }
        }

        public override Vector3 Scale
        {
            get { return base.Scale; }
            set
            {
                if (game != null)
                {
                    game.SpotlightsDirty = true;
                }

                base.Scale = value;
            }
        }

        public SpotLight(string name, int id, RetroShooterGame game,float radius = 100f,float innerConeAngle = 30f,float outerConeAngle = 30f, Vector3 location = default, Vector3 rotation = default, Vector3 scale = default, Actor owner = null) : base(name, id, game, location, rotation, scale, owner)
        {
            InnerConeAngle = innerConeAngle;
            OuterConeAngle = outerConeAngle;
            Radius = radius;
        }

        public SpotLight(XmlNode xmlNode, string name, RetroShooterGame game) : base(xmlNode, name, game)
        {
            if (xmlNode["InnerConeAngle"] != null)
            {
                InnerConeAngle = float.Parse(xmlNode["InnerConeAngle"].InnerText);
            }
            if (xmlNode["OuterConeAngle"] != null)
            {
                OuterConeAngle = float.Parse(xmlNode["OuterConeAngle"].InnerText);
            }
            if (xmlNode["Radius"] != null)
            {
                Radius = float.Parse(xmlNode["Radius"].InnerText);
            }
        }
    }
}
using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroShooter.Engine.Lighting
{
    public class PointLight : BaseLight
    {
        protected float radius = 100f;

        private float time = 0;
        
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
                    game.PointLightsDirty = true;
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
                    game.PointLightsDirty = true;
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
                    game.PointLightsDirty = true;
                }

                base.Scale = value;
            }
        }

        public float Radius => radius;
        public PointLight(string name, int id, RetroShooterGame game,float _radius = 100f, Vector3 location = default, Vector3 rotation = default, Vector3 scale = default, Actor owner = null) : base(name, id, game, location, rotation, scale, owner)
        {
            radius = _radius;
        }

        public PointLight(XmlNode xmlNode, string name, RetroShooterGame game) : base(xmlNode, name, game)
        {
            if (xmlNode["Radius"] != null)
            {
                radius = MathF.Abs(float.Parse(xmlNode["Radius"].InnerText));
            }
        }
    }
}
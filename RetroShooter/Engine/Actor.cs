using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using Microsoft.Xna.Framework;

namespace RetroShooter.Engine
{
    /**
     * Base object for everything that is placed in gameplay world
     */
    public class Actor: BaseObject
    {
        /**
         * Location
         */
        protected Vector3 location;

        protected Vector3 rotation;

        protected Vector3 scale;

        protected Matrix transformMatrix;

        /**
         * Game is the class that handles all of the updates
         */
        protected RetroShooterGame game;

        /*
         * Id of the actor in the world that it is spawned in
         */
        protected int id;

        /**
         * The actor that owns this actor.
         * If actor is a child/slave of the actor then their location is always relative to the owner/parent
         * If this is null then actor is "free"
         */
        private Actor owner;
        
        /**
         * All of the components that this actor has
         * To add new component use only AddComponent function
         */
        protected List<Component> Components = new List<Component>();


        public Actor(string name , int id,RetroShooterGame game, Vector3 location = default, Vector3 rotation = default,
            Vector3 scale = default, Actor owner = null) : base(name)
        {
            this.owner = owner;
            this.location = location;
            this.rotation = rotation;
            this.scale = scale;
            this.game = game ?? throw new ArgumentNullException(nameof(game));
            this.id = id;
        }

        /**
         * This is constructor for reading data from .lvl files(which use xml)
         */
        public Actor(XmlNode xmlNode,string name) : base(name)
        {
            if (xmlNode["Location"] != null)
            {
                Location = Helpers.XmlHelpers.VectorStringToVec3(xmlNode["Location"].InnerText);
            }
            if (xmlNode["Rotation"] != null)
            {
                Location = Helpers.XmlHelpers.VectorStringToVec3(xmlNode["Rotation"].InnerText);
            }
        }

        /*
         * Registers the component as this actor's component
         */
        public T AddComponent<T>(T comp) where T : Component
        {
            if (Components.Find(item => _name == comp.Name) == null)
            {
                Components.Add(comp);
                comp.Owner = this;
            }

            return null;
        }

        /**
         * Returns first component with the given name
         */
        public Component GetComponent(string name)
        {
            return Components.Find(item => _name == name);
        }
        
        /**
         * Returns first component of the type T
         */
        public Component GetComponent<T>()
        {
            return Components.Find(item => item is T);
        }
        
        /**
         * Location
         */
        public Vector3 Location
        {
            get => location + (owner?.location ?? Vector3.Zero);
            set
            {
                location = value;
                RecalculateTransformMatrix();
            }
        }

        public Vector3 Rotation
        {
            get => rotation + (owner?.rotation ?? Vector3.Zero);
            set
            {
                rotation = value;
                RecalculateTransformMatrix();
            }
        }

        public Vector3 Scale
        {
            get => scale + (owner?.scale ?? Vector3.Zero);
            set => scale = value;
        }

        public Vector3 ForwardVector => transformMatrix.Forward;

        public Vector3 RightVector => transformMatrix.Right;
        
        /*
         * Transform matrix of the actor
         */
        public Matrix TransformMatrix => transformMatrix;

        /**
         * The actor that owns this actor.
         * If actor is a child/slave of the actor then their location is always relative to the owner/parent
         */
        public Actor Owner
        {
            get => owner;
            set => owner = value;
        }

        protected int Id => id;

        /**
         * Game is the class that handles all of the updates
         */
        public RetroShooterGame Game
        {
            get => game;
        }

        /*
         * Recalculates transform matrix based on new location and rotation
         */
        protected void RecalculateTransformMatrix()
        {
                float cosPitch = MathF.Cos(MathHelper.ToRadians(Rotation.X));

                float sinPitch = MathF.Sin(MathHelper.ToRadians(Rotation.X));

                float cosYaw = MathF.Cos(MathHelper.ToRadians(Rotation.Y));
                float sinYaw = MathF.Sin(MathHelper.ToRadians(Rotation.Y));

                Vector3 xAxis = new Vector3(cosYaw, 0, -sinYaw);
                Vector3 yAxis = new Vector3(sinYaw * sinPitch, cosPitch, cosYaw * sinPitch);
                Vector3 zAxis = new Vector3(sinYaw * cosPitch, -sinPitch, cosPitch * cosYaw);

                transformMatrix = new Matrix
                (
                    new Vector4(xAxis.X, yAxis.X, zAxis.X, 0),
                    new Vector4(xAxis.Y, yAxis.Y, zAxis.Y, 0),
                    new Vector4(xAxis.Z, yAxis.Z, zAxis.Z, 0),
                    new Vector4(-Vector3.Dot(xAxis, Location), -Vector3.Dot(yAxis, Location),
                        -Vector3.Dot(zAxis, Location), 1)
                );
            
        }
    

        public override void Init()
        {
            base.Init();
            foreach (Component component in Components)
            {
                component?.Init();
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            //should this be calculated each frame?
            //i mean it's calculated before each frame anyway
            /* old version of the code
              public virtual Vector3 ForwardVector
                {
                get => new Vector3
                (
                    (MathF.Cos(MathHelper.ToRadians(rotation.Y)) * MathF.Sin(MathHelper.ToRadians(rotation.X))),
                    MathF.Sin(MathHelper.ToRadians(rotation.Y)),
                    (MathF.Cos(MathHelper.ToRadians(rotation.Y)) * MathF.Sin(MathHelper.ToRadians(rotation.X)))
                 );
                }
             */

            RecalculateTransformMatrix();
            
            foreach (Component component in Components)
            {
                component?.Update(deltaTime);
            }
        }

        public virtual void Draw(float deltaTime)
        {
            foreach (Component component in Components)
            {
                component?.Draw(deltaTime);
            }
        }
        
        
    }
}
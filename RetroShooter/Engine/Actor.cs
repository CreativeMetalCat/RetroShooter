using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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
            this.location = location;
            this.rotation = rotation;
            this.scale = scale;
            this.game = game ?? throw new ArgumentNullException(nameof(game));
            this.id = id;
            this.owner = owner;
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
        public virtual Vector3 Location
        {
            get => location + (owner?.location ?? Vector3.Zero);
            set => location = value;
        }

        public virtual Vector3 Rotation
        {
            get => rotation+ (owner?.rotation ?? Vector3.Zero);
            set => rotation = value;
        }

        public virtual Vector3 Scale
        {
            get => scale + (owner?.scale ?? Vector3.Zero);
            set => scale = value;
        }

        public virtual Vector3 ForwardVector
        {
            get => new Vector3
            (
                (MathF.Cos(MathHelper.ToRadians(rotation.Y)) * MathF.Sin(MathHelper.ToRadians(rotation.X))),
                MathF.Sin(MathHelper.ToRadians(rotation.Y)),
                (MathF.Cos(MathHelper.ToRadians(rotation.Y)) * MathF.Sin(MathHelper.ToRadians(rotation.X)))
            );
        }

        public virtual Vector3 RightVector
        {
            get => new Vector3
            (
                MathF.Sin(rotation.X - MathF.PI / 2f),
                0,
                MathF.Cos(rotation.X - MathF.PI / 2f)
            );
        }

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
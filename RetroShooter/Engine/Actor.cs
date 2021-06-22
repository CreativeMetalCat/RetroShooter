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
        private Vector3 location;

        private Vector3 rotation;

        private Vector3 scale;

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

        public Component AddComponent<T>(string name, object[] args) where T : Component
        {
            if (Components.Find(item => _name == name) == null)
            {
                try
                {
                    var comp = Activator.CreateInstance(typeof(T), new object[] {name, this, args}) as Component;
                    if (comp != null)
                    {
                        Components.Add(comp);
                    }

                    return comp;
                }
                catch (Exception e)
                {
                    //TODO: Add logging information about exception
                    Debug.Write(e.Message);
                    return null;
                }
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
            set => location = value;
        }

        public Vector3 Rotation
        {
            get => rotation+ (owner?.rotation ?? Vector3.Zero);
            set => rotation = value;
        }

        public Vector3 Scale
        {
            get => scale + (owner?.scale ?? Vector3.Zero);
            set => scale = value;
        }

        public Vector3 ForwardVector
        {
            get => new Vector3
            (
                MathF.Cos(rotation.Y) * MathF.Sin(rotation.X),
                MathF.Sin(rotation.Y),
                MathF.Cos(rotation.Y) * MathF.Sin(rotation.X)
            );
        }

        public Vector3 RightVector
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
    }
}
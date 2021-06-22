using System;

namespace RetroShooter.Engine
{
    /*
     * Base class for all components
     * Components are used for simplifying tasks(such as component for drawing meshes, calculating physics body properties etc.)
     */
    public class Component : BaseObject
    {
        private Actor owner;

        public Component(string name, Actor owner) : base(name)
        {
            this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        public Actor Owner
        {
            get => owner;
            set => owner = value;
        }
    }
}
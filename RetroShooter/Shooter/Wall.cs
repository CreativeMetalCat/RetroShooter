﻿using System.Xml;
using Microsoft.Xna.Framework;
using RetroShooter.Engine;
using RetroShooter.Engine.Components;

namespace RetroShooter.Shooter
{
    public class Wall : Engine.Actor
    {
        public Wall(string name, int id, RetroShooterGame game,Actor owner = null, Vector3 location = default, Vector3 rotation = default,
            Vector3 scale = default) : base(name, id, game, location, rotation, scale, owner)
        {
            AddComponent<StaticMeshRenderComponent>(new StaticMeshRenderComponent
                ("Wall",
                    this,
                    "SM_Wall_400x400")
            );
        }
        
        public Wall(string name, int id, RetroShooterGame game,Actor owner = null) : base(name, id, game, Vector3.Zero, Vector3.Zero, Vector3.One, owner)
        {
            AddComponent<StaticMeshRenderComponent>(new StaticMeshRenderComponent
                ("Wall",
                    this,
                    "SM_Wall_400x400")
            );
        }

        public Wall(XmlNode node, string name,RetroShooterGame game) : base(node, name,game)
        {
            AddComponent<StaticMeshRenderComponent>(new StaticMeshRenderComponent
                ("Wall",
                    this,
                    "SM_Wall_400x400")
            );
        }
    }
}
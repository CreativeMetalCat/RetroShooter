using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroShooter.Engine.Components
{
    /**
     * Component that handles rendering of models that don't have any bones
     * NOTE: This component still can render meshes containing bones but it will just ignore them
     */
    public class StaticMeshRenderComponent : Component
    {
        /*
         * Used when this component has to load model manually instead of using given model
         */
        private string _materialName;
        
        public Model Model;

        protected Material.Material material;

        public StaticMeshRenderComponent(string name, Actor owner , Model model = null,
            Material.Material material = null) : base(name, owner)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            this.material = material ?? throw new ArgumentNullException(nameof(material));
        }
        
        public StaticMeshRenderComponent(string name, Actor owner , string modelName,
            string materialName) : base(name, owner)
        {
            Model = owner.Game.Content.Load<Model>(modelName);
            this.material = new Material.Material();
            _materialName = materialName;
        }

        public override void Init()
        {
            base.Init();
            material.Load(_materialName,Owner.Game);
        }

        public Material.Material Material => material;

        public override void Draw(float deltaTime)
        {
            if (Model != null && material != null)
            {
                foreach (var mesh in Model.Meshes)
                {
                    foreach (var meshPart in mesh.MeshParts)
                    {
                        meshPart.Effect = material.Effect;
                        material.Apply(Owner.Game,mesh.ParentBone.Transform);
                    }
                    mesh.Draw();
                }
            }
        }
        
    }
}
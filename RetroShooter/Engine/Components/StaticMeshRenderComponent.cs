using System;
using System.Xml;
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
        enum LoadType
        {
            None,
            Material,
            File
        }
        /*
         * Used when this component has to load model manually instead of using given model
         * Also used as filename is LoadType is File
         */
        private string _materialName;
        
        public Model Model;

        protected Material.Material material;

        private LoadType _loadType;
        public StaticMeshRenderComponent(string name, Actor owner , Model model = null,
            Material.Material material = null) : base(name, owner)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            this.material = material ?? throw new ArgumentNullException(nameof(material));
            _loadType = LoadType.None;
        }
        
        public StaticMeshRenderComponent(string name, Actor owner , string modelName,
            string materialName) : base(name, owner)
        {
            Model = owner.Game.Content.Load<Model>(modelName);
            this.material = new Material.Material();
            _materialName = materialName;
            _loadType = LoadType.Material;
        }

        /*
         * Loads model from file(.mod file)
         */
        public StaticMeshRenderComponent(string name, Actor owner, string assetName): base(name,owner)
        {
            _loadType = LoadType.File;
            _materialName = assetName ?? throw  new NullReferenceException("Model file path is null");
        }

        public override void Init()
        {
            base.Init();
            switch (_loadType)
            {
                case LoadType.Material:
                    material.Load(_materialName,Owner.Game);
                    break;
                case LoadType.File:
                    Mesh.MeshData data = Mesh.Load(_materialName, Owner.Game);
                    Model = data.Model;
                    material = data.Material;
                    break;
            }
            
        }

        public Material.Material Material => material;

        public override void Draw(float deltaTime)
        {
            if (Model != null && material != null)
            {
                Model.Root.Transform = Owner.TransformMatrix;
                Owner?.Game?.AddDebugMessage(Model.Root.Transform.Translation.ToString(),0,Color.Aqua);
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
            else
            {
                Owner?.Game?.AddDebugMessage("StaticMeshComponent Error! Model is null!", 0, Color.Red);
            }
        }
        
    }
}
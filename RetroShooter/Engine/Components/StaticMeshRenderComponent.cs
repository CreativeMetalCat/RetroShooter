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
            float cosPitch = MathF.Cos(MathHelper.ToRadians(Owner.Rotation.X));
            float sinPitch = MathF.Sin(MathHelper.ToRadians(Owner.Rotation.X));

            float cosYaw = MathF.Cos(MathHelper.ToRadians(Owner.Rotation.Y));
            float sinYaw = MathF.Sin(MathHelper.ToRadians(Owner.Rotation.Y));
            Vector3 xAxis = new Vector3(cosYaw, 0, -sinYaw);
            Vector3 yAxis = new Vector3(sinYaw * sinPitch, cosPitch, cosYaw * sinPitch);
            Vector3 zAxis = new Vector3(sinYaw * cosPitch, -sinPitch, cosPitch * cosYaw);

            Model.Root.Transform = new Matrix
            (
                new Vector4(xAxis.X,yAxis.X,zAxis.X,0),
                new Vector4(xAxis.Y,yAxis.Y,zAxis.Y,0),
                new Vector4(xAxis.Z,yAxis.Z,zAxis.Z,0),
                new Vector4(-Vector3.Dot(xAxis,Owner.Location),-Vector3.Dot(yAxis,Owner.Location),-Vector3.Dot(zAxis,Owner.Location),1)
            );
            
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
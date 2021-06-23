using Microsoft.Xna.Framework.Graphics;

namespace RetroShooter.Engine.Components
{
    /**
     * Component that handles rendering of models that don't have any bones
     * NOTE: This component still can render meshes containing bones but it will just ignore them
     */
    public class StaticMeshRenderComponent : Component
    {
        public Model Model;

        protected Material.Material material;

        public Material.Material Material => material;

        void Draw(float deltaTime)
        {
            if (Model != null && material != null)
            {
                foreach (var mesh in Model.Meshes)
                {
                    foreach (var meshPart in mesh.MeshParts)
                    {
                        meshPart.Effect = material.Effect;
                    }
                    mesh.Draw();
                }
            }
        }
        
    }
}
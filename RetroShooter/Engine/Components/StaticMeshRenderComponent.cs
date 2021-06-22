using Microsoft.Xna.Framework.Graphics;

namespace RetroShooter.Engine.Components
{
    /**
     * Component that handles rendering of models that don't have any bones
     * NOTE: This component still can render meshes containing bones but it will just ignore them
     */
    public class StaticMeshRenderComponent
    {
        public Model Model;
    }
}
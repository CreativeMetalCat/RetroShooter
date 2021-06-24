using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RetroShooter
{
    public class Test3D
    {
        //Camera
        public Vector3 camTarget;
        public Vector3 camPosition;
        public Matrix projectionMatrix;
        public Matrix viewMatrix;
        public Matrix worldMatrix;

        private Model model;
        
        //BasicEffect for rendering
        Effect basicEffect;
        
        //Geometric info
        VertexPositionColor[] triangleVertices;
        VertexBuffer vertexBuffer;

        public void Init(GraphicsDevice graphicsDevice)
        {
            
            //Setup Camera
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -100f);
            
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45f), 
                graphicsDevice.DisplayMode.AspectRatio,
                1f, 1000f);
            
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, 
                new Vector3(0f, 1f, 0f));// Y up
            
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.
                Forward, Vector3.Up);
            
            
            
            
           
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            //BasicEffect
            basicEffect = contentManager.Load<Effect>("Effects/TestEffect");


            basicEffect.Parameters["BaseTexture"].SetValue(contentManager.Load<Texture2D>("Textures/T_Bricks"));



            model = contentManager.Load<Model>("Models/SM_Chair");
            //Vert buffer
            vertexBuffer = model.Meshes[0].MeshParts[0].VertexBuffer; /*new VertexBuffer(graphicsDevice, typeof(
                VertexPositionColor), 3, BufferUsage.
                WriteOnly);*/
        }

        public void Draw(GameTime gameTime,GraphicsDevice graphicsDevice)
        {
            /*basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;*/
            
            graphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in model.Meshes[0].MeshParts)
                {
                    meshPart.Effect = basicEffect;
                    basicEffect.Parameters["World"].SetValue(worldMatrix * mesh.ParentBone.Transform);
                    basicEffect.Parameters["View"].SetValue(viewMatrix);
                    basicEffect.Parameters["Projection"].SetValue(projectionMatrix);

                }
                mesh.Draw();
            }
        }
        
    }
}
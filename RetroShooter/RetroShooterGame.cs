using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RetroShooter
{
    public class TestImage
    {
        private string textureName;

        public TestImage(string _textureName)
        {
            textureName = _textureName;
        }
        
        /**
         * Texture to draw
         */
        public Texture2D Image;

        /**
         * Onscreen location
         */
        public Vector2 Location;

       public void LoadContent(ContentManager manager)
        {
            Image = manager.Load<Texture2D>(textureName);
        }
        
       public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image,Location,Color.White);
        }
    }

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
    
    public class RetroShooterGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private TestImage image;

        private Test3D _test3D;
        
        public RetroShooterGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            image = new TestImage("Textures/T_Bricks");

            _test3D = new Test3D();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _test3D.Init(_graphics.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            image.LoadContent(Content);
            
            _test3D.LoadContent(_graphics.GraphicsDevice,Content);
           
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _test3D.camPosition.Y -= 1;
                _test3D.camTarget.Y -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _test3D.camPosition.Y += 1;
                _test3D.camTarget.Y += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _test3D.camPosition.X += 1;
                _test3D.camTarget.X += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _test3D.camPosition.X -= 1;
                _test3D.camTarget.X -= 1;
            }
            _test3D.viewMatrix = Matrix.CreateLookAt( _test3D.camPosition,  _test3D.camTarget, 
                Vector3.Up);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _test3D.Draw(gameTime,_graphics.GraphicsDevice);
            _spriteBatch.Begin();
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

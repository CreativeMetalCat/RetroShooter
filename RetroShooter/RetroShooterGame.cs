using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroShooter.Engine;
using RetroShooter.Engine.Camera;
using RetroShooter.Engine.Material;

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
    
    public class RetroShooterGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public GraphicsDevice GraphicsDevice => _graphics.GraphicsDevice;
        
        /**
         * Current camera that is used for rendering
         */
        public Camera CurrentCamera
        {
            get => currentCamera;
            set => currentCamera = value;
        }

        protected Camera currentCamera;
        
        /**
         * Id of last spawned actor
         * used to give actor a way to get identified without numbers getting repreated
         */
        private int lastActorId = 0;
            
        /**
         * All of the actors that are spawned in the world
         */
        protected List<Engine.Actor> actors = new List<Actor>();

        /**
         * Adds actor to the world.
         * If name is already taken or any other error occured => returns null
         */
        public Actor SpawnActor<T>(string name,Actor owner,object[] args)
        {
            if(actors.Find(item=> item.Name == name) == null)
            {
                try
                {
                    var actor = Activator.CreateInstance(typeof(T), new object[] {name,lastActorId, args,owner}) as Actor;
                    lastActorId++;
                    actors.Add(actor);
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
        
        public RetroShooterGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
          
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            
            _spriteBatch.Begin();
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

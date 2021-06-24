using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroShooter.Engine;
using RetroShooter.Engine.Camera;
using RetroShooter.Engine.Material;
using RetroShooter.Shooter;

namespace RetroShooter
{
    public class RetroShooterGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        protected SpriteFont defaultFont;
        
        public SpriteFont DefaultFont
        {
            get => defaultFont;
        }
        public GraphicsDevice GraphicsDevice => _graphics.GraphicsDevice;
        
        /**
         * Current camera that is used for rendering
         */
        public Camera CurrentCamera
        {
            get => currentCamera;
            set => currentCamera = value;
        }

        public void AddDebugMessage(string msg,float duration= default,Color color = default)
        {
            debugOutput.Add(new DebugMessage(msg,duration,color));
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

        protected List<DebugMessage> debugOutput = new List<DebugMessage>();
        /**
         * Adds actor to the world.
         * If name is already taken or any other error occured => returns null and doesn't add actor
         * It is preferable to pass not already existing actor but a `new Actor(args)`
         */
        public T AddActor<T>(T actor) where T: Actor
        {
            if (actors.Find(item => item.Name == actor.Name) == null)
            {
                actors.Add(actor);
                lastActorId++;
                return actor;
            }
            return actor;
        }

        public RetroShooterGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
          
            
        }
        
        public int LastActorId
        {
            get => lastActorId;
        }
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            defaultFont = Content.Load<SpriteFont>("bebas_neue");
                
            AddActor(new Wall("wall", LastActorId, this,null));
            currentCamera = AddActor(new TestCamera("camera", LastActorId, this));
            
            foreach (Actor actor in actors)
            {
                actor.Init();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
            
            foreach (Actor actor in actors)
            {
                actor.Update(1);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            foreach (Actor actor in actors)
            {
                actor.Draw(1);
            }
            
            AddDebugMessage(gameTime.ElapsedGameTime.Milliseconds.ToString(),0,Color.Blue);
            _spriteBatch.Begin();
            if (debugOutput.Count > 0)
            {
                for (int i = 0; i < debugOutput.Count; i++)
                {
                    _spriteBatch.DrawString(defaultFont, debugOutput[i].Message, new Vector2(0, i * 12), debugOutput[i].Color);
                    debugOutput[i].CurrentLifeTime += gameTime.ElapsedGameTime.Milliseconds;
                }

                for (int i = debugOutput.Count - 1; i >= 0; i--)
                {
                    if (debugOutput[i].CurrentLifeTime > debugOutput[i].Duration)
                    {
                        debugOutput.Remove(debugOutput[i]);
                    }
                }
                debugOutput.Clear();
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

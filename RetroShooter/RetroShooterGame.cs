using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroShooter.Engine;
using RetroShooter.Engine.Basic;
using RetroShooter.Engine.Camera;
using RetroShooter.Engine.Lighting;
using RetroShooter.Engine.Material;
using RetroShooter.Shooter;
using RetroShooter.Shooter.Player;
using DirectionalLight = RetroShooter.Engine.Lighting.DirectionalLight;

namespace RetroShooter
{
    public class RetroShooterGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        protected SpriteFont defaultFont;

        public Vector4 CurrentAmbientLightColor = Vector4.Zero;

        public SpriteFont DefaultFont
        {
            get => defaultFont;
        }

        public GraphicsDevice GraphicsDevice => _graphics.GraphicsDevice;

        public List<PointLight> CurrentlyActivePointLights = new List<PointLight>();
        public List<SpotLight> CurrentlyActiveSpotLights = new List<SpotLight>();

        /**
         * Current camera that is used for rendering
         */
        public Camera CurrentCamera
        {
            get => currentCamera;
            set => currentCamera = value;
        }

        public void AddDebugMessage(string msg, float duration = default, Color color = default)
        {
            debugOutput.Add(new DebugMessage(msg, duration, color));
        }

        public bool PointLightsDirty = true;

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

        private bool IsSpaceDown = false;

        protected List<DebugMessage> debugOutput = new List<DebugMessage>();

        #region PointLights

        /* *
         *Max amount of point lights that engine can render at once
         * */
        public const int MAX_POINT_LIGHTS = 16;

        /*
         * Array of all point light colors
         */
        private Vector4[] _pointColors = new Vector4[MAX_POINT_LIGHTS];

        /*
        * Array of all point light locations
        */
        private Vector3[] _pointLocations = new Vector3[MAX_POINT_LIGHTS];

        /*
        * Array of all point light Intensities
        */
        private float[] _pointIntensities = new float[MAX_POINT_LIGHTS];

        /*
        * Array of all point light radii
        */
        private float[] _pointRadii = new float[MAX_POINT_LIGHTS];

        public Vector4[] PointColors => _pointColors;
        public Vector3[] PointLocations => _pointLocations;
        public float[] PointIntensities => _pointIntensities;
        public float[] PointRadii => _pointRadii;

        #endregion //PointLights

        #region Spotlights

        public const int MAX_SPOT_LIGHTS = 16;

        /*
        * Array of all spot light colors
        */
        private Vector4[] _spotColors = new Vector4[MAX_SPOT_LIGHTS];

        /*
        * Array of all spot light locations
        */
        private Vector3[] _spotLocations = new Vector3[MAX_SPOT_LIGHTS];

        /*
        * Array of all spot light directions
        */
        private Vector3[] _spotDirections = new Vector3[MAX_SPOT_LIGHTS];

        /*
        * Array of all spot light Intensities
        */
        private float[] _spotIntensities = new float[MAX_SPOT_LIGHTS];

        /*
       * Array of all spot light InnerCutoffs
       */
        private float[] _spotInnerCutoffs = new float[MAX_SPOT_LIGHTS];

        /*
       * Array of all spot light OuterCutoffs
       */
        private float[] _spotOuterCutoffs = new float[MAX_SPOT_LIGHTS];
        /*
        * Array of all spot light radii
        */
        private float[] _spotRadii = new float[MAX_SPOT_LIGHTS];

        public Vector4[] SpotColors => _spotColors;
        public Vector3[] SpotLocations => _spotLocations;
        public float[] SpotIntensities => _spotIntensities;
        public float[] SpotRadii => _spotRadii;
        public float[] SpotInnerCutoffs => _spotInnerCutoffs;
        public float[] SpotOuterCutoffs => _spotOuterCutoffs;

        public Vector3[] SpotDirections => _spotDirections;

        /*
         * If this is false spotlight data will be updated
         */
        public bool SpotlightsDirty = true;

        #endregion //Spotlights

        #region DirectionalLights

        public const int MAX_DIR_LIGHTS = 2;

        /*
        * Array of all directional light colors
        */
        private Vector4[] _dirColors = new Vector4[MAX_DIR_LIGHTS];

        /*
        * Array of all directional light directions
        */
        private Vector3[] _dirDirections = new Vector3[MAX_DIR_LIGHTS];

        /*
        * Array of all directional light Intensities
        */
        private float[] _dirIntensities = new float[MAX_DIR_LIGHTS];

        public Vector4[] DirColors => _dirColors;
        public Vector3[] DirDirections => _dirDirections;
        public float[] DirIntensities => _dirIntensities;

        public bool DirectionalLightDirty = true;

        /*
         * List of all directional lights in the game
         * Unlike point or spot lights these all always "Active" and do not get "deactivated"
         * But it still has to be a list because light can be destroyed during runtime
         */
        public List<DirectionalLight> DirectionalLights = new List<DirectionalLight>();
        #endregion

        /**
         * Adds actor to the world.
         * If name is already taken or any other error occured => adds number to the end of the actor's name
         * It is preferable to pass not already existing actor but a `new Actor(args)`
         */
        public T AddActor<T>(T actor) where T : Actor
        {
            if (actors.Find(item => item.Name == actor.Name) != null)
            {
                actor.Name = actor.Name + LastActorId.ToString();
            }

            actors.Add(actor);
            lastActorId++;
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

        public Actor GetActor(string name)
        {
            return actors.Find(item => item.Name == name);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            defaultFont = Content.Load<SpriteFont>("bebas_neue");

            currentCamera = AddActor(new PlayerCamera3D("player", LastActorId, this, 0.1f, true));

            World.LoadWorld("Levels/test", this);

            foreach (Actor actor in actors)
            {
                actor.Init();
            }
        }



        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            foreach (Actor actor in actors)
            {
                actor.Update(gameTime.ElapsedGameTime.Milliseconds);

                //temporary and rather bad solution
                //but the goal is to have lights only be in that list when they are close enough to the player
                if (!CurrentlyActivePointLights.Contains(actor as PointLight) && actor is PointLight)
                {
                    CurrentlyActivePointLights.Add(actor as PointLight);
                }

                if (!CurrentlyActiveSpotLights.Contains(actor as SpotLight) && actor is SpotLight)
                {
                    CurrentlyActiveSpotLights.Add(actor as SpotLight);
                }

                if (!DirectionalLights.Contains(actor as DirectionalLight) && actor is DirectionalLight)
                {
                    DirectionalLights.Add(actor as DirectionalLight);
                }
            }
            if (DirectionalLightDirty)
            {
                for (int i = 0; i < MAX_DIR_LIGHTS; i++)
                {
                    try
                    {
                        if (DirectionalLights[i] != null)
                        {
                            _dirIntensities[i] = DirectionalLights[i].Intensity;
                            _dirColors[i] = DirectionalLights[i].LightColor;
                            _dirDirections[i] = DirectionalLights[i].ForwardVector;
                        }
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        _dirIntensities[i] = 0;
                        _dirColors[i] = Vector4.Zero;
                        _dirDirections[i] = Vector3.Zero;
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        _dirIntensities[i] = 0;
                        _dirColors[i] = Vector4.Zero;
                        _dirDirections[i] = Vector3.Zero;
                    }
                }
                
            }
            
            if (PointLightsDirty)
            {
                //this value MUST match MAX_POINT_LIGHTS in effect used by this material
                for (int i = 0; i < MAX_POINT_LIGHTS; i++)
                {
                    //this means that i is in range of the lights
                    if (CurrentlyActivePointLights.Count > i)
                    {
                        //game.CurrentlyActivePointLights[i].ApplyLightData(_effect, i);
                        _pointColors[i] = CurrentlyActivePointLights[i].LightColor;
                        _pointLocations[i] = CurrentlyActivePointLights[i].Location;
                        _pointIntensities[i] = CurrentlyActivePointLights[i].Intensity;
                        _pointRadii[i] = CurrentlyActivePointLights[i].Radius;
                    }

                    else
                    {
                        _pointColors[i] = Vector4.Zero;
                        _pointLocations[i] = Vector3.Zero;
                        _pointIntensities[i] = 0;
                        _pointRadii[i] = 0;
                    }
                }
            }

            if (SpotlightsDirty)
            {
                //this value MUST match MAX_SPOT_LIGHTS in effect used by this material
                for (int i = 0; i < MAX_SPOT_LIGHTS; i++)
                {
                    //this means that i is in range of the lights
                    if (CurrentlyActiveSpotLights.Count > i)
                    {
                        _spotColors[i] = CurrentlyActiveSpotLights[i].LightColor;
                        _spotLocations[i] = CurrentlyActiveSpotLights[i].Location;
                        _spotDirections[i] = CurrentlyActiveSpotLights[i].ForwardVector;
                        _spotIntensities[i] = CurrentlyActiveSpotLights[i].Intensity;
                        _spotRadii[i] = CurrentlyActiveSpotLights[i].Radius;
                        _spotInnerCutoffs[i] = MathF.Cos(CurrentlyActiveSpotLights[i].InnerConeAngle);
                        _spotOuterCutoffs[i] = MathF.Cos(CurrentlyActiveSpotLights[i].OuterConeAngle);
                    }
                    //this means that i is outside of the range and that we need to place fake lights(lights that will not be calculated)
                    else
                    {
                        _spotColors[i] = Vector4.Zero;
                        _spotLocations[i] = Vector3.Zero;
                        _spotDirections[i] = Vector3.Zero;
                        _spotIntensities[i] = 0;
                        _spotRadii[i] = 0;
                        _spotInnerCutoffs[i] = 0;
                        _spotOuterCutoffs[i] = 0;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !IsSpaceDown)
            {
                IsMouseVisible = !IsMouseVisible;
                IsSpaceDown = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                IsSpaceDown = false;
            }

            for (int i = actors.Count - 1; i > -1; i--)
            {
                if (!actors[i].Valid)
                {
                    //delete actor
                    actors.RemoveAt(i);

                    if (actors[i] is PointLight)
                    {
                        CurrentlyActivePointLights.Remove(actors[i] as PointLight);
                    }
                    if (actors[i] is SpotLight)
                    {
                        CurrentlyActiveSpotLights.Remove(actors[i] as SpotLight);
                    }
                }
            }
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            _spriteBatch.Begin();
            foreach (Actor actor in actors)
            {
                actor.Draw(gameTime.ElapsedGameTime.Milliseconds);
            }

            //all of the materials that needed to update light data were updated
            PointLightsDirty = false;
            SpotlightsDirty = false;
            DirectionalLightDirty = false;

            AddDebugMessage(gameTime.ElapsedGameTime.Milliseconds.ToString(), 0, Color.Blue);
            if (!IsMouseVisible)
            {
                AddDebugMessage("Mouse is hidden. Press SPACEBAR to show mouse", 0, Color.Azure);
            }

            try
            {
                if (debugOutput.Count > 0)
                {
                    for (int i = 0; i < debugOutput.Count; i++)
                    {
                        _spriteBatch.DrawString(defaultFont, debugOutput[i].Message, new Vector2(0, i * 12),
                            debugOutput[i].Color);
                        debugOutput[i].CurrentLifeTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                    }

                    for (int i = debugOutput.Count - 1; i >= 0; i--)
                    {
                        if (debugOutput[i].CurrentLifeTime > debugOutput[i].Duration)
                        {
                            debugOutput.Remove(debugOutput[i]);
                        }
                    }

                }
            }
            catch (System.ArgumentException e)
            {
                _spriteBatch.DrawString(defaultFont, "Debug output error: Attempted to draw illegal characters",
                    Vector2.Zero, Color.Red);
                debugOutput.Clear();
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

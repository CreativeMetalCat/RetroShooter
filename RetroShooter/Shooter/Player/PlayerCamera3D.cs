using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RetroShooter.Engine;
using RetroShooter.Engine.Camera;

namespace RetroShooter.Shooter.Player
{
    /*
     * Player controlled camera that uses mouse for movement
     */
    public class PlayerCamera3D : Camera
    {
        public float MouseSpeed = 0.01f;

        //If camera is in debug mode it also allows player to move around using WASD
        public bool IsInDebugMode = false;
        
        public PlayerCamera3D(string name, int id, RetroShooterGame game,float mouseSpeed = 1f,bool isInDebugMode = false, Vector3 location = default,
            Vector3 rotation = default, Vector3 scale = default, float fieldOfView = 90, Actor owner = null) : base(
            name, id, game, location, rotation, scale, fieldOfView, owner)
        {
            IsInDebugMode = isInDebugMode;
            MouseSpeed = mouseSpeed;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (game.IsActive)
            {
                Point mouseDelta = Mouse.GetState().Position - new Point(game.GraphicsDevice.DisplayMode.Width / 2,
                    game.GraphicsDevice.DisplayMode.Height / 2);
                Rotation += new Vector3(-mouseDelta.Y, -mouseDelta.X, 0) * MouseSpeed;
                
                Mouse.SetPosition(game.GraphicsDevice.DisplayMode.Width / 2,
                    game.GraphicsDevice.DisplayMode.Height / 2);

                if (IsInDebugMode)
                {

                    if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                    {
                        Location += new Vector3(0, 1f * deltaTime, 0);
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
                    {
                        Location -= new Vector3(0, 1f * deltaTime, 0);
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        Location += TransformMatrix.Left * 1f * deltaTime;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        Location += TransformMatrix.Right * 1f * deltaTime;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        Location += TransformMatrix.Backward * 1f * deltaTime;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        Location += TransformMatrix.Forward * 1f * deltaTime;
                    }
                }
            }
        }
    }
}
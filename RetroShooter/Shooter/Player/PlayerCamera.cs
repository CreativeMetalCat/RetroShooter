using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RetroShooter.Engine;
using RetroShooter.Engine.Camera;

namespace RetroShooter.Shooter.Player
{
    /*
     * Player controlled camera that follows doom/Wolfenstein control type(camera moves only horizontally)
     */
    public class PlayerCamera : Camera
    {
        public float RotationSpeed = 1f;

        //If camera is in debug mode it also allows player to move around using WASD
        public bool IsInDebugMode = false;
        
        public PlayerCamera(string name, int id, RetroShooterGame game,float rotationSpeed = 1f,bool isInDebugMode = false, Vector3 location = default,
            Vector3 rotation = default, Vector3 scale = default, float fieldOfView = 90, Actor owner = null) : base(
            name, id, game, location, rotation, scale, fieldOfView, owner)
        {
            IsInDebugMode = isInDebugMode;
            RotationSpeed = rotationSpeed;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Rotation -= new Vector3(0, RotationSpeed * deltaTime,0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Rotation += new Vector3(0, RotationSpeed * deltaTime,0);
            }
            
            

            if (IsInDebugMode)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    Rotation += new Vector3(RotationSpeed * deltaTime, 0,0);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    Rotation -= new Vector3(RotationSpeed * deltaTime, 0,0);
                }
                
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
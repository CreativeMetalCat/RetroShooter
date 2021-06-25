using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroShooter.Engine;

namespace RetroShooter.Shooter
{
    public class TestCamera: Engine.Camera.Camera
    {
        
        public float MouseSpeed = 0.01f;

        public float KeyRotationSpeed = 1f;
        

        public TestCamera(string name, int id, RetroShooterGame game, Vector3 location = default, Vector3 rotation = default, Vector3 scale = default, float depthOfField = default, Actor owner = null) : base(name, id, game, location, rotation, scale, depthOfField, owner)
        {
           
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                location += 1 * deltaTime * ForwardVector;
                Target += 1 * deltaTime * ForwardVector;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                location -= 1 * deltaTime * ForwardVector;
                Target -= 1 * deltaTime * ForwardVector;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                location += 1 * deltaTime * RightVector;
                Target += 1 * deltaTime * RightVector;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                location -= 1 * deltaTime * RightVector;
                Target -= 1 * deltaTime * RightVector;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                rotation.X += KeyRotationSpeed* deltaTime;
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                rotation.X -= KeyRotationSpeed * deltaTime;
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                rotation.Y += KeyRotationSpeed * deltaTime;
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                rotation.Y -= KeyRotationSpeed * deltaTime;
            }

           
            if (!game.IsMouseVisible)
            {
                rotation.X +=MouseSpeed * deltaTime *
                              (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - Mouse.GetState().X);
                rotation.Y += MouseSpeed * deltaTime *
                                                   (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2 - Mouse.GetState().Y);
                Mouse.SetPosition(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2,
                    GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2);
            }
            game.AddDebugMessage(ForwardVector.ToString(), 0, Color.Crimson);
            game.AddDebugMessage("Rad: " + MathHelper.ToRadians(FieldOfView).ToString() + " Deg: " + FieldOfView, 0,
                Color.Cyan);
        }
    }
}
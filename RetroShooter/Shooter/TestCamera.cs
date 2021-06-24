using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RetroShooter.Engine;

namespace RetroShooter.Shooter
{
    public class TestCamera: Engine.Camera.Camera
    {
        public TestCamera(string name, int id, RetroShooterGame game, Vector3 location = default, Vector3 rotation = default, Vector3 scale = default, float depthOfField = default, Actor owner = null) : base(name, id, game, location, rotation, scale, depthOfField, owner)
        {
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                location.Y -= 1 * deltaTime;
                Target.Y -= 1 * deltaTime;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                location.Y += 1 * deltaTime;
                Target.Y += 1 * deltaTime;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                location.X += 1 * deltaTime;
                Target.X += 1 * deltaTime;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                location.X -= 1 * deltaTime;
                Target.X -= 1 * deltaTime;
            }
        }
    }
}
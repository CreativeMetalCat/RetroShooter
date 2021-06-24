using Microsoft.Xna.Framework;

namespace RetroShooter.Engine.Camera
{
    public class Camera : Actor
    {
        /**
         * Where does the camera point
         */
        public Vector3 Target;
        
        protected Matrix projectionMatrix;
        
        protected Matrix worldMatrix;

        protected float FieldOfView = 90f;

        public Camera(string name, int id , RetroShooterGame game , Vector3 location = default,
            Vector3 rotation = default, Vector3 scale = default,float fieldOfView  = 90f, Actor owner = null) :
            base(name, id, game, location, rotation, scale, owner)
        {
            FieldOfView = fieldOfView ;
        }

        public Matrix ProjectionMatrix => projectionMatrix;

        public Matrix ViewMatrix => Matrix.CreateLookAt(location - Vector3.Backward, location + ForwardVector, Vector3.Up);

        public Matrix WorldMatrix => worldMatrix;

        public override void Init()
        {
            base.Init();
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView
            (
                MathHelper.ToRadians(90),
                game.GraphicsDevice.DisplayMode.AspectRatio,
                1f,
                1000f
            );
            worldMatrix = Matrix.CreateWorld(Target, Vector3.Forward, Vector3.Up);
        }
    }
}
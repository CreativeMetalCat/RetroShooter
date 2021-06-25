using System;
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

        public Matrix ViewMatrix
        {
            get
            {
                float cosPitch = MathF.Cos(MathHelper.ToRadians(rotation.X));
                float sinPitch = MathF.Sin(MathHelper.ToRadians(rotation.X));

                float cosYaw = MathF.Cos(MathHelper.ToRadians(rotation.Y));
                float sinYaw = MathF.Sin(MathHelper.ToRadians(rotation.Y));

                Vector3 xAxis = new Vector3(cosYaw, 0, -sinYaw);
                Vector3 yAxis = new Vector3(sinYaw * sinPitch, cosPitch, cosYaw * sinPitch);
                Vector3 zAxis = new Vector3(sinYaw * cosPitch, -sinPitch, cosPitch * cosYaw);

               return  new Matrix
                (
                    new Vector4(xAxis.X,yAxis.X,zAxis.X,0),
                    new Vector4(xAxis.Y,yAxis.Y,zAxis.Y,0),
                    new Vector4(xAxis.Z,yAxis.Z,zAxis.Z,0),
                    new Vector4(-Vector3.Dot(xAxis,location),-Vector3.Dot(yAxis,location),-Vector3.Dot(zAxis,location),1)
                );
            }
        }

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
            worldMatrix = Matrix.CreateWorld(location, Vector3.Forward, Vector3.Up);
        }
    }
}
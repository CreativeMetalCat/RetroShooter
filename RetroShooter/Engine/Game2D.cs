using Box2D.NetStandard;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using Microsoft.Xna.Framework;
using RetroShooter.Engine.Basic;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = System.Numerics.Vector2;

namespace RetroShooter.Engine
{
    public class Game2D : RetroShooterGame
    {
        private Vector2 _worldGravity = new Vector2(0, 9.8f);

        public Vector2 WorldGravity => new Vector2(_worldGravity.X,_worldGravity.Y);
        
        public Vector2 WorldGravityB2D => _worldGravity;

        private Box2D.NetStandard.Dynamics.World.World _world;
        
        public Box2D.NetStandard.Dynamics.World.World PhysicsWorld => _world;

        protected int velocityInetarions = 6;

        protected int positionIterations = 2;

        protected Body testBody;

        protected Body testDynamicBody;

        protected override void LoadContent()
        {
            //generate world
            _world = new Box2D.NetStandard.Dynamics.World.World(_worldGravity);
            //do other loading next
            base.LoadContent();

            //load test actors
            //AddActor(new MeshActor("meshActor", LastActorId, this));

            BodyDef testBodyDef = new BodyDef();
            testBodyDef.position = new Vector2(0, -10);
            testBody = _world.CreateBody(testBodyDef);

            PolygonShape groundBox = new PolygonShape();
            groundBox.SetAsBox(20, 10);

            testBody.CreateFixture(groundBox,0f);

            BodyDef dynamicBodyDef = new BodyDef();
            dynamicBodyDef.type = BodyType.Dynamic;
            dynamicBodyDef.position = Vector2.One;
            
            testDynamicBody = _world.CreateBody(dynamicBodyDef);

            PolygonShape box = new PolygonShape();
            box.SetAsBox(1,1);
            
            FixtureDef dynBox = new  FixtureDef();
            dynBox.shape = box;
            dynBox.density = 1f;
            dynBox.friction = 0.5f;

            testDynamicBody.CreateFixture(dynBox);

            testDynamicBody.CreateFixture(dynBox);
        }

        protected override void Update(GameTime gameTime)
        {
            for (int i = 0; i < 60; ++i)
            {
                _world.Step(1f / 60f, velocityInetarions, positionIterations);
            }
            AddDebugMessage("X: " + testDynamicBody.GetPosition().X.ToString() + " Y: " + testDynamicBody.GetPosition().Y.ToString(),0,Color.Aquamarine);
            
            base.Update(gameTime);
        }
    }
}
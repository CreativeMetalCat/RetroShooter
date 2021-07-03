using System;
using RetroShooter.Engine;

namespace RetroShooter
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            //depending on what physics engine type does this project need different game types should be loaded
            //RetroShooterGame <- No physics at all
            //Game2D <-Box2d implementation of physics
            //Game3D <- Game that uses bepu engine(only as a possible idea no plans to impelment that)
            using (var game = new Game2D())
                game.Run();
        }
    }
}
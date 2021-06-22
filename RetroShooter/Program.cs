using System;

namespace RetroShooter
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new RetroShooterGame())
                game.Run();
        }
    }
}
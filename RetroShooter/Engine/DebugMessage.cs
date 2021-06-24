using System;
using Microsoft.Xna.Framework;

namespace RetroShooter
{
    public class DebugMessage
    {
        public string Message;
        /**
         * How long will the message stay on screen
         */
        public float Duration;

        public Color Color;
        
        public float CurrentLifeTime;

        public  DebugMessage(string msg, float dur, Color color)
        {
            Message = msg ?? throw new ArgumentNullException();
            Duration = dur;
            Color = color;
            CurrentLifeTime = 0;
        }
        
    }
}
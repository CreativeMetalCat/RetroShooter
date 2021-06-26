using System.Linq;
using Microsoft.Xna.Framework;

namespace RetroShooter.Engine.Helpers
{
    public static class XmlHelpers
    {
        public static float[] VectorStringToArray(string vecString)
        {
            return  vecString.Split(" ").Select(x => float.Parse(x)).ToArray();
        }

        public static Vector3 VectorStringToVec3(string vecString)
        {
            float[] vecStr = VectorStringToArray(vecString);
            return new Vector3(vecStr[0],vecStr[1],vecStr[2]);
        }
    }
}
using UnityEngine;

namespace Utils.Project.Scripts.Modules.Utils.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 SetY(this Vector3 vector, float y)
        {
            var newVector = vector;
            newVector.y = y;
            return newVector;
        }
        
        public static Vector3 ToXZVector3(this Vector2 vector)
        {
            return new Vector3(vector.x, 0, vector.y);
        }
    }
}
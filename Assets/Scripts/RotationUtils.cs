using UnityEngine;

namespace Assets.Scripts
{
    public static class RotationUtils
    {
        public static Quaternion DirectionVectorToRotationQuaternion2D(Vector3 vector)
        {
            float angle = Mathf.Atan2(-vector.x, vector.y) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class ShotgunWeapon : BasicWeapon
    {
        [Header("Shotgun Settings")]
        public float ScatterAngle;
        public int BulletCount;

        protected override void CreateSingleProjectile(Vector3 direction)
        {
            var weaponRotation = RotationUtils.DirectionVectorToRotationQuaternion2D(direction);
            var angle = UnityEngine.Random.Range(-ScatterAngle, ScatterAngle);
            var euler = weaponRotation.eulerAngles;
            var newRotation = Quaternion.Euler(euler.x, euler.y, euler.z + angle);
            var newDirection = newRotation * Vector3.up;
            base.CreateSingleProjectile(newDirection);
        }

        protected override void CreateProjectiles(Vector3 direction)
        {
            for (int i = 0; i < BulletCount; i++)
                CreateSingleProjectile(direction);
        }
    }
}

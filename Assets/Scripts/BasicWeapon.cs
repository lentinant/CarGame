﻿using UnityEngine;

namespace Assets.Scripts
{
    public class BasicWeapon : MonoBehaviour
    {
        [Header("Weapon Settings")]
        public string Name;
        public Transform ProjectilePrefab;
        public Transform Muzzle;
        public float ShootCooldown;

        [Header("Projectile Settings")]
        public float ProjectileSpeed;
        public int Damage;
        public float ThrowbackPower;

        public Collider2D Collider { get; set; }

        private float _nextShootTime;

        public virtual void CreateSingleProjectile(Vector3 direction)
        {
            var projectile = (Transform)Instantiate(ProjectilePrefab, Muzzle.position + new Vector3(0, 0, -1), transform.rotation);
            var projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.Init(direction, Collider, Damage, ProjectileSpeed, ThrowbackPower);
        }

        protected virtual void CreateProjectiles(Vector3 direction)
        {
            CreateSingleProjectile(direction);
        }

        public void Shoot(Vector3 direction)
        {
            if (Time.time < _nextShootTime)
                return;

            CreateProjectiles(direction);

            _nextShootTime = Time.time + ShootCooldown;
        }
    }
}

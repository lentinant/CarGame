using UnityEngine;

namespace Assets.Scripts
{
    public class ProjectileSource : MonoBehaviour
    {
        public string Name;
        public Transform ProjectilePrefab;
        public Transform Muzzle;
        public float ShootCooldown;

        private float _nextShootTime;

        public Collider2D Collider { get; set; }

        public void ShootProjectile(Vector3 direction)
        {
            if (Time.time < _nextShootTime)
                return;

            var projectile = (Transform)Instantiate(ProjectilePrefab, Muzzle.position + new Vector3(0, 0, -1), transform.rotation);
            var projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.Init(direction, Collider);

            _nextShootTime = Time.time + ShootCooldown;
        }
    }
}

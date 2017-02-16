using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts
{
    public class BasicEnemyShooter : MonoBehaviour
    {
        private Collider2D _collider;
        private List<ProjectileSource> _weapons;

        public float MaxAttackDistance;

        public float MaxShootingAngle;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();

            _weapons = transform.GetComponents<ProjectileSource>().ToList();
            _weapons.ForEach(p => p.Collider = _collider);
        }

        void Update()
        {
            var target = FollowOnSight.Target;

            var direction = target.position - transform.position;
            var currentAngle = Vector3.Angle(direction, transform.up);
            if (currentAngle > MaxShootingAngle)
                return;

            var distance = direction.magnitude;
            if (distance < MaxAttackDistance)
            {
                var hit = Physics2D.Linecast(transform.position, target.position, 1 << LayerMask.NameToLayer("Obstacle"));
                if (hit.collider != null)
                    return;
                _weapons.ForEach(p => p.ShootProjectile(transform.up));
            }
        }
    }
}

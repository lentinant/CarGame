using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class TurretController : MonoBehaviour
    {
        private Vector3 _prevMousePos;

        private Collider2D _collider;
        private List<ProjectileSource> _weapons;

        void Awake()
        {
            _prevMousePos = Input.mousePosition;
            _collider = transform.parent.GetComponent<Collider2D>();

            _weapons = transform.GetComponents<ProjectileSource>().ToList();
            _weapons.ForEach(p => p.Collider = _collider);
        }

        void Update()
        {
            UpdateRotation();

            if (Input.GetButton("Fire1"))
            {
                if (_weapons.Count > 0)
                    _weapons[0].ShootProjectile(transform.up);
            }

            if (Input.GetButton("Fire2"))
            {
                if (_weapons.Count > 1)
                    _weapons[1].ShootProjectile(transform.up);
            }
        }

        private void UpdateRotation()
        {
            var newDirection = GetRotationVector();
            if(newDirection.magnitude > 0)
            {
                var rotation = RotationUtils.DirectionVectorToRotationQuaternion2D(newDirection);
                transform.rotation = rotation;
            }
        }

        private Vector2 GetRotationVector()
        {
            var vInput = Input.GetAxis("AimingVertical");
            var hInput = Input.GetAxis("AimingHorizontal");

            Vector2 output = new Vector2(hInput, vInput).normalized;
            if (output.magnitude > 0)
                return output;

            if (Input.mousePosition == _prevMousePos)
                return output;

            var worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldMousePosition.z = 0;
            output = (worldMousePosition - transform.position).normalized;
            _prevMousePos = Input.mousePosition;
            return output;
        }
    }
}

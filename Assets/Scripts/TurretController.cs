﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class TurretController : MonoBehaviour
    {
        public float MaxRotationSpeed = 360;

        private Collider2D _collider;
        private List<BasicWeapon> _weapons;

        void Awake()
        {
            _collider = transform.parent.GetComponent<Collider2D>();

            _weapons = transform.GetComponents<BasicWeapon>().ToList();
            _weapons.ForEach(p => p.Collider = _collider);
        }

        void Update()
        {
            UpdateRotation();

            if (Input.GetButton("Fire1"))
            {
                if (_weapons.Count > 0)
                    _weapons[0].Shoot(transform.up);
            }

            if (Input.GetButton("Fire2"))
            {
                if (_weapons.Count > 1)
                    _weapons[1].Shoot(transform.up);
            }
        }

        private void UpdateRotation()
        {
            var newDirection = GetRotationVector();
            var targetRotation = RotationUtils.DirectionVectorToRotationQuaternion2D(newDirection);
            var newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, MaxRotationSpeed * Time.deltaTime);
            transform.rotation = newRotation;
        }

        private Vector2 GetRotationVector()
        {
            var vInput = Input.GetAxis("AimingVertical");
            var hInput = Input.GetAxis("AimingHorizontal");

            Vector2 output = new Vector2(hInput, vInput).normalized;

            var worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldMousePosition.z = 0;
            output = (worldMousePosition - transform.position).normalized;
            return output;
        }
    }
}

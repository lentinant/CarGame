using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class DriverController : MonoBehaviour
    {
        public float MovementSpeed = 1;

        public float MaxRotationSpeed = 45;

        public float CommonCameraSize = 5;
        public float AimingCameraSize = 4;

        public float AimingSpeed = 0.1f;

        public float AimingMovementSpeedScale = 0.5f;

        public float EnterCarDistance = 1;

        private Follower _cameraFollower;

        private TriggerDetection _interactionRange;

        private bool _isAiming = false;
        private SwitchManager _switchManager;

        private List<BasicWeapon> _weapons = new List<BasicWeapon>();

        void Awake()
        {
            _cameraFollower = Camera.main.GetComponent<Follower>();
            _interactionRange = GetComponentInChildren<TriggerDetection>();
            _switchManager = FindObjectOfType<SwitchManager>();
            _weapons = GetComponents<BasicWeapon>().ToList();
        }

        void Update()
        {
            var hInput = Input.GetAxisRaw("Horizontal");
            var vInput = Input.GetAxisRaw("Vertical");

            var movementDirection = new Vector2(hInput, vInput).normalized;

            var newAiming = Input.GetButton("Fire2");
            if (newAiming != _isAiming)
            {
                _isAiming = newAiming;

                _cameraFollower.SwitchFollow(transform, _isAiming ? AimingCameraSize : CommonCameraSize);
            }

            var movementSpeedScale = _isAiming ? AimingMovementSpeedScale : 1;

            transform.Translate(movementDirection * MovementSpeed * Time.deltaTime * movementSpeedScale, Space.World);

            UpdateRotation(movementDirection);

            if (Input.GetButtonDown("Use"))
            {
                Interact();
            }

            if(Input.GetButtonDown("Fire1") && _isAiming)
            {
                _weapons[0].Shoot(transform.up);
            }
        }

        private void UpdateRotation(Vector2 movementDirection)
        {
            Vector2 newDirection = _isAiming ? GetRotationVector() : movementDirection;

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

        public void ActivateCharacter(Vector2 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
            _cameraFollower.SwitchFollow(transform, CommonCameraSize);
        }

        public void DeactivateCharacter()
        {
            gameObject.SetActive(false);
        }

        private void Interact()
        {
            var itemToActivate = _interactionRange.Objects
                .Where(o => o.GetComponent<Interactible>() != null)
                .Select(o => o.GetComponent<Interactible>())
                .OrderBy(o => o.InteractPriority)
                .FirstOrDefault();

            if (itemToActivate != null)
                itemToActivate.Interact(this);
        }
    }
}

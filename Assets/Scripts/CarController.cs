using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class CarController : MonoBehaviour
    {
        public float Acceleration;
        public float Steering;

        public float CameraSize = 8;

        private Rigidbody2D _rigidbody;
        private float _vInput;
        private float _hInput;

        private DriverController _driver;
        private TurretController _turret;

        private List<TriggerDetection> _landingZones;

        private Follower _cameraFollower;
        private IEnumerable<WheelRotation> _wheels;
        private SwitchManager _switchManager;

        void Awake()
        {
            FollowOnSight.Target = transform;
            _driver = FindObjectOfType<DriverController>();
            _driver.gameObject.SetActive(false);
            _turret = GetComponentInChildren<TurretController>();
            _landingZones = GetComponentsInChildren<TriggerDetection>().ToList();
            _cameraFollower = Camera.main.GetComponent<Follower>();

            var interactible = GetComponent<Interactible>();
            interactible.OnInteraction += ActivateCar;

            _wheels = GetComponentsInChildren<WheelRotation>();

            _switchManager = FindObjectOfType<SwitchManager>();
        }

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            _hInput = -Input.GetAxisRaw("Horizontal");
            _vInput = Input.GetAxisRaw("Vertical");
            //if (_vInput == 0)
            //  _vInput = Input.GetButton("GamepadBrake") ? -1 : 0;

            if (Input.GetButtonDown("Use"))
                TryLand();
        }


        void FixedUpdate()
        {
            Vector2 speed = transform.up * (_vInput * Acceleration * _rigidbody.mass);
            _rigidbody.AddForce(speed);

            float direction = Vector2.Dot(_rigidbody.velocity, _rigidbody.GetRelativeVector(Vector2.up));
            if (direction >= 0.0f)
            {
                _rigidbody.rotation += _hInput * Steering * (_rigidbody.velocity.magnitude / 5.0f);
                //_rigidbody.AddTorque((_hInput * Steering) * (_rigidbody.velocity.magnitude / 10.0f));
            }
            else
            {
                _rigidbody.rotation -= _hInput * Steering * (_rigidbody.velocity.magnitude / 5.0f);
                //_rigidbody.AddTorque((-_hInput * Steering) * (_rigidbody.velocity.magnitude / 10.0f));
            }

            Vector2 forward = new Vector2(0.0f, 0.5f);
            float steeringRightAngle;
            if (_rigidbody.angularVelocity > 0)
            {
                steeringRightAngle = -90;
            }
            else
            {
                steeringRightAngle = 90;
            }

            Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
            Debug.DrawLine((Vector3)_rigidbody.position, (Vector3)_rigidbody.GetRelativePoint(rightAngleFromForward), Color.green);

            float driftForce = Vector2.Dot(_rigidbody.velocity, _rigidbody.GetRelativeVector(rightAngleFromForward.normalized));

            Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);

            Debug.DrawLine((Vector3)_rigidbody.position, (Vector3)_rigidbody.GetRelativePoint(relativeForce), Color.red);

            _rigidbody.AddForce(_rigidbody.GetRelativeVector(relativeForce));
        }

        public void UpdateControls(float vInput, float hInput)
        {
            _vInput = vInput;
            _hInput = hInput;
        }

        public void ActivateCar(DriverController driver)
        {
            _switchManager.NotifySwitch(false);

            driver.DeactivateCharacter();
            enabled = true;
            _turret.enabled = true;

            foreach (var wheel in _wheels)
                wheel.enabled = true;

            _cameraFollower.SwitchFollow(transform, CameraSize);
        }

        public void DeactivateCar()
        {
            enabled = false;
            _turret.enabled = false;

            foreach (var wheel in _wheels)
                wheel.enabled = false;
        }

        private void TryLand()
        {
            var freeLandingZone = _landingZones.FirstOrDefault(z => !z.Objects.Any());
            if(freeLandingZone != null)
            {
                DeactivateCar();
                _driver.ActivateCharacter(freeLandingZone.transform.position);
            }
        }
    } 
}

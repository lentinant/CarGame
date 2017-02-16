using UnityEngine;

namespace Assets.Scripts
{
    public class CarController : MonoBehaviour
    {
        public float Acceleration;
        public float Steering;

        private Rigidbody2D _rigidbody;
        private float _vInput;
        private float _hInput;

        void Awake()
        {
            FollowOnSight.Target = transform;
        }

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            _hInput = -Input.GetAxis("Horizontal");
            _vInput = Input.GetAxis("Vertical");
            //if (_vInput == 0)
            //  _vInput = Input.GetButton("GamepadBrake") ? -1 : 0;
        }


        void FixedUpdate()
        {
            Vector2 speed = transform.up * (_vInput * Acceleration);
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
    } 
}

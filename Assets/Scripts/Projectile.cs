using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Projectile : MonoBehaviour
    {
        public float Speed;
        public float MaxTravelDistance = 1000;
        public int Damage;

        [Range(0, 1)]
        public float ForceToDamage;

        private float _mass;

        public string[] IgnoreTags;

        public float FullTrailLength;

        private float _currentTravelDistance = 0;
        private Vector3 _prevPosition;

        private LineRenderer _lineRenderer;

        private bool _initialized = false;

        private Rigidbody2D _rigidbody;
        private Vector3 _direction;

        void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.sortingLayerName = "Objects";
            _lineRenderer.sortingOrder = 4;
            _lineRenderer.SetPosition(0, Vector3.zero);

            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Init(Vector2 direction, Collider2D parentCollider, float impulse)
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            Vector2 force;

            if (impulse > 0)
                force = direction * impulse;
            else
                force = direction * Speed;

            _rigidbody.AddForce(force);
            _prevPosition = transform.position;
            _initialized = true;

            _direction = direction;

            var col = GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(col, parentCollider, true);
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            var damageTaker = coll.transform.GetComponent<DamageTaker>();
            
            if (damageTaker != null && !IgnoreTags.Contains(coll.transform.tag))
            {
                var pulse = _rigidbody.velocity.magnitude * _rigidbody.mass;

                var damage = ForceToDamage * pulse;
                var pushForce = (1 - ForceToDamage) * pulse;

                var destroyed = damageTaker.TakeDamage((int)damage);

                if(!destroyed)
                {
                    var rigidbody = coll.transform.GetComponent<Rigidbody2D>();
                    if (rigidbody != null)
                    {
                        rigidbody.AddForce(pushForce * _direction.normalized);
                    }
                }
            }
                
            Destroy(gameObject);
        }

        void Update()
        {
            if (!_initialized)
                return;

            _currentTravelDistance += Vector3.Distance(_prevPosition, transform.position);
            if (_currentTravelDistance <= FullTrailLength)
                _lineRenderer.SetPosition(0, new Vector3(0, -_currentTravelDistance, 0));
            _prevPosition = transform.position;
            if (_currentTravelDistance >= MaxTravelDistance)
                Destroy(gameObject);
        }
    }
}

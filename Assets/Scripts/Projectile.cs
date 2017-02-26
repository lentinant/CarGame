using UnityEngine;

namespace Assets.Scripts
{
    public class Projectile : MonoBehaviour
    {
        public float MaxTravelDistance = 1000;

        public float FullTrailLength;

        private float _currentTravelDistance = 0;
        private Vector3 _prevPosition;

        private LineRenderer _lineRenderer;

        private bool _initialized = false;

        private Rigidbody2D _rigidbody;
        private Vector3 _direction;
        private int _damage;
        private float _throwbackPower;

        void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.sortingLayerName = "Objects";
            _lineRenderer.sortingOrder = 4;
            _lineRenderer.SetPosition(0, Vector3.zero);

            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Init(Vector2 direction, Collider2D parentCollider, int damage, float projectileSpeed, float throwbackPower)
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            _direction = direction;
            _damage = damage;
            _throwbackPower = throwbackPower;

            _rigidbody.AddForce(direction * projectileSpeed);
            _prevPosition = transform.position;
            _initialized = true;

            var col = GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(col, parentCollider, true);
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            Destroy(gameObject);

            var damageTaker = coll.transform.GetComponent<DamageTaker>();

            if (damageTaker != null && damageTaker.TakeDamage(_damage))
            {
                return;
            }

            var rigidbody = coll.transform.GetComponent<Rigidbody2D>();
            if (rigidbody != null)
            {
                var layer = coll.gameObject.layer;
                var hit = Physics2D.Raycast(transform.position, _direction, MaxTravelDistance, 1 << layer);
                rigidbody.AddForceAtPosition(_direction * _throwbackPower, hit.point);
            }
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

using UnityEngine;

namespace Assets.Scripts
{
    public class DamageTaker : MonoBehaviour
    {
        public int MaxHealth;
        private int _currentHealth;

        void Awake()
        {
            _currentHealth = MaxHealth;
        }

        public bool TakeDamage(int count)
        {
            _currentHealth -= count;

            Debug.Log(string.Format("Object {0} takes {1} damage, {2} hp left", gameObject.name, count, _currentHealth));
            if (_currentHealth <= 0)
            {
                Destroy(gameObject);
                return true;
            }

            return false;    
        }
    }
}

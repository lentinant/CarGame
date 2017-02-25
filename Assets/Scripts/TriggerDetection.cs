using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class TriggerDetection : MonoBehaviour
    {
        private List<Transform> _objectsInTrigger = new List<Transform>();

        public IEnumerable<Transform> Objects
        {
            get { return _objectsInTrigger; }
        }

        void OnTriggerEnter2D(Collider2D obj)
        {
            lock(_objectsInTrigger)
            {
                _objectsInTrigger.Add(obj.transform);
            }
        }

        void OnTriggerExit2D(Collider2D obj)
        {
            lock (_objectsInTrigger)
            {
                _objectsInTrigger.Remove(obj.transform); 
            }
        }

        public void ClearObjects()
        {
            _objectsInTrigger.Clear();
        }
    }
}

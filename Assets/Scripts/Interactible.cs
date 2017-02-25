using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Interactible : MonoBehaviour
    {
        public event Action<DriverController> OnInteraction;

        public int InteractPriority = 0;

        public void Interact(DriverController initiator)
        {
            if (OnInteraction != null)
                OnInteraction(initiator);
        }
    }
}

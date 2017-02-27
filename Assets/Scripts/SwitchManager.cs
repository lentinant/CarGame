using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class SwitchManager : MonoBehaviour
    {
        public Action<bool> OnSwitch;

        public void NotifySwitch(bool fromCarToDriver)
        {
            if (OnSwitch != null)
                OnSwitch(fromCarToDriver);
        }
    }
}

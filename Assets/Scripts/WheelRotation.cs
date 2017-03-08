using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class WheelRotation : MonoBehaviour
    {
        public float FullRotationAngle = 45;

        void Update()
        {
            var angle = -Input.GetAxis("Horizontal") * FullRotationAngle;

            transform.localEulerAngles = new Vector3(0, 0, angle);
        }
    }
}

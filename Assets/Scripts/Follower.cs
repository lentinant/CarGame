using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Follower : MonoBehaviour
    {
        public Vector3 PositionOffset;
        public Transform FollowObject;

        void Update()
        {
            transform.position = FollowObject.position + PositionOffset;
        }
    }
}

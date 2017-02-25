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
        public Transform FollowTarget;

        public float SwitchTargetTime = 0.5f;

        private Camera _camera;

        private bool _isSwitching;
        private float _currentSwitchingTime;
        private Vector3 _initialPosition;
        private float _initialCameraSize;
        private float _targetCameraSize;
        private float _customSwitchTargetTime;

        void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        void Update()
        {
            Vector3 newPosition = PositionOffset;

            if(_isSwitching)
            {
                _currentSwitchingTime += Time.deltaTime;

                var alpha = Mathf.Min(_currentSwitchingTime / _customSwitchTargetTime, 1);

                var newCameraSize = Mathf.Lerp(_initialCameraSize, _targetCameraSize, alpha);
                var newPos = Vector3.Lerp(_initialPosition, FollowTarget.position, alpha);

                _camera.orthographicSize = newCameraSize;

                newPosition += newPos;

                if (alpha.Equals(1))
                    _isSwitching = false;
            }
            else
            {
                newPosition += FollowTarget.position;
            }

            transform.position = FollowTarget.position + PositionOffset;
        }

        public void SwitchFollow(Transform newFollowObject, float newCameraSize, float customSwitchTargetTime = -1)
        {
            _customSwitchTargetTime = customSwitchTargetTime >= 0 ? customSwitchTargetTime : SwitchTargetTime;

            _isSwitching = true;
            _initialCameraSize = _camera.orthographicSize;
            _initialPosition = transform.position;

            _currentSwitchingTime = 0;

            _targetCameraSize = newCameraSize;

            FollowTarget = newFollowObject;
        }
    }
}

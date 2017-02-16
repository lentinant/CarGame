using UnityEngine;

namespace Assets.Scripts
{
    public class FollowOnSight : MonoBehaviour
    {
        public static Transform Target;

        public float MaxDetectionRange = 5;
        public float StopFollowDistance = 0.5f;

        public float MovementSpeed = 2f;

        public float RotationRate = 90f;

        void Update()
        {
            var distance = (transform.position - Target.position).magnitude;

            if(distance < MaxDetectionRange)
            {
                var direction = Target.position - transform.position;
                var expectedRotation = RotationUtils.DirectionVectorToRotationQuaternion2D(direction.normalized);
                var newRotation = Quaternion.RotateTowards(transform.rotation, expectedRotation, RotationRate * Time.deltaTime);
                transform.rotation = newRotation;

                if (distance < StopFollowDistance)
                    return;

                var hit = Physics2D.Linecast(transform.position, Target.position, 1 << LayerMask.NameToLayer("Obstacle"));
                if (hit.collider == null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, Target.position, MovementSpeed * Time.deltaTime);
                }
            }
        }
    }
}

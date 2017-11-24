using UnityEngine;

namespace Character
{
    public class Diagnostics : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody _rigidbody;
        public Vector3 _previousPosition;
        public Vector3 _previousVelocity;

        private void Start()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _previousPosition = _transform.position;
            _previousVelocity = _rigidbody.velocity;
        }

        private void Update()
        {
            if (Vector3.Distance(_previousPosition, _transform.position) > 1.0f)
            {
                Debug.Log(" == JUMP DETECTED AT " + Time.timeSinceLevelLoad + "! ==");
                Debug.Log("Velocity: " + _rigidbody.velocity + ", previously: " + _previousVelocity);
            }

            if (Vector3.Magnitude(_rigidbody.velocity) > 25.0f)
            {
                Debug.Log(" == VELOCITY SPIKE AT " + Time.timeSinceLevelLoad + "! ==");
                Debug.Log("Velocity: " + _rigidbody.velocity + ", previously: " + _previousVelocity);
            }

            _previousPosition = _transform.position;
            _previousVelocity = _rigidbody.velocity;
        }
    }
}
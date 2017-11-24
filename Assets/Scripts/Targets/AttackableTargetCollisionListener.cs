using UnityEngine;
using System;

namespace Targets
{
    public class AttackableTargetCollisionListener : MonoBehaviour, ITargetingBeacon
    {
        private Transform _transform;
        private int _hits;

        public TargetType TargetType { get { return TargetType.Attackable; } }
        public Vector3 Position { get { if (_transform == null) { _transform = transform; } return _transform.position; } }
        public bool IsValid { get { return gameObject.activeInHierarchy; } }

        public event Action<ITargetingBeacon> TargetRemovedEvent;

        private void Start()
        {
            _hits = UnityEngine.Random.Range(1, 4);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Damage Collider")
            {
                _hits -= 1;

                if (_hits < 1)
                {
                    gameObject.SetActive(false);

                    if (TargetRemovedEvent != null)
                    {
                        TargetRemovedEvent.Invoke(this);
                    }
                }
            }
        }
    }
}
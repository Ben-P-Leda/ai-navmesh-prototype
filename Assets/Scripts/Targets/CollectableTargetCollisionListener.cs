using UnityEngine;
using System;

namespace Targets
{
    public class CollectableTargetCollisionListener : MonoBehaviour, ITargetingBeacon
    {
        private Transform _transform;

        public TargetType TargetType { get { return TargetType.Collectable; } }
        public Vector3 Position { get { if (_transform == null) { _transform = transform; } return _transform.position; } }
        public bool IsValid { get { return gameObject.activeInHierarchy; } }

        public event Action<ITargetingBeacon> TargetRemovedEvent;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Player")
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
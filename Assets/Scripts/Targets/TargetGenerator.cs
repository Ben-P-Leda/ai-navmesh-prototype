using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Targets
{
    public class TargetGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _targetPrefab;

        private TargetOrchestrator _orchestrator;
        private List<ITargetingBeacon> _beacons = new List<ITargetingBeacon>();

        public event Action NewTargetGeneratedEvent;

        public ITargetingBeacon GetNearestTarget(Transform fromTransform)
        {
            ITargetingBeacon target = null;

            for (int i = 0; i < _beacons.Count; i++)
            {
                if ((target == null) || (Vector3.Distance(fromTransform.position, _beacons[i].Position) < Vector3.Distance(fromTransform.position, target.Position)))
                {
                    target = _beacons[i];
                }
            }

            return target;
        }

        private void Start()
        {
            _orchestrator = FindObjectOfType<TargetOrchestrator>();

            StartCoroutine(CreateNewTarget());
        }

        private IEnumerator CreateNewTarget()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1, 7));

            Vector3 newTargetPosition = GetNewTargetPosition();

            if (newTargetPosition.x > float.MinValue)
            {
                GameObject newTarget = Instantiate(_targetPrefab, newTargetPosition, Quaternion.Euler(Vector3.zero), transform);

                ITargetingBeacon beacon = newTarget.GetComponent<ITargetingBeacon>();
                beacon.TargetRemovedEvent += HandleTargetRemovedEvent;
                _beacons.Add(beacon);
                _orchestrator.RegisterBeacon(beacon);

                if (NewTargetGeneratedEvent != null)
                {
                    NewTargetGeneratedEvent.Invoke();
                }
            }

            StartCoroutine(CreateNewTarget());
        }

        private void HandleTargetRemovedEvent(ITargetingBeacon beacon)
        {
            _beacons.Remove(beacon);
            _orchestrator.DeregisterBeacon(beacon);
        }

        private Vector3 GetNewTargetPosition()
        {
            Vector3 position = new Vector3(UnityEngine.Random.Range(-7.5f, 7.5f), 0.0f, UnityEngine.Random.Range(-7.5f, 7.5f));

            if (!_orchestrator.ValidTargetLocation(position))
            {
                position.x = float.MinValue;
            }

            return position;
        }
    }
}
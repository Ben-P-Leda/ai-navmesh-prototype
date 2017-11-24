using UnityEngine;
using System.Collections.Generic;
using Targets;

namespace Agent
{
    public class AgentTargetLocator : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _targetGeneratorObjects;

        private TargetGenerator[] _targetGenerators;
        private AgentNavigator _navigator;

        public ITargetingBeacon GetNearestTarget(Transform fromTransform)
        {
            List<ITargetingBeacon> potentialTargets = new List<ITargetingBeacon>();
            for (int i=0; i<_targetGenerators.Length; i++)
            {
                ITargetingBeacon target = _targetGenerators[i].GetNearestTarget(fromTransform);
                if (target != null)
                {
                    potentialTargets.Add(target);
                }
            }

            ITargetingBeacon nearestTarget = null;
            foreach(ITargetingBeacon target in potentialTargets)
            {
                if ((nearestTarget == null) || (Vector3.Distance(fromTransform.position, target.Position) < Vector3.Distance(fromTransform.position, nearestTarget.Position)))
                {
                    nearestTarget = target;
                }
            }

            return nearestTarget;
        }

        private void Start()
        {
            _targetGenerators = new TargetGenerator[_targetGeneratorObjects.Count];
            for (int i = 0; i < _targetGenerators.Length; i++)
            {
                _targetGenerators[i] = _targetGeneratorObjects[i].GetComponent<TargetGenerator>();
                _targetGenerators[i].NewTargetGeneratedEvent += HandleNewTargetGeneratedEvent;
            }

            _navigator = GetComponent<AgentNavigator>();
        }

        private void HandleNewTargetGeneratedEvent()
        {
            _navigator.ReactToPotentialNewTarget();
        }
    }
}
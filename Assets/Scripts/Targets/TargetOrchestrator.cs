using UnityEngine;
using System.Collections.Generic;

namespace Targets
{
    public class TargetOrchestrator : MonoBehaviour
    {
        private List<ITargetingBeacon> _beacons = new List<ITargetingBeacon>();

        public bool ValidTargetLocation(Vector3 proposedLocation)
        {
            bool validLocation = true;

            for (int i=0; ((i < _beacons.Count) && (validLocation)); i++)
            {
                if (Vector3.Distance(proposedLocation, _beacons[i].Position) < 1.5f)
                {
                    validLocation = false;
                }
            }

            return validLocation;
        }

        public void RegisterBeacon(ITargetingBeacon beacon)
        {
            _beacons.Add(beacon);
        }

        public void DeregisterBeacon(ITargetingBeacon beacon)
        {
            _beacons.Remove(beacon);
        }
    }
}
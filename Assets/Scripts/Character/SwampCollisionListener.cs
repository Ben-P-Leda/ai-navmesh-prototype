using UnityEngine;
using System;

namespace Character
{
    public class SwampCollisionListener : MonoBehaviour
    {
        public event Action<bool> SwampEntryEvent;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Swamp")
            {
                SwampEntryEvent.Invoke(true);
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.tag == "Swamp")
            {
                SwampEntryEvent.Invoke(false);
            }
        }
    }
}
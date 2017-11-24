using UnityEngine;
using System;

namespace Character
{
    public class AnimationCallbackHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject _damageCollider;

        public event Action AttackMovementStartEvent;
        public event Action AttackMovementEndEvent;

        private void Start()
        {
            _damageCollider.SetActive(false);
        }

        private void BeginAttackMovement()
        {
            AttackMovementStartEvent.Invoke();
        }

        private void EndAttackMovement()
        {
            AttackMovementEndEvent.Invoke();
        }

        private void ActivatePrimaryDamageCollider()
        {
            _damageCollider.SetActive(true);
        }

        private void DeactivateDamageColliders()
        {
            _damageCollider.SetActive(false);
        }
    }
}

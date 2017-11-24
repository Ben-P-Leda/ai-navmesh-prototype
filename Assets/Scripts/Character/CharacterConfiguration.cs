using UnityEngine;
using System;

namespace Character
{
    [Serializable]
    public class CharacterConfiguration
    {
        [SerializeField]
        private float _basicMovementSpeed;
        public float BasicMovementSpeed {  get { return _basicMovementSpeed; } }

        [SerializeField]
        private float _attackSlideMovementSpeed;
        public float AttackSlideMovementSpeed { get { return _attackSlideMovementSpeed; } }
    }
}
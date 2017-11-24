using UnityEngine;

namespace Character.Actuators
{
    public class CharacterAnimationActuator
    {
        private Animator _animator;

        public CharacterAnimationActuator(Animator animator)
        {
            _animator = animator;
        }

        public void Actuate(CharacterState state, bool enteredNewState)
        {
            switch (state)
            {
                case CharacterState.Attacking: ActuateForAttacking(enteredNewState); break;
                case CharacterState.Moving: ActuateForMoving(); break;
                case CharacterState.Idle: ActuateForIdle(); break;
            }
        }

        private void ActuateForAttacking(bool enteredNewState)
        {
            if (enteredNewState)
            {
                _animator.SetTrigger("Attack");
            }
        }

        private void ActuateForMoving()
        {
            _animator.SetBool("Walking", true);
        }

        private void ActuateForIdle()
        {
            _animator.SetBool("Walking", false);
        }
    }
}
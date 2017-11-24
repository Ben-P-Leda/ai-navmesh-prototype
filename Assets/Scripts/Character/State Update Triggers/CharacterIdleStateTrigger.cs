using UnityEngine;
using Foundation;

namespace Character.StateUpdateTriggers
{
    public class CharacterIdleStateTrigger
    {
        private ICommandProvider _commandProvider;
        private bool _animatorEnteredTriggerState;

        public CharacterIdleStateTrigger(ICommandProvider commandProvider, Animator animator)
        {
            _commandProvider = commandProvider;
            _animatorEnteredTriggerState = false;

            animator.GetBehaviour<AnimationStateChangeHandler>().EnterStateEvent += HandleEnterStateEvent;
        }

        public CharacterState HandleStateUpdate(CharacterState state)
        {
            if (ShouldEnterIdleState(state))
            {
                state = CharacterState.Idle;
            }

            _animatorEnteredTriggerState = false;

            return state;
        }

        private void HandleEnterStateEvent()
        {
            _animatorEnteredTriggerState = true;
        }

        private bool ShouldEnterIdleState(CharacterState state)
        {
            if ((state == CharacterState.Moving) && (!_commandProvider.Moving)) { return true; }
            if (_animatorEnteredTriggerState) { return true; }

            return false;
        }
    }
}
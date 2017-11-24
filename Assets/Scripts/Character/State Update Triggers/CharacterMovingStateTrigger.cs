using Foundation;

namespace Character.StateUpdateTriggers
{
    public class CharacterMovingStateTrigger
    {
        private ICommandProvider _commandProvider;

        public CharacterMovingStateTrigger(ICommandProvider commandProvider)
        {
            _commandProvider = commandProvider;
        }

        public CharacterState HandleStateUpdate(CharacterState state)
        {
            if ((_commandProvider.Moving) && (CanEnterMovingState(state)))
            {
                state = CharacterState.Moving;
            }

            return state;
        }

        private bool CanEnterMovingState(CharacterState state)
        {
            if (state == CharacterState.Attacking) { return false; }

            return true;
        }
    }
}
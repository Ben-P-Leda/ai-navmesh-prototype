using Foundation;

namespace Character.StateUpdateTriggers
{
    public class CharacterAttackingStateTrigger
    {
        private ICommandProvider _commandProvider;

        public CharacterAttackingStateTrigger(ICommandProvider commandProvider)
        {
            _commandProvider = commandProvider;
        }

        public CharacterState HandleStateUpdate(CharacterState state)
        {
            if (_commandProvider.Attacking)
            {
                state = CharacterState.Attacking;
            }

            return state;
        }
    }
}
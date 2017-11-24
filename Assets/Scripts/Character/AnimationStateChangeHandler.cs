using UnityEngine;
using System;

namespace Character
{
    public class AnimationStateChangeHandler : StateMachineBehaviour
    {
        public event Action EnterStateEvent;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (EnterStateEvent != null)
            {
                EnterStateEvent.Invoke();
            }
        }
    }
}
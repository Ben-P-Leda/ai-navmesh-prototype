using UnityEngine;
using Foundation;

namespace Agent
{
    public class AgentCommandProvider : MonoBehaviour, ICommandProvider
    {
        public bool InputFromHuman { get { return false; } }
        public bool Moving { get; private set; }
        public bool Attacking { get; private set; }
        public float XMovement {  get { return 0.0f; } }
        public float ZMovement { get { return 0.0f; } }

        private void Start()
        {
            GetComponent<IAgentAI>().SetActionEvent += HandleSetActionEvent;
        }

        private void HandleSetActionEvent(AgentBehaviourAction action)
        {
            Moving = false;
            Attacking = false;

            switch (action)
            {
                case AgentBehaviourAction.MoveToTarget: Moving = true; break;
                case AgentBehaviourAction.BasicAction: Attacking = true; break;
                case AgentBehaviourAction.MovingBasicAction: Moving = true; Attacking = true; break;
            }
        }
    }
}
using System;
using Targets;

namespace Agent
{
    public interface IAgentAI
    {
        bool enabled { set; }

        event Action<ITargetingBeacon> SetTargetEvent;
        event Action<AgentBehaviourAction> SetActionEvent;
    }
}

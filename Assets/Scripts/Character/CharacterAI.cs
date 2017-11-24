using UnityEngine;
using System;
using Agent;
using Targets;

namespace Character
{
    public class CharacterAI : MonoBehaviour, IAgentAI
    {
        private Transform _transform;
        private ITargetingBeacon _target;
        private AgentBehaviourAction _currentAction;
        private AgentTargetLocator _targetLocator;

        public event Action<ITargetingBeacon> SetTargetEvent;
        public event Action<AgentBehaviourAction> SetActionEvent;

        private void Start()
        {
            _transform = transform;
            _target = null;
            _currentAction = AgentBehaviourAction.Idle;
            _targetLocator = GetComponent<AgentTargetLocator>();
        }

        private void Update()
        {
            if (!CurrentTargetIsValid())
            {
                SelectNewTarget();
            }

            SelectBehaviour();
        }

        private bool CurrentTargetIsValid()
        {
            if (_target == null) { return false; }
            if (!_target.IsValid) { return false; }

            return true;
        }

        private void SelectNewTarget()
        {
            _target = _targetLocator.GetNearestTarget(_transform);
            SetTargetEvent.Invoke(_target);
        }

        private void SelectBehaviour()
        {
            AgentBehaviourAction selectedAction = AgentBehaviourAction.Idle;

            if (CurrentTargetIsValid())
            {
                float distanceToTarget = Vector3.Distance(_target.Position, _transform.position);
                if (BehaviourShouldBeMoveToTarget(distanceToTarget))
                {
                    selectedAction = AgentBehaviourAction.MoveToTarget;
                }
                else if (BehaviourShouldBeMovingBasicAction(distanceToTarget))
                {
                    selectedAction = AgentBehaviourAction.MovingBasicAction;
                }
                else
                {
                    selectedAction = AgentBehaviourAction.BasicAction;
                }
            }

            if (selectedAction != _currentAction)
            {
                _currentAction = selectedAction;
                if (SetActionEvent != null)
                {
                    SetActionEvent.Invoke(_currentAction);
                }
            }
        }

        private bool BehaviourShouldBeMoveToTarget(float distanceToTarget)
        {
            if ((_target.TargetType == TargetType.Attackable) && (distanceToTarget > 3.0f)) { return true; }
            if ((_target.TargetType == TargetType.Collectable) && (distanceToTarget > 0.25f)) { return true; }

            return false;
        }

        private bool BehaviourShouldBeMovingBasicAction(float distanceToTarget)
        {
            if ((_target.TargetType == TargetType.Attackable) && (distanceToTarget > 1.0f)) { return true; }

            return false;
        }
    }
}
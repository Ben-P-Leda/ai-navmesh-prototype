using UnityEngine;
using UnityEngine.AI;
using Agent;
using Character.Actuators;
using Character.StateUpdateTriggers;
using Foundation;
using Targets;
using System;

namespace Character
{
    public class CharacterOrchestrator : MonoBehaviour, ITargetingBeacon
    {
        [SerializeField]
        private bool _isHumanControlled;
        [SerializeField]
        private CharacterConfiguration _configuration;

        private CharacterAttackingStateTrigger _attackingStateTrigger;
        private CharacterMovingStateTrigger _movingStateTrigger;
        private CharacterIdleStateTrigger _idleStateTrigger;

        private CharacterMovementActuator _movementActuator;
        private CharacterAnimationActuator _animationActuator;

        private CharacterState _currentState;
        private Transform _transform;

        public event Action<ITargetingBeacon> TargetRemovedEvent;

        public TargetType TargetType { get { return TargetType.Player; } }
        public Vector3 Position {  get { return _transform.position; } }
        public bool IsValid {  get { return false; } }

        private void Start()
        {
            _transform = transform;

            ICommandProvider commandProvider = _isHumanControlled ? ActivateHumanControl() : ActivateAgentControl();
            Animator animator = GetComponentInChildren<Animator>();

            _idleStateTrigger = new CharacterIdleStateTrigger(commandProvider, animator);
            _movingStateTrigger = new CharacterMovingStateTrigger(commandProvider);
            _attackingStateTrigger = new CharacterAttackingStateTrigger(commandProvider);

            _movementActuator = new CharacterMovementActuator(transform, GetComponent<Rigidbody>(), GetComponent<AgentNavigator>(), GetComponent<IAgentAI>(), GetComponent<SwampCollisionListener>(),
                commandProvider, _configuration);
            _animationActuator = new CharacterAnimationActuator(animator);

            _currentState = CharacterState.Idle;

            FindObjectOfType<TargetOrchestrator>().RegisterBeacon(this);
        }

        private ICommandProvider ActivateAgentControl()
        {
            GetComponent<PlayerCommandProvider>().enabled = false;

            GetComponent<NavMeshAgent>().enabled = true;

            return GetComponent<AgentCommandProvider>();
        }

        private ICommandProvider ActivateHumanControl()
        {
            GetComponent<AgentCommandProvider>().enabled = false;
            GetComponent<AgentNavigator>().enabled = false;
            GetComponent<IAgentAI>().enabled = false;
            GetComponent<AgentTargetLocator>().enabled = false;

            GetComponent<NavMeshObstacle>().enabled = true;

            return GetComponent<PlayerCommandProvider>();
        }

        private void Update()
        {
            bool stateWasUpdated = CheckForStateChange();

            _movementActuator.Actuate(_currentState, stateWasUpdated);
            _animationActuator.Actuate(_currentState, stateWasUpdated);
        }

        private bool CheckForStateChange()
        {
            CharacterState previousState = _currentState;

            _currentState = _attackingStateTrigger.HandleStateUpdate(_currentState);
            _currentState = _movingStateTrigger.HandleStateUpdate(_currentState);
            _currentState = _idleStateTrigger.HandleStateUpdate(_currentState);

            return (previousState != _currentState);
        }
    }
}
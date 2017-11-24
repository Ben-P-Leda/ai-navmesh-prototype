using UnityEngine;
using Agent;
using Foundation;
using Targets;

namespace Character.Actuators
{
    public class CharacterMovementActuator
    {
        private Transform _transform;
        private Rigidbody _rigidbody;
        private AgentNavigator _agentNavigator;
        private ICommandProvider _commandProvider;
        private CharacterConfiguration _configuration;
        private ITargetingBeacon _target;
        private bool _attackMovementCanStart;
        private bool _attackMovementMustEnd;
        private Vector3 _fixedMovementDirection;
        private float _fixedMovementSpeed;
        private float _movementSpeedModifier;

        public CharacterMovementActuator(Transform transform, Rigidbody rigidbody, AgentNavigator agentNavigator, IAgentAI agentAI, SwampCollisionListener swampCollisionListener,
            ICommandProvider commandProvider, CharacterConfiguration configuration)
        {
            _transform = transform;
            _rigidbody = rigidbody;
            _agentNavigator = agentNavigator;
            _commandProvider = commandProvider;
            _configuration = configuration;

            _attackMovementCanStart = false;
            _attackMovementMustEnd = false;
            _movementSpeedModifier = 1.0f;

            AnimationCallbackHandler animationCallbackHandler = _transform.GetComponentInChildren<AnimationCallbackHandler>();
            animationCallbackHandler.AttackMovementStartEvent += HandleAttackMovementStartEvent;
            animationCallbackHandler.AttackMovementEndEvent += HandleAttackMovementEndEvent;

            agentAI.SetTargetEvent += HandleSetTargetEvent;

            swampCollisionListener.SwampEntryEvent += HandleSwampEntryEvent;
        }

        public void HandleSetTargetEvent(ITargetingBeacon target)
        {
            _target = target;
        }

        private void HandleAttackMovementStartEvent()
        {
            _attackMovementCanStart = true;
            _attackMovementMustEnd = false;
        }

        private void HandleAttackMovementEndEvent()
        {
            _attackMovementMustEnd = true;
        }

        private void HandleSwampEntryEvent(bool isInSwamp)
        {
            _movementSpeedModifier = isInSwamp ? 0.5f : 1.0f;
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
            SetNavigatorActive(false);

            if (_attackMovementCanStart)
            {
                _attackMovementCanStart = false;

                if (_commandProvider.Moving)
                {
                    _fixedMovementDirection = GetDirection();
                    _fixedMovementSpeed = _configuration.AttackSlideMovementSpeed * _movementSpeedModifier;
                }
            }
            else if (_attackMovementMustEnd)
            {
                _fixedMovementDirection = Vector3.zero;
                _fixedMovementSpeed = 0.0f;
            }

            SetVelocity(_fixedMovementDirection, _fixedMovementSpeed);
        }

        private void SetNavigatorActive(bool isActive)
        {
            if (!_commandProvider.InputFromHuman)
            {
                _agentNavigator.SetNavigatorActive(isActive);
            }
        }

        private void SetNavigatorSpeed(float speed)
        {
            if (!_commandProvider.InputFromHuman)
            {
                _agentNavigator.SetNavigatorSpeed(speed);
            }
        }

        private Vector3 GetDirection()
        {
            Vector3 direction = Vector3.zero;

            if (_commandProvider.InputFromHuman)
            {
                direction.x = _commandProvider.XMovement;
                direction.z = _commandProvider.ZMovement;
            }
            else
            {
                direction.x = (_target.Position - _transform.position).x;
                direction.z = (_target.Position - _transform.position).z;
            }

            return direction.normalized;
        }

        private void SetVelocity(Vector3 direction, float speed)
        {
            if ((direction.x != 0.0f) || (direction.z != 0.0f))
            {
                _transform.LookAt(_transform.position + direction);
            }
            _rigidbody.velocity = new Vector3(direction.x * speed, _rigidbody.velocity.y, direction.z * speed);
        }

        private void ActuateForMoving()
        {
            SetNavigatorSpeed(_configuration.BasicMovementSpeed * _movementSpeedModifier);
            SetNavigatorActive(true);

            if (_commandProvider.InputFromHuman)
            {
                Vector3 direction = GetDirection();
                SetVelocity(direction, _configuration.BasicMovementSpeed * _movementSpeedModifier);
            }
        }

        private void ActuateForIdle()
        {
            SetNavigatorActive(false);
            _fixedMovementDirection = Vector3.zero;
            _fixedMovementSpeed = 0.0f;

            SetVelocity(_fixedMovementDirection, _fixedMovementSpeed);
        }
    }
}
using UnityEngine;
using UnityEngine.AI;
using Targets;

namespace Agent
{
    public class AgentNavigator : MonoBehaviour
    {
        private ITargetingBeacon _target;
        private NavMeshAgent _navMeshAgent;
        private bool _navigatorRunning;
        private Vector3 _previousTargetPosition;
        private RigidbodyConstraints _defaultConstraints;
        private Rigidbody _rigidBody;

        public void SetNavigatorActive(bool isActive)
        {
            if ((!isActive) && (_navigatorRunning))
            {
                _navigatorRunning = false;
                _navMeshAgent.isStopped = true;
                _rigidBody.constraints = _defaultConstraints;
            }
            else if ((isActive) && (_target != null) && (!_navigatorRunning))
            {
                _navigatorRunning = true;
                _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
                UpdatePathToTarget();
                _navMeshAgent.isStopped = false;
            }
        }

        public void SetNavigatorSpeed(float speed)
        {
            _navMeshAgent.speed = speed;
        }

        public void ReactToPotentialNewTarget()
        {
            if ((_navigatorRunning) && (_target != null))
            {
                UpdatePathToTarget();
            }
        }

        private void UpdatePathToTarget()
        {
            _navMeshAgent.SetDestination(_target.Position);
        }

        private void Start()
        {
            _navigatorRunning = false;

            _navMeshAgent = GetComponent<NavMeshAgent>();

            GetComponent<IAgentAI>().SetTargetEvent += HandleSetTargetEvent;

            _rigidBody = GetComponent<Rigidbody>();
            _defaultConstraints = _rigidBody.constraints;
        }

        private void HandleSetTargetEvent(ITargetingBeacon target)
        {
            _target = target;
            if (_target != null)
            {
                _previousTargetPosition = target.Position;

                if (_navigatorRunning)
                {
                    UpdatePathToTarget();
                }
            }
        }

        private void Update()
        {
            if ((_navigatorRunning) && (_target != null) && (Vector3.Distance(_target.Position, _previousTargetPosition) > 1.0f))
            {
                UpdatePathToTarget();

                _previousTargetPosition = _target.Position;
            }
        }
    }
}
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace MH.Games.RTS
{
    public class UnitMove : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent _agent = null;
        [SerializeField] private Targeting _target;
        [SerializeField] private UnitAttack _unitAttack;
        private Camera _mainCamera;

        #region Server
        [ServerCallback]
        private void Update()
        {
            var attackedTarget = _target.GetAttackedTarget();

            if(attackedTarget != null)
            {
                if((attackedTarget.transform.position - transform.position).sqrMagnitude > 
                    Mathf.Pow(_unitAttack.FireRange,2))
                {
                    _agent.SetDestination(attackedTarget.transform.position);
                }
                else if (_agent.hasPath)
                {
                    _agent.ResetPath();
                }
                return;
            }
            if (!_agent.hasPath) return;
            if (_agent.remainingDistance > _agent.stoppingDistance) return;
            _agent.ResetPath();
        }

        [Command]
        public void CommandMove(Vector3 position)
        {
            _target.ClearTarget();

            if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

            _agent.SetDestination(hit.position);
        }

        #endregion
    }
}


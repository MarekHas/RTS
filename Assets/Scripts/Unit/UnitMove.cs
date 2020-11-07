using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace MH.Games.RTS
{
    public class UnitMove : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent _agent = null;

        private Camera _mainCamera;

        #region Server

        [Command]
        public void CommandMove(Vector3 position)
        {
            if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

            _agent.SetDestination(hit.position);
        }

        #endregion
    }
}


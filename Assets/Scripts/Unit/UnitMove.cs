﻿using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace MH.Games.RTS
{
    public class UnitMove : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent _agent = null;
        [SerializeField] private Target _target;
        private Camera _mainCamera;

        #region Server
        [ServerCallback]
        private void Update()
        {
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


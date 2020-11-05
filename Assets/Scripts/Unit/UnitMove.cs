using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace MH.Games.RTS
{
    public class UnitMove : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent _agent = null;

        private Camera _mainCamera;

        #region Server

        [Command]
        private void CommandMove(Vector3 position)
        {
            if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

            _agent.SetDestination(hit.position);
        }

        #endregion

        #region Client

        public override void OnStartAuthority()
        {
            _mainCamera = Camera.main;
        }

        [ClientCallback]
        private void Update()
        {
            if (!hasAuthority) { return; }

            if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }

            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

            CommandMove(hit.point);
        }

        #endregion

    }
}


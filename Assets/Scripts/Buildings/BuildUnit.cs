using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MH.Games.RTS
{
    public class BuildUnit : NetworkBehaviour, IPointerClickHandler
    {
        [SerializeField] private Health _health; 
        [SerializeField] private GameObject _unit = null;
        [SerializeField] private Transform _buildPoint = null;

        #region Server

        public override void OnStartServer()
        {
            _health.OnDie += OnBuildingDestroyed;
        }

        public override void OnStopServer()
        {
            _health.OnDie -= OnBuildingDestroyed;
        }

        [Server]
        private void OnBuildingDestroyed()
        {
            NetworkServer.Destroy(gameObject);
        }

        [Command]
        private void CommandSpawnUnit()
        {
            GameObject unitInstance = Instantiate(
                _unit,
                _buildPoint.position,
                _buildPoint.rotation);

            NetworkServer.Spawn(unitInstance, connectionToClient);
        }
        #endregion

        #region Client
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) { return; }

            if (!hasAuthority) { return; }

            CommandSpawnUnit();
        }
        #endregion
    }
}


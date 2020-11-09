using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

namespace MH.Games.RTS
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField]private List<Unit> _playerUnits = new List<Unit>();

        public List<Unit> PlayerUnits { get => _playerUnits; private set => _playerUnits = value; }

        #region Server
        public override void OnStartServer()
        {
            Unit.OnSpawnedUnit_Server += SpawnedUnitServerHandler;
            Unit.OnDespawnedUnit_Server += DespawnedUnitServerHandler;
        }

        public override void OnStopServer()
        {
            Unit.OnSpawnedUnit_Server -= SpawnedUnitServerHandler;
            Unit.OnDespawnedUnit_Server -= DespawnedUnitServerHandler;
        }

        private void SpawnedUnitServerHandler(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }
       
            PlayerUnits.Add(unit);
        }

        private void DespawnedUnitServerHandler(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

            PlayerUnits.Remove(unit);
        }
        #endregion

        #region Client
        public override void OnStartAuthority()
        {
            if (NetworkServer.active) { return; }

            Unit.OnSpawnedUnit_Client += SpawnedUnitClientHandler;
            Unit.OnDespawnedUnit_Client += DespawnedUnitClientHandler;
        }

        public override void OnStopClient()
        {
            if (!isClientOnly || !hasAuthority) return;

            Unit.OnSpawnedUnit_Client -= SpawnedUnitClientHandler;
            Unit.OnDespawnedUnit_Client -= DespawnedUnitClientHandler;
        }

        public void SpawnedUnitClientHandler(Unit unit)
        {
            PlayerUnits.Add(unit);
        }

        public void DespawnedUnitClientHandler(Unit unit)
        {
            PlayerUnits.Remove(unit);
        }
        #endregion
    }
}
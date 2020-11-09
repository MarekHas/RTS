using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace MH.Games.RTS
{
    public class Unit : NetworkBehaviour
    {
        [SerializeField] private UnitMove _unitMove = null;
        [SerializeField] private Target _target = null;
        [SerializeField] private UnityEvent _onSelected = null;
        [SerializeField] private UnityEvent _onDeselected = null;
        public static event Action<Unit> OnSpawnedUnit_Server;
        public static event Action<Unit> OnDespawnedUnit_Server;
        public static event Action<Unit> OnSpawnedUnit_Client;
        public static event Action<Unit> OnDespawnedUnit_Client;
        #region Server
        public override void OnStartServer()
        {
            OnSpawnedUnit_Server?.Invoke(this);
        }

        public override void OnStopServer()
        {
            OnDespawnedUnit_Server?.Invoke(this);
        }
        #endregion

        #region Client

        public override void OnStartClient()
        {
            if (!isClientOnly || !hasAuthority) return;
            
            OnSpawnedUnit_Client?.Invoke(this);
        }

        public override void OnStopClient()
        {
            if (!isClientOnly || !hasAuthority) return;

            OnDespawnedUnit_Client?.Invoke(this);
        }

        public UnitMove GetUnitMove()
        {
            return _unitMove;
        }
        public Target GetTarget()
        {
            return _target;
        }
        [Client]
        public void SelectUnit()
        {
            if (!hasAuthority) { return; }

            _onSelected?.Invoke();
        }

        [Client]
        public void DeselectUnit()
        {
            if (!hasAuthority) { return; }

            _onDeselected?.Invoke();
        }
        #endregion
    }
}


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
        [SerializeField] private int _cost = 10;
        [SerializeField] private Health _health = null;
        [SerializeField] private UnitMove _unitMove = null;
        [SerializeField] private Targeting _target = null;
        [SerializeField] private UnityEvent _onSelected = null;
        [SerializeField] private UnityEvent _onDeselected = null;
        public static event Action<Unit> OnSpawnedUnit_Server;
        public static event Action<Unit> OnDespawnedUnit_Server;
        public static event Action<Unit> OnSpawnedUnit_Client;
        public static event Action<Unit> OnDespawnedUnit_Client;
        
        public int Cost { get; }
        #region Server
        public override void OnStartServer()
        {
            _health.OnDie += OnUnitDestroyed;
            OnSpawnedUnit_Server?.Invoke(this);
        }

        public override void OnStopServer()
        {
            _health.OnDie -= OnUnitDestroyed;
            OnDespawnedUnit_Server?.Invoke(this);
        }

        [Server]
        public void OnUnitDestroyed()
        {
            NetworkServer.Destroy(gameObject);
        }
        #endregion

        #region Client
        public override void OnStartAuthority()
        {
            OnSpawnedUnit_Client?.Invoke(this);
        }

        public override void OnStopClient()
        {
            if (!hasAuthority) return;

            OnDespawnedUnit_Client?.Invoke(this);
        }

        public UnitMove GetUnitMove()
        {
            return _unitMove;
        }
        public Targeting GetTarget()
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


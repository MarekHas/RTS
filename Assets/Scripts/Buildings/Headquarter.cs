using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

namespace MH.Games.RTS
{
    public class Headquarter : NetworkBehaviour
    {
        [SerializeField] private Health _health;
        public static Action<int> ServerOnPlayerDestroyed;
        public static Action<Headquarter> OnHeadquarterSpawned;//Server
        public static Action<Headquarter> OnHeadquarterDespawned;//Server
        #region Server
        public override void OnStartServer()
        {
            _health.OnDie += DestroyedHeadquarter;
            OnHeadquarterSpawned?.Invoke(this);
        }

        public override void OnStopServer()
        {
            _health.OnDie -= DestroyedHeadquarter;
            OnHeadquarterDespawned?.Invoke(this);
        }

        [Server]
        private void DestroyedHeadquarter()
        {
            ServerOnPlayerDestroyed?.Invoke(connectionToClient.connectionId);
            NetworkServer.Destroy(gameObject);
        }
        #endregion

        #region Client

        #endregion
    }
}


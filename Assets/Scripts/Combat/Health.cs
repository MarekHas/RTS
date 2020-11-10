using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

namespace MH.Games.RTS
{
    public class Health : NetworkBehaviour
    {
        [SerializeField] private int _maxHealth = 100;

        [SyncVar]
        private int _actualHealthValue;

        public event Action OnDie;//server
        public event Action<int, int> OnHealthChanged;//client
        
        #region Server
        public override void OnStartServer()
        {
            _actualHealthValue = _maxHealth;
            Headquarter.ServerOnPlayerDestroyed += PlayerDestroyedHandle;
        }

        public override void OnStopServer()
        {
            Headquarter.ServerOnPlayerDestroyed -= PlayerDestroyedHandle;
        }

        private void PlayerDestroyedHandle(int playerID)
        {
            if(playerID != connectionToClient.connectionId) { return; }
            TakeDamage(_actualHealthValue);//Kill player
        }

        [Server]
        public void TakeDamage(int damagePoint)
        {
            if (_actualHealthValue == 0) { return; }

            _actualHealthValue = Mathf.Max(_actualHealthValue - damagePoint, 0);

            if (_actualHealthValue != 0) { return; }

            OnDie?.Invoke();

            Debug.Log("Object Destroyed !!!");
        }

        #endregion

        #region Client

        private void ChangedHealth(int previousHealth, int updatedHealth)
        {
            OnHealthChanged?.Invoke(updatedHealth, _maxHealth);
        }

        #endregion


    }
}


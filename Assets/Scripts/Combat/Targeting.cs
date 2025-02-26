﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MH.Games.RTS
{
    public class Targeting : NetworkBehaviour
    {
        private Attackable _attackedTarget;

        public override void OnStartServer()
        {
            GameOverManager.ServerOnGameOver += GameOverHandler;
        }

        public override void OnStopServer()
        {
            GameOverManager.ServerOnGameOver -= GameOverHandler;
        }
      
        public Attackable GetAttackedTarget()
        {
            return _attackedTarget;
        }
        #region Server

        [Command]
        public void CmdSetTarget(GameObject targetGameObject)
        {
            if (!targetGameObject.TryGetComponent<Attackable>(out Attackable newTarget)) { return; }

            _attackedTarget = newTarget;
            Debug.Log("Attack!!!" + _attackedTarget.gameObject.name);
        }

        [Server]
        public void ClearTarget()
        {
            _attackedTarget = null;
        }

        private void GameOverHandler()
        {
            ClearTarget();
        }
        #endregion

        #region Client

        #endregion

    }
}


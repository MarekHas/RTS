﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEditor;
namespace MH.Games.RTS
{
    public class Target : NetworkBehaviour
    {
        private Attack _attackedTarget;

        public Attack GetAttackedTarget()
        {
            return _attackedTarget;
        }
        #region Server

        [Command]
        public void CmdSetTarget(GameObject targetGameObject)
        {
            if (!targetGameObject.TryGetComponent<Attack>(out Attack newTarget)) { return; }

            _attackedTarget = newTarget;
        }

        [Server]
        public void ClearTarget()
        {
            _attackedTarget = null;
        }

        #endregion

        #region Client

        #endregion

    }
}


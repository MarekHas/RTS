using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MH.Games.RTS
{
    //Targeter
    public class Targeting : NetworkBehaviour
    {
        private Attackable _attackedTarget;

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

        #endregion

        #region Client

        #endregion

    }
}


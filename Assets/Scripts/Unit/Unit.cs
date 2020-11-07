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
        [SerializeField] private UnityEvent _onSelected = null;
        [SerializeField] private UnityEvent _onDeselected = null;

        #region Client
        public UnitMove GetUnitMove()
        {
            return _unitMove;
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


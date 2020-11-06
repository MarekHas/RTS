using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildUnit : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _unit = null;
    [SerializeField] private Transform _buildPoint = null;

    #region Server
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MH.Games.RTS
{
    public class PlayerColor : NetworkBehaviour
    {
        [SerializeField] private Renderer[] colorRenderers = new Renderer[0];

        [SyncVar(hook = nameof(PlayerColorUpdate))]
        private Color teamColor = new Color();

        #region Server

        public override void OnStartServer()
        {
            var player = connectionToClient.identity.GetComponent<PlayerManager>();

            teamColor = player.PlayerColor;
        }
        #endregion

        #region Client

        private void PlayerColorUpdate(Color oldColor, Color newColor)
        {
            foreach (Renderer renderer in colorRenderers)
            {
                renderer.material.SetColor("_BaseColor", newColor);
            }
        }
        #endregion
    }
}
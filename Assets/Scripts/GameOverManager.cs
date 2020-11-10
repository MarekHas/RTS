using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

namespace MH.Games.RTS
{
    public class GameOverManager : NetworkBehaviour
    {
        public static event Action ServerOnGameOver;
        public static event Action<string> ClientOnFinishedGame;
        private List<Headquarter> _headquarters = new List<Headquarter>();
        
        #region Server
        public override void OnStartServer()
        {
            Headquarter.OnHeadquarterSpawned += SpawnedHeadquarter;
            Headquarter.OnHeadquarterDespawned += DespawnedHeadquarter;
        }

        public override void OnStopServer()
        {
            Headquarter.OnHeadquarterSpawned -= SpawnedHeadquarter;
            Headquarter.OnHeadquarterDespawned -= DespawnedHeadquarter;
        }

        [Server]
        private void SpawnedHeadquarter(Headquarter headquarter)
        {
            _headquarters.Add(headquarter);
        }

        [Server]
        private void DespawnedHeadquarter(Headquarter headquarter)
        {
            _headquarters.Remove(headquarter);

            if(_headquarters.Count != 1) { return; }
            //else
            int winnerPlayerID = _headquarters[0].connectionToClient.connectionId;
            Debug.Log("Game Finished only one player left !!!");

            CilentRpcGameOver($"Winner : Player {winnerPlayerID}");
            ServerOnGameOver?.Invoke();
        }
        #endregion

        #region Client
        [ClientRpc]
        private void CilentRpcGameOver(string winnerName)
        {
            ClientOnFinishedGame?.Invoke(winnerName);
        }
        #endregion
    }
}
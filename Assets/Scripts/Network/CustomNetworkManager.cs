using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

namespace MH.Games.RTS
{
    public class CustomNetworkManager : NetworkManager
    {
        [SerializeField] private GameObject _headquarter = null;
        [SerializeField] private GameOverManager _gameOverManagerPrefab = null;
        public static event Action ClientOnConnected;
        public static event Action ClientOnDisconnected;

        public override void OnClientConnect(NetworkConnection connection)
        {
            base.OnClientConnect(connection);

            ClientOnConnected?.Invoke();
        }

        public override void OnClientDisconnect(NetworkConnection connection)
        {
            base.OnClientDisconnect(connection);

            ClientOnDisconnected?.Invoke();
        }

        public override void OnServerAddPlayer(NetworkConnection conectionToTheClient)
        {
            base.OnServerAddPlayer(conectionToTheClient);

            PlayerManager playerManager = conectionToTheClient.identity.GetComponent<PlayerManager>();
            playerManager.SetPlayerColor(
                new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f))
                );

            GameObject unitBuilderInstance = 
                Instantiate(_headquarter, 
                conectionToTheClient.identity.transform.position, 
                conectionToTheClient.identity.transform.rotation);

            NetworkServer.Spawn(unitBuilderInstance, conectionToTheClient);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if (SceneManager.GetActiveScene().name.StartsWith("Level_"))
            {
                var gameOverManager = Instantiate(_gameOverManagerPrefab);
                NetworkServer.Spawn(gameOverManager.gameObject);
            }
        }
    }
}
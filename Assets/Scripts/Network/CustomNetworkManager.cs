using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

namespace MH.Games.RTS
{
    public class CustomNetworkManager : NetworkManager
    {
        [SerializeField] private GameObject _headquarter = null;
        [SerializeField] private GameOverManager _gameOverManagerPrefab = null;
        
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
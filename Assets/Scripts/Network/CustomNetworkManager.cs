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
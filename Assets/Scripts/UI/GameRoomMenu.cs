using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MH.Games.RTS
{
    public class GameRoomMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _roomUI = null;

        private void Start()
        {
            CustomNetworkManager.ClientOnConnected += HandleClientConnected;
        }

        private void OnDestroy()
        {
            CustomNetworkManager.ClientOnConnected -= HandleClientConnected;

        }

        private void HandleClientConnected()
        {
            _roomUI.SetActive(true);
        }

        public void LeaveRoom()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopHost();
            }
            else
            {
                NetworkManager.singleton.StopClient();

                SceneManager.LoadScene(0);
            }
        }
    }
}
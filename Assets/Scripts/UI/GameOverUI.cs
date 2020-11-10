using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

namespace MH.Games.RTS
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private TMP_Text _winnerName;

        private void Start()
        {
            GameOverManager.ClientOnFinishedGame += GameOver;
        }

        private void OnDestroy()
        {
            GameOverManager.ClientOnFinishedGame -= GameOver;
        }

        public void QuitGame()
        {
            if(NetworkServer.active && NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopHost();
            }
            else
            {
                NetworkManager.singleton.StopClient();
            }
        }

        private void GameOver(string winnerName)
        {
            _winnerName.text = $"{winnerName} is Winner";
            _gameOverPanel.SetActive(true);
        }
    }
}
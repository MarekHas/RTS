using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


namespace MH.Games.RTS
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _startGamePanel = null;

        public void HostGame()
        {
            _startGamePanel.SetActive(false);

            NetworkManager.singleton.StartHost();
        }

    }
}
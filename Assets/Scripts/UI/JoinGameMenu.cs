using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MH.Games.RTS
{
    public class JoinGameMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _startGamePanel = null;
        [SerializeField] private TMP_InputField addressInput = null;
        [SerializeField] private Button joinButton = null;

        private void OnEnable()
        {
            CustomNetworkManager.ClientOnConnected += HandleClientConnected;
            CustomNetworkManager.ClientOnDisconnected += HandleClientDisconnected;
        }

        private void OnDisable()
        {
            CustomNetworkManager.ClientOnConnected -= HandleClientConnected;
            CustomNetworkManager.ClientOnDisconnected -= HandleClientDisconnected;
        }

        public void Join()
        {
            string address = addressInput.text;

            CustomNetworkManager.singleton.networkAddress = address;
            CustomNetworkManager.singleton.StartClient();

            joinButton.interactable = false;
        }

        private void HandleClientConnected()
        {
            joinButton.interactable = true;

            gameObject.SetActive(false);
            _startGamePanel.SetActive(false);
        }

        private void HandleClientDisconnected()
        {
            joinButton.interactable = true;
        }

    }
}
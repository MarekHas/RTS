using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

namespace MH.Games.RTS
{
    public class ResourcesUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resourcesValueText = null;
        private PlayerManager _playerManager;

        private void Update()
        {
            if (_playerManager == null)
            {
                _playerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();

                if (_playerManager != null)
                {
                    ResoucesChangedHandler(_playerManager.Resources);

                    _playerManager.OnResourcesChanged += ResoucesChangedHandler;
                }
            }
        }

        private void OnDestroy()
        {
            _playerManager.OnResourcesChanged -= ResoucesChangedHandler;
        }

        private void ResoucesChangedHandler(int actualValue)
        {
            _resourcesValueText.text = $"Resources : {actualValue}";
        }
    }
}
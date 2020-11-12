using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MH.Games.RTS
{
    public class ResourceProduction : NetworkBehaviour
    {
        [SerializeField] private Health _health = null;
        [SerializeField] private int _productionValue = 10;
        [SerializeField] private float _productionTime = 2f;

        private float _timer;
        private PlayerManager _player;

        public override void OnStartServer()
        {
            _timer = _productionTime;
            _player = connectionToClient.identity.GetComponent<PlayerManager>();


            _health.OnDie += ServerHandleDie;
            GameOverManager.ServerOnGameOver += ServerHandleGameOver;
        }

        public override void OnStopServer()
        {
            _health.OnDie -= ServerHandleDie;
            GameOverManager.ServerOnGameOver -= ServerHandleGameOver;
        }

        [ServerCallback]
        private void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _player.SetResources(_player.GetResources() + _productionValue);
                Debug.Log( "R Value : " + _player.GetResources());
                _timer += _productionTime;
            }
        }

        private void ServerHandleDie()
        {
            NetworkServer.Destroy(gameObject);
        }

        private void ServerHandleGameOver()
        {
            enabled = false;
        }

    }
}
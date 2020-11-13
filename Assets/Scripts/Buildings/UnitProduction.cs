using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;

namespace MH.Games.RTS
{
    public class UnitProduction : NetworkBehaviour, IPointerClickHandler
    {
        [SerializeField] private Health _health; 
        [SerializeField] private Unit _unit = null;
        [SerializeField] private Transform _buildPoint = null;
        [SerializeField] private TMP_Text _unitLeftToBuild = null;
        [SerializeField] private Image _buildTimerImage = null;
        [SerializeField] private int _maxUnitInQueue=5;
        [SerializeField] private float _moveRangeAfterSpawned=5;
        [SerializeField] private float _buildTime = 5f;

        [SyncVar(hook = nameof(UnitsInQueueUpdate))]
        private int _unitInQueue;
        [SyncVar]
        private float _unitBuildTimeLeft;
        private float _progressImageVelocity;

        private void Update()
        {
            if (isServer) { CreateUnit(); };
            if (isClient) { ProductionTimerUpdate(); };
        }

        #region Server

        public override void OnStartServer()
        {
            _health.OnDie += OnBuildingDestroyed;
        }

        public override void OnStopServer()
        {
            _health.OnDie -= OnBuildingDestroyed;
        }

        [Server]
        private void CreateUnit()
        {
            if(_unitInQueue == 0) { return; }

            _unitBuildTimeLeft += Time.deltaTime;

            if (_unitBuildTimeLeft < _buildTime) { return; }

            var unitInstance = Instantiate(
                _unit,
                _buildPoint.position,
                _buildPoint.rotation);

            NetworkServer.Spawn(unitInstance.gameObject, connectionToClient);

            Vector3 spawnOffset = UnityEngine.Random.insideUnitSphere * _moveRangeAfterSpawned;
            spawnOffset.y = _buildPoint.position.y;

            UnitMove unitMove = unitInstance.GetComponent<UnitMove>();
            unitMove.ServerMove(_buildPoint.position + spawnOffset);

            _unitInQueue--;
            _unitBuildTimeLeft = 0;
        }

        [Server]
        private void OnBuildingDestroyed()
        {
            NetworkServer.Destroy(gameObject);
        }

        [Command]
        private void CommandSpawnUnit()
        {
            if(_unitInQueue == _maxUnitInQueue) { return; }

            var playerManager = connectionToClient.identity.GetComponent<PlayerManager>();

            if(playerManager.GetResources() < _unit.Cost) { return; }

            _unitInQueue++;
            playerManager.SetResources(playerManager.GetResources() - _unit.Cost);
        }
        #endregion

        #region Client
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) { return; }

            if (!hasAuthority) { return; }

            CommandSpawnUnit();
        }

        private void UnitsInQueueUpdate(int previousValue, int actualValue)
        {
            _unitLeftToBuild.text = actualValue.ToString();
        }

        private void ProductionTimerUpdate()
        {
            float buildProgress = _unitBuildTimeLeft / _buildTime;

            if(buildProgress < _buildTimerImage.fillAmount)
            {
                _buildTimerImage.fillAmount = buildProgress;
            }
            else
            {
                _buildTimerImage.fillAmount = Mathf.SmoothDamp
                    (
                    _buildTimerImage.fillAmount, buildProgress,
                    ref _progressImageVelocity,
                    0.1f
                    );
            }
        }
        #endregion
    }
}


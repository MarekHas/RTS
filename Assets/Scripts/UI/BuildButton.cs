using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MH.Games.RTS
{
    public class BuildButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Building _building = null;
        [SerializeField] private Image _buildingSymbol = null;
        [SerializeField] private TMP_Text _constructionCost = null;
        [SerializeField] private LayerMask _floorLayer = new LayerMask();

        private Camera _mainCamera;
        private PlayerManager _playerManager;
        private GameObject _previewBuilding;
        private Renderer _buildingRenderer;
        private BoxCollider _buidligCollider;

        private void Start()
        {
            _mainCamera = Camera.main;

            _buildingSymbol.sprite = _building.Icon;
            _constructionCost.text = _building.Cost.ToString();

            _buidligCollider = _building.GetComponent<BoxCollider>();
        }

        private void Update()
        {
            if (_playerManager == null)
            {
                _playerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
            }

            if (_previewBuilding == null) { return; }

            UpdatePreviewBuilding();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) { return; }

            if(_playerManager.GetResources() < _building.Cost) { return; }

            _previewBuilding = Instantiate(_building.Preview);
            _buildingRenderer = _previewBuilding.GetComponentInChildren<Renderer>();

            _previewBuilding.SetActive(false);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_previewBuilding == null) { return; }

            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _floorLayer))
            {
                _playerManager.TryPutBuilding(_building.Id, hit.point);
            }

            Destroy(_previewBuilding);
        }

        private void UpdatePreviewBuilding()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _floorLayer)) { return; }

            _previewBuilding.transform.position = hit.point;

            if (!_previewBuilding.activeSelf)
            {
                _previewBuilding.SetActive(true);
            }
            Color color = _playerManager.IsPlacingBuildingPossible(_buidligCollider,hit.point) ? Color.green : Color.red;

            _buildingRenderer.material.SetColor("_BaseColor", color);
        }

    }
}

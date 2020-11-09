using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

namespace MH.Games.RTS
{
    public class UnitSelection : MonoBehaviour
    {
        [SerializeField] private RectTransform _unitSelection = null;
        [SerializeField] private LayerMask _layerMask = new LayerMask();
        private Vector2 _drawStartingPoint;
        private PlayerManager _playerManager;
        private Camera _mainCamera;
        public List<Unit> SelectedUnits { get; set; } = new List<Unit>();

        private void Start()
        {
            _mainCamera = Camera.main;
            Unit.OnDespawnedUnit_Client += RemoveUnit;
        }
        
        private void OnDestroy()
        {
            Unit.OnDespawnedUnit_Client -= RemoveUnit;
        }

        private void RemoveUnit(Unit unit)
        {
            SelectedUnits.Remove(unit);
        }

        private void Update()
        {
            if(_playerManager == null)
            {
                _playerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartDrawSelectionArea();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                ClearUnitsSelectionArea();
            }
            else if (Mouse.current.leftButton.isPressed)
            {
                DrawingSelectionArea();
            }
        }

        private void StartDrawSelectionArea()
        {
            if (!Keyboard.current.leftShiftKey.isPressed)
            {
                foreach (Unit unit in SelectedUnits)
                {
                    unit.DeselectUnit();
                }

                SelectedUnits.Clear();
            }

            _unitSelection.gameObject.SetActive(true);
            _drawStartingPoint = Mouse.current.position.ReadValue();
            DrawingSelectionArea();
        }

        private void DrawingSelectionArea()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            float areaWidth = mousePos.x - _drawStartingPoint.x;
            float areaHeight = mousePos.y - _drawStartingPoint.y;

            _unitSelection.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
            _unitSelection.anchoredPosition = _drawStartingPoint + 
                new Vector2(areaWidth / 2, areaHeight / 2);
        }

        private void ClearUnitsSelectionArea()
        {
            _unitSelection.gameObject.SetActive(false);

            if(_unitSelection.sizeDelta.magnitude == 0)
            {
                SingleSelection();
                return;
            }

            MultiSelection();
        }

        private void SingleSelection()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask)) { return; }

            if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) { return; }

            if (!unit.hasAuthority) { return; }

            SelectedUnits.Add(unit);

            foreach (Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.SelectUnit();
            }
        }

        private void MultiSelection()
        {
            Vector2 min = _unitSelection.anchoredPosition -(_unitSelection.sizeDelta / 2);
            Vector2 max = _unitSelection.anchoredPosition + (_unitSelection.sizeDelta / 2);

            foreach (var unit in _playerManager.PlayerUnits)
            {
                if (SelectedUnits.Contains(unit)) continue;

                Vector3 screenPos = _mainCamera.WorldToScreenPoint(unit.transform.position);
                if(screenPos.x > min.x && screenPos.x < max.x && 
                    screenPos.y > min.y && screenPos.y < max.y)
                {
                    SelectedUnits.Add(unit);
                    unit.SelectUnit();
                } 
            }
        }

    }
}


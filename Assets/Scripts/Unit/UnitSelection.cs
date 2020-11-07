using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MH.Games.RTS
{
    public class UnitSelection : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask = new LayerMask();
        private Camera _mainCamera;

        public List<Unit> SelectedUnits { get; set; } = new List<Unit>();

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                foreach (Unit unit in SelectedUnits)
                {
                    unit.DeselectUnit();
                }

                SelectedUnits.Clear();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                ClearUnitsSelectionArea();
            }
        }

        private void ClearUnitsSelectionArea()
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

    }
}


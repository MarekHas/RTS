using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class AreaSelection : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask = new LayerMask();
    private Camera _mainCamera;
    private List<Unit> _selectedUnits = new List<Unit>();

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            foreach (Unit unit in _selectedUnits)
            {
                unit.DeselectUnit();
            }

            _selectedUnits.Clear();
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

        _selectedUnits.Add(unit);

        foreach (Unit selectedUnit in _selectedUnits)
        {
            selectedUnit.SelectUnit();
        }
    }

}

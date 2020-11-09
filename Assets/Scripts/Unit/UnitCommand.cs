using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MH.Games.RTS
{
    public class UnitCommand : MonoBehaviour
    {
        [SerializeField] private UnitSelection unitSelectionHandler = null;
        [SerializeField] private LayerMask layerMask = new LayerMask();

        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }

            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }

            if(hit.collider.TryGetComponent<Attack>(out Attack attack))
            {
                if (attack.hasAuthority)
                {
                    MoveUnit(hit.point);
                    return;
                }
                TryAttack(hit.point);
            }
            MoveUnit(hit.point);
        }

        private void MoveUnit(Vector3 point)
        {
            foreach (Unit unit in unitSelectionHandler.SelectedUnits)
            {
                unit.GetUnitMove().CommandMove(point);
            }
        }

        private void TryAttack(Vector3 point)
        {
            foreach (Unit unit in unitSelectionHandler.SelectedUnits)
            {
                unit.GetUnitMove().CommandMove(point);
            }
        }
    }
}
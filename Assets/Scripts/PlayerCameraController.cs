using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MH.Games.RTS
{
    public class PlayerCameraController : NetworkBehaviour
    {
        [SerializeField] private Transform _cameraTransform = null;
        [SerializeField] private float _cameraSpeed = 20f;
        [SerializeField] private float _borderThickness = 10f;
        [SerializeField] private Vector2 _xLimits = Vector2.zero;
        [SerializeField] private Vector2 _zLimits = Vector2.zero;

        private Vector2 _previousInput;
        private GameControls _controls;

        public override void OnStartAuthority()
        {
            _cameraTransform.gameObject.SetActive(true);

            _controls = new GameControls();

            _controls.Player.CameraMove.performed += SetPreviousInput;
            _controls.Player.CameraMove.canceled += SetPreviousInput;

            _controls.Enable();
        }

        [ClientCallback]
        private void Update()
        {
            if (!hasAuthority || !Application.isFocused) { return; }

            CameraPositionUpdate();
        }

        private void CameraPositionUpdate()
        {
            Vector3 pos = _cameraTransform.position;

            if (_previousInput == Vector2.zero)
            {
                Vector3 cursorMovement = Vector3.zero;

                Vector2 cursorPosition = Mouse.current.position.ReadValue();

                if (cursorPosition.y >= Screen.height - _borderThickness)
                {
                    cursorMovement.z += 1;
                }
                else if (cursorPosition.y <= _borderThickness)
                {
                    cursorMovement.z -= 1;
                }
                if (cursorPosition.x >= Screen.width - _borderThickness)
                {
                    cursorMovement.x += 1;
                }
                else if (cursorPosition.x <= _borderThickness)
                {
                    cursorMovement.x -= 1;
                }

                pos += cursorMovement.normalized * _cameraSpeed * Time.deltaTime;
            }
            else
            {
                pos += new Vector3(_previousInput.x, 0f, _previousInput.y) * _cameraSpeed * Time.deltaTime;
            }

            pos.x = Mathf.Clamp(pos.x, _xLimits.x, _xLimits.y);
            pos.z = Mathf.Clamp(pos.z, _zLimits.x, _zLimits.y);

            _cameraTransform.position = pos;
        }

        private void SetPreviousInput(InputAction.CallbackContext ctx)
        {
            _previousInput = ctx.ReadValue<Vector2>();
        }
    }
}
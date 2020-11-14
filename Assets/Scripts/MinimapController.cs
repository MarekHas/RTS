using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace MH.Games.RTS
{
    public class MinimapController : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        [SerializeField] private RectTransform _minimapRect = null;
        [SerializeField] private float _mapScale = 20f;
        [SerializeField] private float _offset = -6f;

        private Transform playerCameraTransform;

        private void Update()
        {
            if (playerCameraTransform != null) { return; }

            if (NetworkClient.connection.identity == null) { return; }

            playerCameraTransform = NetworkClient.connection.identity
                .GetComponent<PlayerManager>().GetCameraTransform();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            MoveCamera();
        }

        public void OnDrag(PointerEventData eventData)
        {
            MoveCamera();
        }

        private void MoveCamera()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _minimapRect,
                mousePosition,
                null,
                out Vector2 localPoint
            )) { return; }

            Vector2 lerp = new Vector2(
                (localPoint.x - _minimapRect.rect.x) / _minimapRect.rect.width,
                (localPoint.y - _minimapRect.rect.y) / _minimapRect.rect.height);

            Vector3 newCameraPos = new Vector3(
                Mathf.Lerp(-_mapScale, _mapScale, lerp.x),
                playerCameraTransform.position.y,
                Mathf.Lerp(-_mapScale, _mapScale, lerp.y));

            playerCameraTransform.position = newCameraPos + new Vector3(0f, 0f, _offset);
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MH.Games.RTS
{
    public class Bullet : NetworkBehaviour
    {
        [SerializeField] private float _force=10f;
        [SerializeField] private float _bulletLifeTime = 5f;
        [SerializeField] private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody.velocity = transform.forward * _force;
        }

        public override void OnStartServer()
        {
            Invoke(nameof(DestroyBullet), _bulletLifeTime);
        }

        [Server]
        private void DestroyBullet()
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}
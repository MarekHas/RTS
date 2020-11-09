using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MH.Games.RTS
{
    public class Bullet : NetworkBehaviour
    {
        [SerializeField] private int _damgePoints = 20;
        [SerializeField] private float _force = 10f;
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

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out NetworkIdentity networkIdentity))
            {
                if(networkIdentity.connectionToClient == connectionToClient) { return; }
            }   
            
            if(other.TryGetComponent(out Health health))
            {
                health.TakeDamage(_damgePoints);
            }

            DestroyBullet();
        }
        [Server]
        private void DestroyBullet()
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}
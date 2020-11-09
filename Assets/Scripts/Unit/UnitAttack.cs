using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MH.Games.RTS
{
    public class UnitAttack : NetworkBehaviour
    {
        [SerializeField] private GameObject _projectilePrefab = null;
        [SerializeField] private Transform _weaponTip = null;
        [SerializeField] private float _fireRange = 5f;
        [SerializeField] private float _fireRate = 1f;
        [SerializeField] private float _rotationSpeed = 20f;

        [SerializeField]private Targeting _target = null;
        private float _lastFireTime;

        public float FireRange { get => _fireRange; private set => _fireRange = value; }

        [ServerCallback]
        private void Update()
        {
            var aimedTarget = _target.GetAttackedTarget();

            if (aimedTarget == null) { return; }

            if (!CanFireAtTarget()) { return; }

            Quaternion targetRotation =
                Quaternion.LookRotation(aimedTarget.transform.position - transform.position);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            if (Time.time > (1 / _fireRate) + _lastFireTime)
            {
                Quaternion projectileRotation = Quaternion.LookRotation(
                    aimedTarget.GetAimPoint().position - _weaponTip.position);

                GameObject projectileInstance = Instantiate(
                    _projectilePrefab, _weaponTip.position, projectileRotation);

                NetworkServer.Spawn(projectileInstance, connectionToClient);

                _lastFireTime = Time.time;
            }
        }

        [Server]
        private bool CanFireAtTarget()
        {
            return (_target.GetAttackedTarget().transform.position - transform.position).sqrMagnitude
                <= FireRange * FireRange;
        }
    }
}
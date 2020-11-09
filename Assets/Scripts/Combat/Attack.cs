using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MH.Games.RTS
{
    public class Attack : NetworkBehaviour
    {
        [SerializeField] private Transform _weaponTip = null;

        public Transform GetWeaponTip()
        {
            return _weaponTip;
        }
    }
}


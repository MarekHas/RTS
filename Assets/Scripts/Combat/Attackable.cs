using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MH.Games.RTS
{
    //Targatable 
    public class Attackable : NetworkBehaviour
    {
        [SerializeField] private Transform _aimPoint = null;

        public Transform GetAimPoint()
        {
            return _aimPoint;
        }
    }
}


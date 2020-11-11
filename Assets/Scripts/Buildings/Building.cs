using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

namespace MH.Games.RTS
{
    public class Building : NetworkBehaviour
    {
        [SerializeField] private GameObject _preview = null;
        [SerializeField] private Sprite _icon = null;
        [SerializeField] private int _id = -1;
        [SerializeField] private int _cost = 100;
  

        public static event Action<Building> OnConstructionServer;
        public static event Action<Building> OnDemolitionServer;

        public static event Action<Building> OnConstructionAuthority;
        public static event Action<Building> OnDemolitionAuthority;

        public GameObject Preview{ get => _preview;}
        public Sprite Icon{ get => _icon;}
        public int Id{ get => _id;}
        public int Cost{ get => _cost;}

        #region Server

        public override void OnStartServer()
        {
            OnConstructionServer?.Invoke(this);
        }

        public override void OnStopServer()
        {
            OnDemolitionServer?.Invoke(this);
        }

        #endregion

        #region Client

        public override void OnStartAuthority()
        {
            OnConstructionAuthority?.Invoke(this);
        }

        public override void OnStopClient()
        {
            if (!hasAuthority) { return; }

            OnDemolitionAuthority?.Invoke(this);
        }

        #endregion

    }
}
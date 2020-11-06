using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MH.Games.RTS
{
    public class CustomNetworkManager : NetworkManager
    {
        [SerializeField] private GameObject _unitBuilder = null;

        public override void OnServerAddPlayer(NetworkConnection conectionToTheClient)
        {
            base.OnServerAddPlayer(conectionToTheClient);
            GameObject unitBuilderInstance = 
                Instantiate(_unitBuilder, conectionToTheClient.identity.transform.position, conectionToTheClient.identity.transform.rotation);
            NetworkServer.Spawn(unitBuilderInstance, conectionToTheClient);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

namespace MH.Games.RTS
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private Transform cameraTransform = null;
        [SerializeField] private LayerMask _blockingLayers = new LayerMask();
        [SerializeField] private Building[] _allBuildingsInGame = new Building[0];
        [SerializeField] private float _range = 5f;
        public List<Unit> PlayerUnits { get; private set; } = new List<Unit>();
        public List<Building> Buildings { get; private set; } = new List<Building>();

        [SyncVar(hook = nameof(ResourcesValueUpdate))]
        private int _resources = 500;

        private Color _playerColor = new Color();

        public event Action<int> OnResourcesChanged;

        public int GetResources()
        {
            return _resources;
        }

        public Transform GetCameraTransform()
        {
            return cameraTransform;
        }

        public Color PlayerColor { get; }
        public bool IsPlacingBuildingPossible(BoxCollider boxCollider,Vector3 point)
        {
            return true;
            //if (Physics.CheckBox(
            //    point + boxCollider.center,
            //    boxCollider.size / 2,
            //    Quaternion.identity,
            //    _blockingLayers))
            //{
            //    return false;
            //}

            //foreach (var building in Buildings)
            //{
            //    if ((point - building.transform.position).sqrMagnitude <= _range * _range)
            //    {
            //        return true;
            //    }
            //}

            //return false;
        }

        #region Server
        public override void OnStartServer()
        {
            Unit.OnSpawnedUnit_Server += SpawnedUnitServerHandler;
            Unit.OnDespawnedUnit_Server += DespawnedUnitServerHandler;
            Building.OnConstructionServer += ConstructionBuildigHandler;
            Building.OnDemolitionServer += DemolitionBuildingHandler;
        }

        public override void OnStopServer()
        {
            Unit.OnSpawnedUnit_Server -= SpawnedUnitServerHandler;
            Unit.OnDespawnedUnit_Server -= DespawnedUnitServerHandler;
            Building.OnConstructionServer -= ConstructionBuildigHandler;
            Building.OnDemolitionServer -= DemolitionBuildingHandler;
        }

        [Server]
        public void SetResources(int newResources)
        {
            _resources = newResources;
        }

        [Server]
        public void SetPlayerColor(Color color)
        {
            _playerColor = color;
        }

        [Command]
        public void TryPutBuilding(int buildingId, Vector3 point)
        {
            Building buildingToPlace = null;

            foreach (Building building in _allBuildingsInGame)
            {
                if (building.Id == buildingId)
                {
                    buildingToPlace = building;
                    break;
                }
            }

            if (buildingToPlace == null) { return; }

            if(_resources < buildingToPlace.Cost) { return; }

            BoxCollider buildingCollider = buildingToPlace.GetComponent<BoxCollider>();

            if (!IsPlacingBuildingPossible(buildingCollider,point)) { return; }

            GameObject buildingInstance =
                Instantiate(buildingToPlace.gameObject, point, buildingToPlace.transform.rotation);

            NetworkServer.Spawn(buildingInstance, connectionToClient);
            SetResources(_resources - buildingToPlace.Cost);
        }

        private void SpawnedUnitServerHandler(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }
       
            PlayerUnits.Add(unit);
        }

        private void DespawnedUnitServerHandler(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

            PlayerUnits.Remove(unit);
        }

        private void ConstructionBuildigHandler(Building building)
        {
            if (building.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

            Buildings.Remove(building);
        }

        private void DemolitionBuildingHandler(Building building)
        {
            if (building.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

            Buildings.Remove(building);
        }

        #endregion

        #region Client
        public override void OnStartAuthority()
        {
            if (NetworkServer.active) { return; }

            Unit.OnSpawnedUnit_Client += SpawnedUnitClientHandler;
            Unit.OnDespawnedUnit_Client += DespawnedUnitClientHandler;
            Building.OnConstructionAuthority += ConstructionBuildingAuthorityHandler;
            Building.OnDemolitionAuthority += DemolitionBuildingAuthorityHandler;
        }

        public override void OnStopClient()
        {
            if (!isClientOnly || !hasAuthority) return;

            Unit.OnSpawnedUnit_Client -= SpawnedUnitClientHandler;
            Unit.OnDespawnedUnit_Client -= DespawnedUnitClientHandler;
            Building.OnConstructionAuthority -= ConstructionBuildingAuthorityHandler;
            Building.OnDemolitionAuthority -= DemolitionBuildingAuthorityHandler;
        }

        private void ResourcesValueUpdate(int previousValue, int actualValue)
        {
            OnResourcesChanged?.Invoke(actualValue);
        }

        private void DemolitionBuildingAuthorityHandler(Building building)
        {
            Buildings.Add(building);
        }

        private void ConstructionBuildingAuthorityHandler(Building building)
        {
            Buildings.Remove(building);
        }

        private void SpawnedUnitClientHandler(Unit unit)
        {
            PlayerUnits.Add(unit);
        }

        private void DespawnedUnitClientHandler(Unit unit)
        {
            PlayerUnits.Remove(unit);
        }
        #endregion
    }
}
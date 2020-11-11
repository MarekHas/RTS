﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

namespace MH.Games.RTS
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private Building[] _allBuildingsInGame = new Building[0];
        public List<Unit> PlayerUnits { get; private set; } = new List<Unit>();
        public List<Building> Buildings { get; private set; } = new List<Building>();
        
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

            GameObject buildingInstance =
                Instantiate(buildingToPlace.gameObject, point, buildingToPlace.transform.rotation);

            NetworkServer.Spawn(buildingInstance, connectionToClient);
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
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelItemsManager : Singleton<LevelItemsManager>
{
    [SerializeField] private LevelItem _driverPrefab;
    private ObjectPool<LevelItem> _driverPool;

    [SerializeField] private LevelItem _vehiclePrefab;
    private ObjectPool<LevelItem> _vehiclePool;

    [SerializeField] private LevelItem _obstaclePrefab;
    private ObjectPool<LevelItem> _obstaclePool;

    private List<LevelItem> _vehiclesInLevelList = new List<LevelItem>();

    public bool AnItemMoving { get; private set; }

    private void OnEnable()
    {
        BusSystem.Level.OnLevelUnload += OnLevelUnload;
    }
    private void OnDisable()
    {
        BusSystem.Level.OnLevelUnload -= OnLevelUnload;
    }

    private void OnLevelUnload()
    {
        ResetAllItems();
    }

    public void Initialize()
    {
        _driverPool = new ObjectPool<LevelItem>(_driverPrefab, 5, transform);
        _vehiclePool = new ObjectPool<LevelItem>(_vehiclePrefab, 5, transform);
        _obstaclePool = new ObjectPool<LevelItem>(_obstaclePrefab, 5, transform);
    }

    public LevelItem GetFromPool(Enums.ItemTypes itemType)
    {
        switch (itemType)
        {
            case Enums.ItemTypes.Obstacle:
                return _obstaclePool.Get();

            case Enums.ItemTypes.Driver:
                return _driverPool.Get();

            case Enums.ItemTypes.Vehicle:
                LevelItem vehicle = _vehiclePool.Get();
                _vehiclesInLevelList.Add(vehicle);
                return vehicle;

            default:
                return null;
        }
    }

    public void PutIntoPool(LevelItem item)
    {
        switch (item.LevelItemData.ItemType)
        {
            case Enums.ItemTypes.Obstacle:
                _obstaclePool.Return(item);
                break;

            case Enums.ItemTypes.Driver:
                _driverPool.Return(item);
                break;

            case Enums.ItemTypes.Vehicle:
                _vehiclesInLevelList.Remove(item);
                _vehiclePool.Return(item);
                break;
        }

        item.transform.SetParent(transform);
    }

    public void CheckIfLevelCompleted()
    {
        bool allClear = true;
        foreach (Vehicle vehicle in _vehiclesInLevelList)
        {
            if (!vehicle.Gone)
            {
                allClear = false;
                break;
            }
        }

        if (allClear)
        {
            BusSystem.Level.CallLevelSuccess();
        }
    }

    private void ResetAllItems()
    {
        _obstaclePool.ReturnAll();
        _driverPool.ReturnAll();
        _vehiclePool.ReturnAll();

        _vehiclesInLevelList = new List<LevelItem>();
    }

    public void SetMovingState(bool state)
    {
        AnItemMoving = state;
    }
}

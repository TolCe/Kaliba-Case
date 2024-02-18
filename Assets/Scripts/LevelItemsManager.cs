using UnityEngine;

public class LevelItemsManager : Singleton<LevelItemsManager>
{
    [SerializeField] private LevelItem _driverPrefab;
    private ObjectPool<LevelItem> _driverPool;

    [SerializeField] private LevelItem _vehiclePrefab;
    private ObjectPool<LevelItem> _vehiclePool;

    [SerializeField] private LevelItem _obstaclePrefab;
    private ObjectPool<LevelItem> _obstaclePool;

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
                return _vehiclePool.Get();

            default:
                return null;
        }
    }

    public void PutIntoPool(LevelItem item)
    {
        switch (item.ItemType)
        {
            case Enums.ItemTypes.Obstacle:
                _obstaclePool.Return(item);
                break;

            case Enums.ItemTypes.Driver:
                _driverPool.Return(item);
                break;

            case Enums.ItemTypes.Vehicle:
                _vehiclePool.Return(item);
                break;
        }
    }
}

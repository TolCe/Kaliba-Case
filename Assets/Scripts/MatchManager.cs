using System.Collections.Generic;
using UnityEngine;

public class MatchManager : Singleton<MatchManager>
{
    private DynamicLevelItem _selectedDriver;
    private DynamicLevelItem _correctCar;

    public void SetDriver(DynamicLevelItem driver = null)
    {
        if (_selectedDriver != null)
        {
            _selectedDriver.RemoveHighlight();
        }

        _selectedDriver = driver;

        if (_selectedDriver != null)
        {
            _selectedDriver.HighlightItem();
        }
    }

    public void SetCar(DynamicLevelItem car)
    {
        if (_selectedDriver == null)
        {
            return;
        }

        if (car.Pair == _selectedDriver.Pair)
        {
            car.AttachedTile.SetAttachedItem();
            List<TileElement> tileElementList = GridManager.Instance.FindPath(car.AttachedTile);
            MoveDriver(tileElementList);
            _correctCar = car;
        }
        else
        {
            Debug.Log("Driver and car didn't match!");
        }

        SetDriver();
    }

    public async void MoveDriver(List<TileElement> targetTileList)
    {
        DynamicLevelItem cacheDriver = _selectedDriver;
        SetDriver();

        if (cacheDriver == null || targetTileList == null)
        {
            if (targetTileList == null)
            {
                Debug.Log("Path blocked!");
            }

            return;
        }

        await cacheDriver.MoveItem(targetTileList);

        if (_correctCar != null)
        {
            LevelItemsManager.Instance.PutIntoPool(cacheDriver);
            LevelItemsManager.Instance.PutIntoPool(_correctCar);
            _correctCar = null;
        }
    }
}

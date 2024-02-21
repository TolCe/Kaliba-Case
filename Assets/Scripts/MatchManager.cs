using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class MatchManager : Singleton<MatchManager>
{
    private Driver _selectedDriver;
    private Vehicle _selectedVehicle;

    public void SetDriver(Driver driver = null)
    {
        if (_selectedDriver != null)
        {
            _selectedDriver.DeselectDriver();
        }

        _selectedDriver = driver;

        if (LevelItemsManager.Instance.AnItemMoving)
        {
            return;
        }

        if (_selectedDriver != null)
        {
            _selectedDriver.SelectDriver();
        }
    }

    public void SetVehicle(Vehicle vehicle)
    {
        if (LevelItemsManager.Instance.AnItemMoving)
        {
            return;
        }

        if (_selectedDriver == null)
        {
            return;
        }

        if (vehicle.Pair == _selectedDriver.Pair)
        {
            List<List<TileElement>> pathList = new List<List<TileElement>>();
            foreach (TileElement tile in vehicle.DoorTileList)
            {
                List<TileElement> tileList = GridManager.Instance.FindPath(_selectedDriver.RegardedTileList[0], tile);
                if (tileList != null)
                {
                    pathList.Add(tileList);
                }
            }
            if (pathList.Count > 0)
            {
                _selectedVehicle = vehicle;
                List<TileElement> tileElementList = pathList.OrderByDescending(list => list.Count()).Last();
                MoveDriver(tileElementList, _selectedDriver, _selectedVehicle);
            }
            else
            {
                Debug.Log("Path blocked!");
            }
        }
        else
        {
            Debug.Log("Driver and car didn't match!");
        }

        ResetItems();
    }

    public void MoveToEmptyTile(TileElement emptyTile)
    {
        if (_selectedDriver == null)
        {
            return;
        }

        List<TileElement> tileElementList = GridManager.Instance.FindPath(_selectedDriver.RegardedTileList[0], emptyTile);
        MoveDriver(tileElementList, _selectedDriver);
    }

    public async void MoveDriver(List<TileElement> targetTileList, Driver driver, Vehicle vehicle = null)
    {
        LevelItemsManager.Instance.SetMovingState(true);
        ResetItems();

        await driver.Move(targetTileList);
        if (vehicle != null)
        {
            vehicle.PutDriverInCar(driver);
        }
        else
        {
            LevelItemsManager.Instance.SetMovingState(false);
        }
    }

    private void ResetItems()
    {
        SetDriver();
        _selectedVehicle = null;
    }
}

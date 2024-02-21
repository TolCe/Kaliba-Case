using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Vehicle : DynamicLevelItem
{
    public List<TileElement> DoorTileList { get; private set; }
    [SerializeField] private Transform _leftDoorTransform, _rightDoorTransform;

    public void SetDoorTiles(List<TileElement> tileList)
    {
        DoorTileList = new List<TileElement>(tileList);
    }

    public async Task OpenDoor(Vector3 pos)
    {
        Transform selectedDoor = _leftDoorTransform;
        if (Vector3.Distance(pos, _rightDoorTransform.position) < Vector3.Distance(pos, _leftDoorTransform.position))
        {
            selectedDoor = _rightDoorTransform;
        }
        await selectedDoor.DORotate(60f * Vector3.up, 0.3f).AsyncWaitForCompletion();
    }

    public async Task CloseDoor()
    {
        Transform selectedDoor = _leftDoorTransform;
        if (_rightDoorTransform.eulerAngles != Vector3.zero)
        {
            selectedDoor = _rightDoorTransform;
        }
        await selectedDoor.DORotate(Vector3.zero, 0.3f).AsyncWaitForCompletion();
    }

    public async void PutDriverInCar(Driver driver)
    {
        driver.EnterVehicle();
        await OpenDoor(driver.transform.position);

        driver.RemoveFromLevel();
        LevelItemsManager.Instance.PutIntoPool(driver);
        LevelItemsManager.Instance.SetMovingState(false);

        await Task.Delay(250);
        await CloseDoor();
        await WaitForClearPath();

        RemoveFromLevel();
        LevelItemsManager.Instance.CheckIfLevelCompleted();
        LevelItemsManager.Instance.PutIntoPool(this);
    }

    public async Task WaitForClearPath()
    {
        List<TileElement> outPathList = FindOutDirection();
        while (outPathList.Count <= 0)
        {
            await Task.Delay(50);
            outPathList = FindOutDirection();
        }
        await MoveItem(outPathList);
    }

    private List<TileElement> FindOutDirection()
    {
        List<List<TileElement>> exitPathList = new List<List<TileElement>>
        {
            CheckPath(-1),
            CheckPath(1),
        };

        for (int i = exitPathList.Count - 1; i >= 0; i--)
        {
            if (exitPathList[i] == null)
            {
                exitPathList.RemoveAt(i);
                continue;
            }
            if (exitPathList[i].Count == 0)
            {
                exitPathList.RemoveAt(i);
            }
        }
        if (exitPathList.Count > 0)
        {
            List<TileElement> tileElementList = exitPathList.OrderByDescending(list => list.Count()).Last();
            return tileElementList;
        }

        return new List<TileElement>();
    }

    private List<TileElement> CheckPath(int i)
    {
        List<TileElement> exitPathList = new List<TileElement>();
        TileElement tile = RegardedTileList[0];
        bool roadClear = true;
        int j = 1;
        Vector2 direction = new Vector2(Direction.x, Direction.z);
        TileElement exitTile = null;
        while (
            (tile.Pos + j * i * direction).x >= 0
            && (tile.Pos + j * i * direction).x < GridManager.Instance.TileWidth
            && (tile.Pos + j * i * direction).y >= 0
            && (tile.Pos + j * i * direction).y < GridManager.Instance.TileHeight
                )
        {
            if (!GridManager.Instance.GetTileAtPosition(tile.Pos + j * i * direction).IsWalkable(this))
            {
                roadClear = false;
                break;
            }
            exitTile = GridManager.Instance.GetTileAtPosition(tile.Pos + j * i * direction);
            j++;
        }

        if (roadClear)
        {
            exitPathList = GridManager.Instance.FindPath(tile, exitTile);
            if (exitPathList != null)
            {
                return exitPathList;
            }
        }

        return exitPathList;
    }
}

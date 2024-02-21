using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class LevelItem : MonoBehaviour
{
    public LevelItemDataSO LevelItemData { get; private set; }
    public List<TileElement> RegardedTileList { get; private set; }
    public Vector3 Direction { get; private set; }
    public Enums.Pairs Pair;

    public virtual void Initialize(TileElement tile)
    {
        LevelItemData = tile.TileVO.LevelItemData;
        Pair = tile.TileVO.Pair;

        GetAttachedTiles(tile);

        Vector3 position = (RegardedTileList[0].transform.position + RegardedTileList[RegardedTileList.Count - 1].transform.position) * 0.5f;
        transform.position = position;
        transform.eulerAngles = (90f * Direction.x + Direction.z * (90 * Direction.z - 90)) * Vector3.up;

        gameObject.SetActive(true);
    }

    public void SetDirection(Vector3 direction)
    {
        Direction = direction;
    }

    public void GetAttachedTiles(TileElement tile)
    {
        if (RegardedTileList != null)
        {
            foreach (var item in RegardedTileList)
            {
                item.SetAttachedItem();
            }
        }

        Vector3 vehicleDirection = tile.TileVO.FacedDirection;
        SetDirection(vehicleDirection);

        RegardedTileList = new List<TileElement>();
        for (int i = 0; i < LevelItemData.Size; i++)
        {
            TileElement newTile = GridManager.Instance.GetTileAtPosition(tile.Pos + i * (new Vector2(-Direction.x, Direction.z)));
            RegardedTileList.Add(newTile);
        }

        SetAttachedTiles();

        if (this is Vehicle)
        {
            Vehicle vehicle = this as Vehicle;
            List<TileElement> tileList = new List<TileElement>
            {
                GridManager.Instance.GetTileAtPosition(tile.Pos + (new Vector2(Direction.z, Direction.x))),
                GridManager.Instance.GetTileAtPosition(tile.Pos - (new Vector2(Direction.z, Direction.x)))
            };
            vehicle.SetDoorTiles(tileList);
        }
    }

    private void SetAttachedTiles()
    {
        transform.SetParent(RegardedTileList[0].transform, true);
        foreach (TileElement tile in RegardedTileList)
        {
            tile.SetAttachedItem(this);
        }
    }
}

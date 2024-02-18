using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TileElement : MonoBehaviour
{
    private LevelItem _attachedItem;

    private TileVO _tileVO;
    [SerializeField] private Renderer _rend;
    public Vector2 Pos { get; private set; }
    public float TileSize;

    private List<TileElement> _connectedTileViaItem = new List<TileElement>();

    public void Initialize(int row, int column, TileVO tileVO)
    {
        Pos = new Vector2(row, column);

        _tileVO = tileVO;
        LevelItem levelItem = LevelItemsManager.Instance.GetFromPool(_tileVO.ItemType);
        levelItem?.Initialize(this, _tileVO.Pair);
    }

    public void SetAttachedItem(LevelItem levelItem = null)
    {
        _attachedItem = levelItem;
        foreach (var tile in _connectedTileViaItem)
        {
            tile.SetAttachedItem(levelItem);
        }
    }

    public void CheckVehicleCreation()
    {
        if (_tileVO.ItemType == Enums.ItemTypes.Vehicle)
        {
            TileElement tile = null;
            for (int i = 1; i < _tileVO.VehicleSize; i++)
            {
                tile = GridManager.Instance.GetTileAtPosition(Pos + new Vector2(Mathf.Cos(Mathf.Deg2Rad * 90f * (3 - _tileVO.DirectionCoefficient)), Mathf.Sin(Mathf.Deg2Rad * 90f * (3 - _tileVO.DirectionCoefficient))));
                _connectedTileViaItem.Add(tile);
            }

            _attachedItem.transform.position = (transform.position + tile.transform.position) * 0.5f;
            SetAttachedItem(_attachedItem);
        }
    }

    private void OnMouseDown()
    {
        SelectTile();
    }

    public void SelectTile()
    {
        if (_attachedItem == null)
        {
            List<TileElement> tileElementList = GridManager.Instance.FindPath(this);
            MatchManager.Instance.MoveDriver(tileElementList);
        }
        else
        {
            if (_attachedItem is DynamicLevelItem)
            {
                DynamicLevelItem attachedItem = _attachedItem as DynamicLevelItem;
                attachedItem.SetClickActions();
                GridManager.Instance.SelectTile(this);
            }
        }
    }

    public void DeselectTile()
    {
        if (_attachedItem != null)
        {
            if (_attachedItem is DynamicLevelItem)
            {
                DynamicLevelItem attachedItem = _attachedItem as DynamicLevelItem;
                attachedItem.RemoveHighlight();
            }
        }
    }

    #region Pathfinding
    public TileElement Connection { get; private set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;

    public List<TileElement> Neighbours;
    private List<Vector2> _neighbourDirectionList = new List<Vector2>() { new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, 0) };

    public void SetNeighbourTiles()
    {
        Neighbours = new List<TileElement>();

        foreach (var tile in _neighbourDirectionList.Select(dir => GridManager.Instance.GetTileAtPosition(Pos + dir)).Where(tile => tile != null))
        {
            Neighbours.Add(tile);
        }
    }

    public void SetConnection(TileElement tileElement)
    {
        Connection = tileElement;
    }

    public void SetG(float g)
    {
        G = g;
    }

    public void SetH(float h)
    {
        H = h;
    }

    public float GetDistance(TileElement other)
    {
        var dist = new Vector2Int(Mathf.Abs((int)Pos.x - (int)other.Pos.x), Mathf.Abs((int)Pos.y - (int)other.Pos.y));

        var lowest = Mathf.Min(dist.x, dist.y);
        var highest = Mathf.Max(dist.x, dist.y);

        var horizontalMovesRequired = highest - lowest;

        return lowest * 14 + horizontalMovesRequired * 10;
    }

    public bool IsWalkable()
    {
        return _attachedItem == null;
    }
    #endregion
}

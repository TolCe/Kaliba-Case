using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileElement : MonoBehaviour
{
    public LevelItem AttachedItem { get; private set; }

    public TileVO TileVO { get; private set; }
    [SerializeField] private Renderer _rend;
    public Vector2 Pos { get; private set; }
    public float TileSize;

    public void Initialize(int row, int column, TileVO tileVO)
    {
        Pos = new Vector2(column, row);
        TileVO = tileVO;
        gameObject.SetActive(true);
    }

    public void InitializeTileAttachedItem()
    {
        if (TileVO.LevelItemData != null)
        {
            LevelItem levelItem = LevelItemsManager.Instance.GetFromPool(TileVO.LevelItemData.ItemType);
            levelItem?.Initialize(this);
        }
    }

    public void SetAttachedItem(LevelItem levelItem = null)
    {
        AttachedItem = levelItem;
    }

    private void OnMouseDown()
    {
        SelectTile();
    }

    public void SelectTile()
    {
        if (LevelItemsManager.Instance.AnItemMoving)
        {
            return;
        }

        if (AttachedItem == null)
        {
            MatchManager.Instance.MoveToEmptyTile(this);
        }
        else
        {
            if (AttachedItem is DynamicLevelItem)
            {
                DynamicLevelItem attachedItem = AttachedItem as DynamicLevelItem;
                attachedItem.SetClickActions();
            }
        }
    }

    public void ResetTile()
    {
        SetAttachedItem();
    }

    #region Pathfinding
    public TileElement Connection { get; private set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;

    public List<TileElement> VerticalNeighbours;
    public List<TileElement> HorizontalNeighbours;
    private List<Vector2> _verticalNeighbourDirList = new List<Vector2>() { new Vector2(-1, 0), new Vector2(1, 0) };
    private List<Vector2> _horizontalNeighbourDirList = new List<Vector2>() { new Vector2(0, 1), new Vector2(0, -1) };

    public void SetNeighbourTiles()
    {
        VerticalNeighbours = new List<TileElement>();
        HorizontalNeighbours = new List<TileElement>();

        foreach (var tile in _verticalNeighbourDirList.Select(dir => GridManager.Instance.GetTileAtPosition(Pos + dir)).Where(tile => tile != null))
        {
            VerticalNeighbours.Add(tile);
        }
        foreach (var tile in _horizontalNeighbourDirList.Select(dir => GridManager.Instance.GetTileAtPosition(Pos + dir)).Where(tile => tile != null))
        {
            HorizontalNeighbours.Add(tile);
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

    public bool IsWalkable(LevelItem item)
    {
        return AttachedItem == item || AttachedItem == null;
    }
    #endregion
}

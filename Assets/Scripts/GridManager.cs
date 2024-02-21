using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : Singleton<GridManager>
{
    private List<TileElement> _tileList;
    public int TileHeight { get; private set; }
    public int TileWidth { get; private set; }

    [SerializeField] private TileElement _tilePrefab;
    private ObjectPool<TileElement> _tilePool;
    [SerializeField] private Transform _tileContainer;

    protected override void Awake()
    {
        base.Awake();

        _tilePool = new ObjectPool<TileElement>(_tilePrefab, 20, _tileContainer);
    }
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
        ResetGrid();
    }

    public void CreateGrid(GridContainer grid)
    {
        _tileList = new List<TileElement>();

        TileHeight = grid[0].GridHeight;
        TileWidth = grid[0].GridWidth;

        for (int i = 0; i < grid[0].GridWidth; i++)
        {
            for (int j = 0; j < grid[0].GridHeight; j++)
            {
                TileElement tileElement = _tilePool.Get();
                tileElement.transform.position = tileElement.TileSize * new Vector3((i * 2) - grid[0].GridWidth + 1, tileElement.transform.position.y, ((grid[0].GridHeight - j - 1) * 2) - grid[0].GridHeight + 1);

                TileVO selectedVO = grid[0].Grid[i * grid[0].GridHeight + j];
                tileElement.Initialize(j, i, selectedVO);

                _tileList.Add(tileElement);
            }
        }

        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, grid[0].GridWidth * 4 + 3f, Camera.main.transform.position.z);

        CameraController.Instance.SetCamera(grid[0].GridHeight > 5);

        foreach (var tile in _tileList)
        {
            tile.SetNeighbourTiles();
            tile.InitializeTileAttachedItem();
        }
    }

    public TileElement GetTileAtPosition(Vector2 pos)
    {
        int row = (int)pos.y;
        int column = (int)pos.x;

        if (row >= 0 && column >= 0 && row < TileHeight && column < TileWidth)
        {
            TileElement tile = _tileList.FirstOrDefault(x => x.Pos == pos);
            return tile;
        }

        return null;
    }

    public List<TileElement> FindPath(TileElement startTile, TileElement targetTile)
    {
        if (startTile == null)
        {
            return null;
        }

        var toSearch = new List<TileElement>() { startTile };
        var processed = new List<TileElement>();

        while (toSearch.Any())
        {
            var current = toSearch[0];
            foreach (var t in toSearch)
                if (t.F < current.F || t.F == current.F && t.H < current.H) current = t;

            processed.Add(current);
            toSearch.Remove(current);

            if (current == targetTile)
            {
                var currentPathTile = targetTile;
                var path = new List<TileElement>();
                var count = 100;
                while (currentPathTile != startTile)
                {
                    path.Add(currentPathTile);
                    currentPathTile = currentPathTile.Connection;
                    count--;
                    if (count < 0) throw new Exception();
                }

                return path;
            }

            List<TileElement> neighbourTiles = new List<TileElement>();
            foreach (TileElement tile in current.VerticalNeighbours)
            {
                neighbourTiles.Add(tile);
            }
            foreach (TileElement tile in current.HorizontalNeighbours)
            {
                neighbourTiles.Add(tile);
            }

            foreach (var neighbour in neighbourTiles.Where(t => t.IsWalkable(startTile.AttachedItem) && !processed.Contains(t)))
            {
                var inSearch = toSearch.Contains(neighbour);

                var costToNeighbor = current.G + current.GetDistance(neighbour);

                if (!inSearch || costToNeighbor < neighbour.G)
                {
                    neighbour.SetG(costToNeighbor);
                    neighbour.SetConnection(current);

                    if (!inSearch)
                    {
                        if (targetTile != null)
                        {
                            neighbour.SetH(neighbour.GetDistance(targetTile));
                            toSearch.Add(neighbour);
                        }
                    }
                }
            }
        }

        return null;
    }

    public void ResetGrid()
    {
        foreach (TileElement tile in _tileList)
        {
            tile.ResetTile();
        }
        _tileList = new List<TileElement>();
        _tilePool.ReturnAll();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private GridContainer _vo;
    [SerializeField] private TileElement _tilePrefab;
    [SerializeField] private Transform _tileContainer;

    private List<TileElement> _tileList;
    private int _tileHeight, _tileWidth;

    private TileElement _selectedTile;

    public void CreateGrid()
    {
        _tileList = new List<TileElement>();

        _tileHeight = _vo[0].GridHeight;
        _tileWidth = _vo[0].GridWidth;

        for (int i = 0; i < _vo[0].GridWidth; i++)
        {
            for (int j = 0; j < _vo[0].GridHeight; j++)
            {
                TileElement tileElement = Instantiate(_tilePrefab, _tileContainer).GetComponent<TileElement>();
                tileElement.transform.position = tileElement.TileSize * new Vector3((i * 2) - _vo[0].GridWidth + 1, tileElement.transform.position.y, ((_vo[0].GridHeight - j - 1) * 2) - _vo[0].GridHeight + 1);

                TileVO selectedVO = _vo[0].Grid[i * _vo[0].GridHeight + j];
                tileElement.Initialize(j, i, selectedVO);

                _tileList.Add(tileElement);
            }
        }

        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, _vo[0].GridWidth * 4 + 3f, Camera.main.transform.position.z);

        CameraController.Instance.SetCamera(_vo[0].GridHeight > 5);

        foreach (var tile in _tileList)
        {
            tile.CheckVehicleCreation();
            tile.SetNeighbourTiles();
        }
    }

    public TileElement GetTileAtPosition(Vector2 pos)
    {
        int row = (int)pos.x;
        int column = (int)pos.y;

        if (row >= 0 && column >= 0 && row < _tileHeight && column < _tileWidth)
        {
            TileElement tile = _tileList.FirstOrDefault(x => x.Pos == pos);
            return tile;
        }

        return null;
    }

    public void SelectTile(TileElement tile)
    {
        _selectedTile = tile;
    }

    public List<TileElement> FindPath(TileElement targetTile)
    {
        var toSearch = new List<TileElement>() { _selectedTile };
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
                while (currentPathTile != _selectedTile)
                {
                    path.Add(currentPathTile);
                    currentPathTile = currentPathTile.Connection;
                    count--;
                    if (count < 0) throw new Exception();
                }

                return path;
            }

            foreach (var neighbour in current.Neighbours.Where(t => t.IsWalkable() && !processed.Contains(t)))
            {
                var inSearch = toSearch.Contains(neighbour);

                var costToNeighbor = current.G + current.GetDistance(neighbour);

                if (!inSearch || costToNeighbor < neighbour.G)
                {
                    neighbour.SetG(costToNeighbor);
                    neighbour.SetConnection(current);

                    if (!inSearch)
                    {
                        neighbour.SetH(neighbour.GetDistance(targetTile));
                        toSearch.Add(neighbour);
                    }
                }
            }
        }
        return null;
    }
}

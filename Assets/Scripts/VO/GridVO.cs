using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

[Serializable]
public class GridVO
{
    [EnumToggleButtons] public Enums.ItemTypes AttachedItemType;

    [OnValueChanged("CreateGrid")]
    public int GridWidth = 4;
    [OnValueChanged("CreateGrid")]
    public int GridHeight = 4;

    [SerializeField][HideInInspector] public List<TileVO> Grid;

#if UNITY_EDITOR
    [ShowInInspector]
    [TableMatrix(SquareCells = true, DrawElementMethod = "DrawElement")]
    private TileVO[,] _editorGrid;
    [SerializeField] private TileVO _insertationTileData;

    private TileVO DrawElement(Rect rect, TileVO tile)
    {
        switch (tile.ItemType)
        {
            case Enums.ItemTypes.None:
                EditorGUI.DrawRect(rect.Padding(1), Color.white);
                break;
            case Enums.ItemTypes.Obstacle:
                EditorGUI.DrawRect(rect.Padding(1), Color.red);
                break;
            case Enums.ItemTypes.Driver:
                EditorGUI.DrawRect(rect.Padding(1), Color.yellow);
                break;
            case Enums.ItemTypes.Vehicle:
                EditorGUI.DrawRect(rect.Padding(1), Color.magenta);
                break;
            default:
                break;
        }

        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            if (Event.current.button == 0)
            {
                tile.ItemType = AttachedItemType;
            }
            else if (Event.current.button == 1)
            {
                tile.ItemType = Enums.ItemTypes.None;
            }

            Serialize();
            _insertationTileData = tile;
            GUI.changed = true;
            Event.current.Use();
        }

        if (tile.ItemType is Enums.ItemTypes.Driver || tile.ItemType is Enums.ItemTypes.Vehicle)
        {
            var labelStyle = EditorStyles.label;
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontSize = 16;
            var newRect = rect.SetSize(rect.size * 0.4f);
            newRect.center = rect.center;
            EditorGUI.DrawRect(newRect, Color.black);
            EditorGUI.LabelField(rect, $"{tile.Pair}");

            if (tile.ItemType is Enums.ItemTypes.Vehicle)
            {

            }
        }

        return tile;
    }

    private void CreateGrid()
    {
        Grid = new List<TileVO>();
        _editorGrid = new TileVO[GridWidth, GridHeight];

        for (int i = 0; i < GridHeight * GridWidth; i++)
        {
            Grid.Add(new TileVO
            {
                ItemType = Enums.ItemTypes.None,
            });
        }

        Deserialize();
    }

    [OnInspectorInit]
    private void OnInit()
    {
        _editorGrid = new TileVO[GridWidth, GridHeight];
        Deserialize();
    }

    private void Serialize()
    {
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                Grid[i * GridHeight + j] = _editorGrid[i, j];
            }
        }
    }
    private void Deserialize()
    {
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                _editorGrid[i, j] = Grid[i * GridHeight + j];
            }
        }
    }
#endif
}
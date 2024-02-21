using System;
using UnityEngine;

[Serializable]
public class TileVO
{
    public LevelItemDataSO LevelItemData = null;
    public Enums.Pairs Pair;
    public Vector3 FacedDirection = Vector3.zero;
}

using System;
using UnityEngine;

[Serializable]
public class TileVO
{
    public Enums.ItemTypes ItemType = Enums.ItemTypes.None;
    public Enums.Pairs Pair = Enums.Pairs.Blue;
    public int VehicleSize = 2;
    public int DirectionCoefficient = 0;
}

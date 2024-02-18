using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridContainer", menuName = "Grid Container")]
public class GridContainer : ScriptableObject
{
    public List<GridVO> Grids;

    public GridVO this[int index]
    {
        get
        {
            return Grids[index];
        }
    }
}
using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DynamicLevelItem : LevelItem
{
    private Renderer[] _rends;
    [SerializeField] private GameObject _highlightItemGO;
    public bool Gone { get; private set; }

    public override void Initialize(TileElement tileElement)
    {
        base.Initialize(tileElement);

        _rends = GetComponentsInChildren<Renderer>();
        ColorItem();
        RemoveHighlight();
    }

    public void SetClickActions()
    {
        if (this is Driver)
        {
            MatchManager.Instance.SetDriver(this as Driver);
        }
        else if (this is Vehicle)
        {
            MatchManager.Instance.SetVehicle(this as Vehicle);
        }
    }

    public void GoOutside()
    {
        Gone = true;
    }

    public void RemoveFromLevel()
    {
        foreach (var tile in RegardedTileList)
        {
            tile.SetAttachedItem();
        }
        GoOutside();
    }

    private void ColorItem()
    {
        Color color = Color.white;

        switch (Pair)
        {
            case Enums.Pairs.Blue:

                color = Color.blue;

                break;
            case Enums.Pairs.Green:

                color = Color.green;

                break;
            case Enums.Pairs.Yellow:

                color = Color.yellow;

                break;
            case Enums.Pairs.Red:

                color = Color.red;

                break;
            case Enums.Pairs.Purple:

                color = Color.magenta;

                break;
            case Enums.Pairs.Orange:

                color = Color.cyan;

                break;
        }

        color.a = 1f;
        foreach (var rend in _rends)
        {
            rend.material.color = color;
        }
    }

    public void HighlightItem()
    {
        if (_highlightItemGO == null)
        {
            return;
        }

        _highlightItemGO.SetActive(true);
    }

    public void RemoveHighlight()
    {
        if (_highlightItemGO == null)
        {
            return;
        }

        _highlightItemGO.SetActive(false);
    }

    public async Task MoveItem(List<TileElement> targetTileList)
    {
        for (int i = targetTileList.Count - 1; i >= 0; i--)
        {
            await MoveItemToOtherTile(targetTileList[i]);
        }
    }

    public async Task MoveItemToOtherTile(TileElement targetTile)
    {
        Vector3 targetPos = targetTile.transform.position;
        RotateItem(targetPos);
        await transform.DOMove(targetPos, 0.25f).OnComplete(() =>
        {
            GetAttachedTiles(targetTile);
        }
        ).AsyncWaitForCompletion();
    }

    private void RotateItem(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;
        float angle = Mathf.Atan(direction.x / direction.z);
        transform.eulerAngles = Mathf.Rad2Deg * angle * Vector3.up;
    }
}

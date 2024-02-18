using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DynamicLevelItem : LevelItem
{
    private Renderer[] _rends;
    [SerializeField] private GameObject _highlightItemGO;

    public override void Initialize(TileElement attachedTile, Enums.Pairs pair)
    {
        base.Initialize(attachedTile, pair);

        _rends = GetComponentsInChildren<Renderer>();
        ColorItem();
        RemoveHighlight();
    }

    public void SetClickActions()
    {
        if (ItemType == Enums.ItemTypes.Driver)
        {
            MatchManager.Instance.SetDriver(this);
        }
        else if (ItemType == Enums.ItemTypes.Vehicle)
        {
            MatchManager.Instance.SetCar(this);
        }
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
        await transform.DOMove(targetTile.transform.position, 0.25f).OnComplete(() =>
        {
            SetAttachedTile(targetTile);
        }
        ).AsyncWaitForCompletion();
    }
}

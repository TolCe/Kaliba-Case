using UnityEngine;

public class LevelItem : MonoBehaviour
{
    public TileElement AttachedTile { get; private set; }
    public Enums.ItemTypes ItemType;
    public Enums.Pairs Pair { get; private set; }

    public virtual void Initialize(TileElement attachedTile, Enums.Pairs pair)
    {
        SetAttachedTile(attachedTile);
        Pair = pair;

        transform.localPosition = Vector3.zero;
        gameObject.SetActive(true);
    }

    public void SetAttachedTile(TileElement attachedTile)
    {
        AttachedTile?.SetAttachedItem();
        AttachedTile = attachedTile;
        attachedTile.SetAttachedItem(this);
        transform.SetParent(attachedTile.transform, true);
    }
}

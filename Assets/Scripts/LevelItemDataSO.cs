using UnityEngine;

[CreateAssetMenu(fileName = "LevelItemData_", menuName = "Level Item Data")]
public class LevelItemDataSO : ScriptableObject
{
    public Enums.ItemTypes ItemType;
    public int Size;
}

using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/ItemSO")]
public class ItemSO : ScriptableObject
{
    // Item information
    public string ItemName = "";
    public string ItemType = "";
    public string ItemDescription = "";
    public string ItemEffect = "";

    // Inventory sprite
    public Sprite ItemInventorySprite = null;

    // Item usage
    public float ItemRange = 0;
    public float ItemCooldown = 0;
    public int ItemUsage = -1;
}
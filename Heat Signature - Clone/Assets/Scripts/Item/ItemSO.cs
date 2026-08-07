using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/ItemSO")]
public class ItemSO : ScriptableObject
{
    //Inventory 
    public string ItemDescription = "";
    public Sprite ItemInventorySprite = null;

    //Item Usage (If Item has -1 usage it can be used infinitely)
    public float ItemRange = 0;
    public float ItemCooldown = 0;
    public int ItemUsage = -1;

}

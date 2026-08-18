using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const int INVENTORY_SIZE = 8;

    [Header("Inventory Slots")]
    [SerializeField]
    private Item[] _inventorySlots = new Item[INVENTORY_SIZE];

    [Header("Starting Gadgets")]
    [SerializeField]
    private Item[] _startingGadgets = new Item[4];

    [Header("Primary / Secondary")]
    [SerializeField]
    private Item _primaryItem;

    [SerializeField]
    private Item _secondaryItem;

    private void Start()
    {
        for (int i = 0; i < _startingGadgets.Length; i++)
        {
            if (_startingGadgets[i] != null)
            {
                _inventorySlots[i] = _startingGadgets[i];
            }
        }
    }

    // -------------------------
    // INVENTORY
    // -------------------------

    public Item[] InventorySlots
    {
        get { return _inventorySlots; }
    }

    public bool AddItem(Item item)
    {
        if (item == null)
        {
            return false;
        }

        for (int i = 0; i < _inventorySlots.Length; i++)
        {
            if (_inventorySlots[i] == null)
            {
                _inventorySlots[i] = item;

                Debug.Log("Picked up: " + GetItemName(item));

                return true;
            }
        }

        Debug.Log("Inventory is full!");

        return false;
    }

    public void MoveItem(int fromSlot, int toSlot)
    {
        if (fromSlot < 0 || fromSlot >= INVENTORY_SIZE)
        {
            return;
        }

        if (toSlot < 0 || toSlot >= INVENTORY_SIZE)
        {
            return;
        }

        (_inventorySlots[toSlot], _inventorySlots[fromSlot]) = (_inventorySlots[fromSlot], _inventorySlots[toSlot]);
    }
    public void EnableSlot(int activeSlot)
    {
        Item item = _inventorySlots[activeSlot];
        item.gameObject.SetActive(true);
        if(item.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
    public void DisableSlot(int activeSlot)
    {
        Item item = _inventorySlots[activeSlot];
        item.gameObject.SetActive(false);
        if(item.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
    public Item GetItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= INVENTORY_SIZE)
        {
            return null;
        }

        return _inventorySlots[slotIndex];
    }

    // -------------------------
    // PRIMARY
    // -------------------------


    // -------------------------
    // SECONDARY
    // -------------------------



    // -------------------------
    // STASH
    // -------------------------


    // -------------------------
    // ITEM NAME
    // -------------------------

    private string GetItemName(Item item)
    {
        if (item == null)
        {
            return "";
        }

        if (item.ItemSO != null &&
            !string.IsNullOrEmpty(item.ItemSO.ItemName))
        {
            return item.ItemSO.ItemName;
        }

        return item.gameObject.name;
    }
}
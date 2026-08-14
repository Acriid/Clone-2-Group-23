using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const int INVENTORY_SIZE = 8;

    [SerializeField]
    private Item[] _inventorySlots = new Item[INVENTORY_SIZE];

    [SerializeField]
    private Item[] _startingGadgets = new Item[4];

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

    // Inventory slots
    public Item[] InventorySlots
    {
        get { return _inventorySlots; }
    }

    // Add item
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

    // Move item
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

        Item item = _inventorySlots[fromSlot];

        _inventorySlots[fromSlot] = _inventorySlots[toSlot];
        _inventorySlots[toSlot] = item;
    }

    // Get item
    public Item GetItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= INVENTORY_SIZE)
        {
            return null;
        }

        return _inventorySlots[slotIndex];
    }

    // Get Slot 1 item
    public Item GetLeftClickItem()
    {
        return _inventorySlots[0];
    }

    // Get Slot 2 item
    public Item GetRightClickItem()
    {
        return _inventorySlots[1];
    }

    // Get item name
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
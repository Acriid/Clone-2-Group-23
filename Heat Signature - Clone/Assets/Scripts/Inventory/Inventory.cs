using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Constants.
    private const int INVENTORY_SIZE = 8;

    // Private variables.
    [SerializeField] private Item[] _inventorySlots = new Item[INVENTORY_SIZE];

    // Public getter.
    public Item[] InventorySlots
    {
        get { return _inventorySlots; }
    }

    // Adds an item to the first available inventory slot.
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
                return true;
            }
        }

        return false;
    }

    // Moves an item between two inventory slots.
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

    // Gets an item from a specific slot.
    public Item GetItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= INVENTORY_SIZE)
        {
            return null;
        }

        return _inventorySlots[slotIndex];
        Debug.Log("Item in slot " + slotIndex + ": " + _inventorySlots[slotIndex]);
        
        
    }

    // Gets the item assigned to Slot 1.
    public Item GetLeftClickItem()
    {
        return _inventorySlots[0];
    }

    // Gets the item assigned to Slot 2.
    public Item GetRightClickItem()
    {
        return _inventorySlots[1];
    }
}
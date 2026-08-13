using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const int INVENTORY_SIZE = 8;

    [SerializeField] private Item[] _inventorySlots = new Item[INVENTORY_SIZE];

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

                Debug.Log("Picked up: " + item.name);

                return true;
            }
        }

        Debug.Log("Inventory is full!");

        return false;
    }

    public void MoveItem(int fromSlot, int toSlot)
    {
        if (fromSlot < 0 || fromSlot >= INVENTORY_SIZE)
            return;

        if (toSlot < 0 || toSlot >= INVENTORY_SIZE)
            return;

        Item item = _inventorySlots[fromSlot];

        _inventorySlots[fromSlot] = _inventorySlots[toSlot];
        _inventorySlots[toSlot] = item;
    }

    public Item GetItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= INVENTORY_SIZE)
        {
            return null;
        }

        return _inventorySlots[slotIndex];
    }

    public Item GetLeftClickItem()
    {
        return _inventorySlots[0];
    }

    public Item GetRightClickItem()
    {
        return _inventorySlots[1];
    }
}
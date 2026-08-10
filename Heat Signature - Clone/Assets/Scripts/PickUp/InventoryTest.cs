using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryTest : MonoBehaviour
{
    // The item currently being hovered/selected.
    [SerializeField] private Item _nearbyItem;

    // Picks up the item when E is pressed.
    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            PickUpItem();
        }
    }

    // Picks up the selected item.
    private void PickUpItem()
    {
        if (_nearbyItem != null)
        {
            _nearbyItem.PickUpItem();
        }
    }

    // Allows another system to set the nearby item.
    public void SetNearbyItem(Item item)
    {
        _nearbyItem = item;
    }
}
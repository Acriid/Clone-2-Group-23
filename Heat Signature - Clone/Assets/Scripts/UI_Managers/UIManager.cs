using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Private variables.
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameObject _inventoryPanel;

    [SerializeField] private GameObject[] _inventorySlotObjects;

    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _itemDescriptionText;

    [SerializeField] private Image _itemIconImage;

    // Functions.
    private void Start()
    {
        UpdateInventoryUI();
        CloseInventory();
    }

    public void OpenInventory()
    {
        if (_inventoryPanel != null)
        {
            _inventoryPanel.SetActive(true);
            UpdateInventoryUI();
        }
    }

    public void CloseInventory()
    {
        if (_inventoryPanel != null)
        {
            _inventoryPanel.SetActive(false);
        }
    }

    public void UpdateInventoryUI()
    {
        if (_inventory == null)
        {
            return;
        }

        for (int i = 0; i < _inventorySlotObjects.Length; i++)
        {
            if (_inventorySlotObjects[i] == null)
            {
                continue;
            }

            Item item = _inventory.GetItem(i);

            UpdateSlot(i, item);
        }
    }

    private void UpdateSlot(int slotIndex, Item item)
    {
        if (item == null)
        {
            return;
        }

        if (item.ItemSO == null)
        {
            return;
        }

        // Item UI updating will be added here.
    }

    public void ShowItemInformation(Item item)
    {
        if (item == null || item.ItemSO == null)
        {
            return;
        }

        if (_itemNameText != null)
        {
            _itemNameText.text = item.gameObject.name;
        }

        if (_itemDescriptionText != null)
        {
            _itemDescriptionText.text = item.ItemSO.ItemDescription;
        }

        if (_itemIconImage != null)
        {
            _itemIconImage.sprite = item.ItemSO.ItemInventorySprite;
        }
    }
}
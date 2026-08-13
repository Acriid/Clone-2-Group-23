using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // =========================
    // INVENTORY
    // =========================

    [Header("Inventory")]
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private Inventory _inventory;


    // =========================
    // INVENTORY SLOT UI
    // =========================

    [Header("Inventory Slots")]

    // The buttons for Slot 1 - Slot 8.
    [SerializeField] private Button[] _itemButtons = new Button[8];

    // The Image components inside Slot 1 - Slot 8.
    [SerializeField] private Image[] _itemSprites = new Image[8];

    // The text components inside Slot 1 - Slot 8.
    [SerializeField] private TMP_Text[] _itemNames = new TMP_Text[8];


    // =========================
    // ITEM INFORMATION PANEL
    // =========================

    [Header("Item Information Panel")]

    [SerializeField] private GameObject _itemInformationPanel;

    [SerializeField] private TMP_Text _itemType;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemDescription;
    [SerializeField] private TMP_Text _itemEffect;


    // =========================
    // START
    // =========================

    private void Start()
    {
        // Hide the inventory when the game starts.
        if (_inventoryPanel != null)
        {
            _inventoryPanel.SetActive(false);
        }

        // Hide the item information panel when the game starts.
        HideItemInformation();

        // Make sure the inventory UI starts empty.
        UpdateInventoryDisplay();
    }


    // =========================
    // UPDATE
    // =========================

    private void Update()
    {
        // Press SPACE to open/close inventory.
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }

        // Keep the inventory UI updated.
        UpdateInventoryDisplay();
    }


    // =========================
    // INVENTORY TOGGLE
    // =========================

    public void ToggleInventory()
    {
        if (_inventoryPanel == null)
        {
            return;
        }

        bool inventoryIsOpen = _inventoryPanel.activeSelf;

        _inventoryPanel.SetActive(!inventoryIsOpen);

        // If closing inventory, hide item information.
        if (inventoryIsOpen)
        {
            HideItemInformation();
        }

        // Update the inventory whenever it opens.
        if (!inventoryIsOpen)
        {
            UpdateInventoryDisplay();
        }
    }


    // =========================
    // UPDATE INVENTORY DISPLAY
    // =========================

    private void UpdateInventoryDisplay()
    {
        if (_inventory == null)
        {
            return;
        }

        for (int i = 0; i < 8; i++)
        {
            Item item = _inventory.GetItem(i);

            // -------------------------
            // EMPTY SLOT
            // -------------------------

            if (item == null)
            {
                // Remove the name.
                if (_itemNames[i] != null)
                {
                    _itemNames[i].text = "";
                }

                // Hide the sprite.
                if (_itemSprites[i] != null)
                {
                    _itemSprites[i].enabled = false;
                    _itemSprites[i].sprite = null;
                }

                // Empty slot cannot be clicked.
                if (_itemButtons[i] != null)
                {
                    _itemButtons[i].interactable = false;
                }

                continue;
            }
            // ITEM EXISTS
    
            string itemName = GetItemName(item);


            // Show item name.
            if (_itemNames[i] != null)
            {
                _itemNames[i].text = itemName;
            }


            // Show the item's actual sprite.
            if (_itemSprites[i] != null)
            {
                if (item.ItemSO != null &&
                    item.ItemSO.ItemInventorySprite != null)
                {
                    _itemSprites[i].sprite =
                        item.ItemSO.ItemInventorySprite;

                    _itemSprites[i].enabled = true;
                }
                else
                {
                    _itemSprites[i].sprite = null;
                    _itemSprites[i].enabled = false;
                }
            }


            // Allow the slot to be clicked.
            if (_itemButtons[i] != null)
            {
                _itemButtons[i].interactable = true;
            }
        }
    }

    // GET ITEM NAME

    private string GetItemName(Item item)
    {
        if (item == null)
        {
            return "";
        }

        string objectName = item.gameObject.name.ToLower();


        if (objectName.Contains("longblade"))
        {
            return "LONGBLADE";
        }

        if (objectName.Contains("concussion"))
        {
            return "CONCUSSION HAMMER";
        }

        if (objectName.Contains("shortblade"))
        {
            return "SHORTBLADE";
        }

        if (objectName.Contains("gun"))
        {
            return "GUN";
        }

        if (objectName.Contains("visitor"))
        {
            return "VISITOR";
        }

        if (objectName.Contains("sidewinder"))
        {
            return "SIDEWINDER";
        }

        if (objectName.Contains("swapper"))
        {
            return "SWAPPER";
        }

        if (objectName.Contains("slipstream"))
        {
            return "SLIPSTREAM";
        }


        // If none of the names match,
        // use the actual GameObject name.
        return item.gameObject.name;
    }


    // CLICK INVENTORY SLOT

    public void ShowItemInformationForSlot(int slotIndex)
    {
        if (_inventory == null)
        {
            return;
        }

        if (slotIndex < 0 || slotIndex >= 8)
        {
            return;
        }

        Item item = _inventory.GetItem(slotIndex);


        // Nothing in this slot.
        if (item == null)
        {
            HideItemInformation();
            return;
        }


        // Check which item was actually picked up.
        string itemName = GetItemName(item);


        switch (itemName)
        {
            case "LONGBLADE":

                ShowItemInformation(
                    "Melee",
                    "LONGBLADE",
                    "Hold the button to aim your dash, release to strike.",
                    "8m Dash\n0.4s Cooldown\nQuiet Strike"
                );

                break;


            case "CONCUSSION HAMMER":

                ShowItemInformation(
                    "Melee",
                    "CONCUSSION HAMMER",
                    "Hold the button to aim your dash, release to strike.",
                    "8m Dash\nVery Short Recovery\nHigh Knockback"
                );

                break;


            case "SHORTBLADE":

                ShowItemInformation(
                    "Melee",
                    "SHORTBLADE",
                    "Aim at a nearby target to strike automatically.",
                    "2m Dash\nVery Short Recovery"
                );

                break;


            case "GUN":

                ShowItemInformation(
                    "Firearm",
                    "GUN",
                    "Hold the button to aim your shot, release to fire.",
                    "Infinite Range\n1s per Shot"
                );

                break;


            case "VISITOR":

                ShowItemInformation(
                    "Teleporter",
                    "VISITOR",
                    "Hold the button to choose a destination, release to teleport.",
                    "Returns to original position\n1 second"
                );

                break;


            case "SIDEWINDER":

                ShowItemInformation(
                    "Teleporter",
                    "SIDEWINDER",
                    "Hold the button to choose a destination, release to teleport.",
                    "Requires a Direct Path"
                );

                break;


            case "SWAPPER":

                ShowItemInformation(
                    "Teleporter",
                    "SWAPPER",
                    "Hold the button to select a target, release to swap positions.",
                    "Target: Enemy\nPermanent Swap"
                );

                break;


            case "SLIPSTREAM":

                ShowItemInformation(
                    "Gadget",
                    "SLIPSTREAM",
                    "Hold the button to activate.",
                    "Time: 10x Slower\nMovement: 5x Faster\nDuration: 10s"
                );

                break;


            default:

                // Unknown item.
                ShowItemInformation(
                    "",
                    itemName,
                    "",
                    ""
                );

                break;
        }
    }


    // SHOW INFORMATION

    private void ShowItemInformation(
        string itemType,
        string itemName,
        string itemDescription,
        string itemEffect)
    {
        if (_itemInformationPanel != null)
        {
            _itemInformationPanel.SetActive(true);
        }

        if (_itemType != null)
        {
            _itemType.text = itemType;
        }

        if (_itemName != null)
        {
            _itemName.text = itemName;
        }

        if (_itemDescription != null)
        {
            _itemDescription.text = itemDescription;
        }

        if (_itemEffect != null)
        {
            _itemEffect.text = itemEffect;
        }
    }


    // HIDE INFORMATION

    public void HideItemInformation()
    {
        if (_itemInformationPanel != null)
        {
            _itemInformationPanel.SetActive(false);
        }
    }

    // TOGGLE INFORMATION

    public void ToggleItemInformationPanel()
    {
        if (_itemInformationPanel == null)
        {
            return;
        }

        bool isOpen = _itemInformationPanel.activeSelf;

        if (isOpen)
        {
            HideItemInformation();
        }
        else
        {
            _itemInformationPanel.SetActive(true);
        }
    }
}
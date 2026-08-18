using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Inventory")]

    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private Inventory _inventory;


    [Header("Inventory Slots")]

    [SerializeField] private Button[] _itemButtons = new Button[8];
    [SerializeField] private Image[] _itemSprites = new Image[8];
    [SerializeField] private TMP_Text[] _itemNames = new TMP_Text[8];


    [Header("Item Information Panel")]

    [SerializeField] private GameObject _itemInformationPanel;

    [SerializeField] private TMP_Text _itemType;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemDescription;
    [SerializeField] private TMP_Text _itemEffect;


    private void Start()
    {
        if (_inventoryPanel != null)
        {
            _inventoryPanel.SetActive(false);
        }

        HideItemInformation();

        UpdateInventoryDisplay();
    }


    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }

        UpdateInventoryDisplay();
    }


    public void ToggleInventory()
    {
        if (_inventoryPanel == null)
        {
            return;
        }

        bool inventoryIsOpen = _inventoryPanel.activeSelf;

        _inventoryPanel.SetActive(!inventoryIsOpen);

        if (inventoryIsOpen)
        {
            HideItemInformation();
        }
        else
        {
            UpdateInventoryDisplay();
        }
    }


    private void UpdateInventoryDisplay()
    {
        if (_inventory == null)
        {
            return;
        }

        for (int i = 0; i < 8; i++)
        {
            if (_itemButtons[i] == null)
            {
                continue;
            }

            Item item = _inventory.GetItem(i);

            if (item == null)
            {
                if (_itemNames[i] != null)
                {
                    _itemNames[i].text = "";
                }

                if (_itemSprites[i] != null)
                {
                    _itemSprites[i].sprite = null;
                    _itemSprites[i].enabled = false;
                }

                _itemButtons[i].interactable = false;

                continue;
            }

            string itemName = GetItemName(item);

            if (_itemNames[i] != null)
            {
                _itemNames[i].text = itemName;
            }

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

            _itemButtons[i].interactable = true;
        }
    }


    private string GetItemName(Item item)
    {
        if (item == null)
        {
            return "";
        }

        if (item.ItemSO != null &&
            !string.IsNullOrEmpty(item.ItemSO.ItemName))
        {
            return item.ItemSO.ItemName.ToUpper();
        }

        return item.gameObject.name.ToUpper();
    }


    public void ShowVisitorInformation()
    {
        ShowItemInformation(
            "Teleporter",
            "VISITOR",
            "Teleport to a selected location and return to your original position.",
            "Returns to original position\n2 second timer"
        );
    }


    public void ShowSidewinderInformation()
    {
        ShowItemInformation(
            "Teleporter",
            "SIDEWINDER",
            "Teleport to a selected location while following a direct path.",
            "Requires a Direct Path"
        );
    }


    public void ShowSwapperInformation()
    {
        ShowItemInformation(
            "Teleporter",
            "SWAPPER",
            "Select a target and swap positions with it.",
            "Target: Enemy\nPermanent Swap"
        );
    }


    public void ShowSlipstreamInformation()
    {
        ShowItemInformation(
            "Gadget",
            "SLIPSTREAM",
            "Activate Slipstream to temporarily change time and movement.",
            "Time: 10x Slower\nMovement: 5x Faster\nDuration: 10s"
        );
    }


    public void ShowLongbladeInformation()
    {
        ShowItemInformation(
            "Melee",
            "LONGBLADE",
            "Hold the button to aim your dash, then release to strike.",
            "8m Dash\n0.4s Cooldown\nQuiet Strike"
        );
    }


    public void ShowConcussionHammerInformation()
    {
        ShowItemInformation(
            "Melee",
            "CONCUSSION HAMMER",
            "Hold the button to aim your dash, then release to strike.",
            "8m Dash\nVery Short Recovery\nHigh Knockback"
        );
    }


    public void ShowShortbladeInformation()
    {
        ShowItemInformation(
            "Melee",
            "SHORTBLADE",
            "Aim at a nearby target to perform a short dash attack.",
            "2m Dash\nVery Short Recovery"
        );
    }


    public void ShowGunInformation()
    {
        ShowItemInformation(
            "Firearm",
            "GUN",
            "Hold the button to aim your shot, then release to fire.",
            "Infinite Range\n1s per Shot"
        );
    }


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


    public void HideItemInformation()
    {
        if (_itemInformationPanel != null)
        {
            _itemInformationPanel.SetActive(false);
        }
    }


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
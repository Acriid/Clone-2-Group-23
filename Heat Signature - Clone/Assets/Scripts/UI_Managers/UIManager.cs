using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    // Private variables.

    // Inventory panel.
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private Inventory _inventory;

    // Item information panel.
    [SerializeField] private GameObject _itemInformationPanel;

    // Item information text.
    [SerializeField] private TMP_Text _itemType;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemDescription;
    [SerializeField] private TMP_Text _itemEffect;
    [SerializeField] private TMP_Text[] _itemNames;


    // Functions.

    private void Update()
    {
        // Press Space to open or close the inventory.
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }


    // Opens or closes the inventory.
    private void ToggleInventory()
    {
        if (_inventoryPanel != null)
        {
            bool inventoryIsOpen = _inventoryPanel.activeSelf;

            _inventoryPanel.SetActive(!inventoryIsOpen);

            // Hide item information when closing the inventory.
            if (inventoryIsOpen)
            {
                HideItemInformation();
            }
        }
    }
    private void UpdateInventoryNames()
{
    if (_inventory == null)
    {
        return;
    }

    for (int i = 0; i < _itemNames.Length; i++)
    {
        Item item = _inventory.GetItem(i);

        if (item != null)
        {
            if (item.gameObject.name.Contains("Longblade"))
            {
                _itemNames[i].text = "LONGBLADE";
            }
            else if (item.gameObject.name.Contains("Concussion"))
            {
                _itemNames[i].text = "CONCUSSION HAMMER";
            }
            else if (item.gameObject.name.Contains("Shortblade"))
            {
                _itemNames[i].text = "SHORTBLADE";
            }
            else if (item.gameObject.name.Contains("Gun"))
            {
                _itemNames[i].text = "GUN";
            }
            else if (item.gameObject.name.Contains("Visitor"))
            {
                _itemNames[i].text = "VISITOR";
            }
            else if (item.gameObject.name.Contains("Sidewinder"))
            {
                _itemNames[i].text = "SIDEWINDER";
            }
            else if (item.gameObject.name.Contains("Swapper"))
            {
                _itemNames[i].text = "SWAPPER";
            }
            else if (item.gameObject.name.Contains("Slipstream"))
            {
                _itemNames[i].text = "SLIPSTREAM";
            }
        }
        else
        {
            _itemNames[i].text = "";
        }
    }
}


    // Displays information for the selected item.
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


    // Shows Longblade information.
    public void ShowLongbladeInformation()
    {
        ShowItemInformation(
            "Melee",
            "LONGBLADE",
            "Hold the button to aim your dash, release to strike.",
            "8m Dash\n0.4s Cooldown\nQuiet Strike"
        );
    }


    // Shows Concussion Hammer information.
    public void ShowConcussionHammerInformation()
    {
        ShowItemInformation(
            "Melee",
            "CONCUSSION HAMMER",
            "Hold the button to aim your dash, release to strike.",
            "8m Dash\nVery Short Recovery\nHigh Knockback"
        );
    }


    // Shows Shortblade information.
    public void ShowShortbladeInformation()
    {
        ShowItemInformation(
            "Melee",
            "SHORTBLADE",
            "Aim at a nearby target to strike automatically.",
            "2m Dash\nVery Short Recovery"
        );
    }


    // Shows Gun information.
    public void ShowGunInformation()
    {
        ShowItemInformation(
            "Firearm",
            "GUN",
            "Hold the button to aim your shot, release to fire.",
            "Infinite Range\n1s per Shot"
        );
    }


    // Shows Visitor information.
    public void ShowVisitorInformation()
    {
        ShowItemInformation(
            "Teleporter",
            "VISITOR",
            "Hold the button to choose a destination, release to teleport.",
            "Returns to original position\n1 second"
        );
    }


    // Shows Sidewinder information.
    public void ShowSidewinderInformation()
    {
        ShowItemInformation(
            "Teleporter",
            "SIDEWINDER",
            "Hold the button to choose a destination, release to teleport.",
            "Requires a Direct Path"
        );
    }


    // Shows Swapper information.
    public void ShowSwapperInformation()
    {
        ShowItemInformation(
            "Teleporter",
            "SWAPPER",
            "Hold the button to select a target, release to swap positions.",
            "Target: Enemy\nPermanent Swap"
        );
    }


    // Shows Slipstream information.
    public void ShowSlipstreamInformation()
    {
        ShowItemInformation(
            "Gadget",
            "SLIPSTREAM",
            "Hold the button to activate.",
            "Time: 10x Slower\nMovement: 5x Faster\nDuration: 10s"
        );
    }


    // Hides the item information panel.
    public void HideItemInformation()
    {
        if (_itemInformationPanel != null)
        {
            _itemInformationPanel.SetActive(false);
        }
    }
}
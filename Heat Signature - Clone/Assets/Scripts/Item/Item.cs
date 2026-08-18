using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] protected ItemSO _itemSO;

    [Header("Inventory")]
    [SerializeField] private Inventory _inventory;

    [Header("UI")]
    [SerializeField] private GameObject _itemInformationPanel;

    [Header("Item")]
    [SerializeField] protected GameObject _parentObject;

    [Header("Enemy Item")]
    [SerializeField] protected bool _enemyItem = false;

    [Header("Time")]
    [SerializeField] protected TimeManager _timeManager;

    protected float _timeVariable = 1f;

    public ItemSO ItemSO
    {
        get { return _itemSO; }
        set { _itemSO = value; }
    }

    public GameObject ItemInformationPanel
    {
        get { return _itemInformationPanel; }
        set { _itemInformationPanel = value; }
    }

    // This is the base UseItem function.
    // Individual items can override this.
    public virtual void UseItem()
    {
        if (_itemSO == null)
        {
            Debug.LogWarning("ItemSO is not assigned to " + gameObject.name);
            return;
        }

        Debug.Log("Using item: " + _itemSO.ItemName);
    }

    public virtual void ThrowItem()
    {
        Debug.Log("Throwing item: " + GetItemName());
    }

    public virtual void DropItem()
    {
        Debug.Log("Dropping item: " + GetItemName());
    }

    public virtual void PickUpItem()
    {
        if (_inventory == null)
        {
            Debug.LogWarning(
                "Inventory is not assigned to " + gameObject.name
            );

            return;
        }

        bool added = _inventory.AddItem(this);

        if (added)
        {
            Debug.Log(GetItemName() + " picked up!");

            // Hide the world item after picking it up.
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Inventory is full!");
        }
    }

    // Allows the player to click the item in the world to pick it up.
    private void OnMouseDown()
    {
        PickUpItem();
    }

    protected string GetItemName()
    {
        if (_itemSO != null &&
            !string.IsNullOrEmpty(_itemSO.ItemName))
        {
            return _itemSO.ItemName;
        }

        return gameObject.name;
    }
}
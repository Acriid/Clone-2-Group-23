using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected ItemSO _itemSO;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameObject _itemInformationPanel;
    [SerializeField] protected GameObject _parentObject;

    [SerializeField] protected bool _enemyItem = false;
    [SerializeField] protected TimeManager _timeManager = null;

    protected float _timeVariable = 1f;

    // ItemSO getter and setter
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

    // Use item
    public virtual void UseItem()
    {
        Debug.Log("Using item: " + GetItemName());
    }

    // Throw item
    public virtual void ThrowItem()
    {
    }

    // Drop item
    public virtual void DropItem()
    {
    }

    // Pick up item
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

            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Inventory is full!");
        }
    }

    // Click world item
    private void OnMouseDown()
    {
        PickUpItem();
    }

    // Get item name
    private string GetItemName()
    {
        if (_itemSO != null &&
            !string.IsNullOrEmpty(_itemSO.ItemName))
        {
            return _itemSO.ItemName;
        }

        return gameObject.name;
    }
}
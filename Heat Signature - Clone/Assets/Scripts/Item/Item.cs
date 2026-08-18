using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected ItemSO _itemSO;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameObject _itemInformationPanel;
    [SerializeField] protected GameObject _parentObject;

    [SerializeField] protected bool _enemyItem = false;
    [SerializeField] protected TimeManager _timeManager = null;

    [SerializeField] private VisitorTest _visitorTest;

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

    public virtual void UseItem()
    {
        if (_itemSO == null)
        {
            Debug.LogWarning("ItemSO is not assigned to " + gameObject.name);
            return;
        }

        string itemName = _itemSO.ItemName.ToUpper();

        if (_visitorTest == null)
        {
            Debug.LogWarning(
                "VisitorTest is not assigned to " + gameObject.name
            );

            return;
        }

        switch (itemName)
        {
            case "VISITOR":
                _visitorTest.UseVisitor();
                break;

            case "SIDEWINDER":
                _visitorTest.UseSidewinder();
                break;

            case "SWAPPER":
                _visitorTest.UseSwapper();
                break;

            case "SLIPSTREAM":
                _visitorTest.UseSlipstream();
                break;

            default:
                Debug.Log("Using item: " + itemName);
                break;
        }
    }

    public virtual void ThrowItem()
    {
    }

    public virtual void DropItem()
    {
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

            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Inventory is full!");
        }
    }

    private void OnMouseDown()
    {
        PickUpItem();
    }

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
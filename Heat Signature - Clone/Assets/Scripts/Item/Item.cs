using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//Parent class for other scripts to use
public abstract class Item : MonoBehaviour
{
    //ItemSO gets set in inspector
    [SerializeField] protected ItemSO _itemSO;
    // Inventory that will receive this item.
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameObject _itemInformationPanel;
    [SerializeField] protected GameObject _parentObject;
    

    //Time Manager
    [SerializeField] protected bool _enemyItem = false;
    [SerializeField] protected TimeManager _timeManager = null;


    protected float _timeVariable = 1f;
    

    //Public getter and setter for _itemSO
    public ItemSO ItemSO 
    {
        get {return _itemSO;}
        set {_itemSO = value;}
    }

    public GameObject ItemInformationPanel { get => _itemInformationPanel; set => _itemInformationPanel = value; }

    public virtual void UseItem(){}
    public virtual void ThrowItem(){}
    public virtual void DropItem(){}

     // Called when the player clicks the item.
    public virtual void PickUpItem()
    {
        if (_inventory == null)
        {
            Debug.LogWarning("Inventory is not assigned to " + gameObject.name);
            return;
        }

        bool added = _inventory.AddItem(this);

        if (added)
        {
            Debug.Log(gameObject.name + " picked up!");

            // Remove the item from the world.
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Inventory is full!");
        }
    }

    // Left-click the item to pick it up.
    private void OnMouseDown()
    {
        PickUpItem();
    }

}

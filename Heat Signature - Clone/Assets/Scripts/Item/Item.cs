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
    
  
  /// These functions are simple,so from here on will be deleted once ive worked on the gadget machenic on picking the object.
    // Picks up this item.

    void Start()
    {
        _inventory = FindFirstObjectByType<Inventory>();
    }

    //it checks if E was pressed and that the item was picked up.
     private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            PickUpItem();
        }
    }
    public virtual void PickUpItem()
    {
         if (_inventory == null)
        {
            Debug.LogWarning("No Inventory found in the scene.");
            return;
        }

        if (_itemSO == null)
        {
            Debug.LogWarning("No ItemSO assigned to this item.");
            return;
        }

        bool itemAdded = _inventory.AddItem(this);

        if (itemAdded)
        {
            Debug.Log("Item picked up.");

            // Hide the world object.
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Inventory is full.");
        }
    
    }

    // Checks if the mouse is hovering over the item.
    private void OnMouseOver()
    {
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            PickUpItem();
        }
    }

}

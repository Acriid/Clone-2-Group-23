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
    public virtual void PickUpItem(){}
    
  
  
     
   

}

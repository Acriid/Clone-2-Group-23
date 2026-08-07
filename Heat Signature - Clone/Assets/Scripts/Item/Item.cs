using Unity.VisualScripting;
using UnityEngine;

//Parent class for other scripts to use
public abstract class Item : MonoBehaviour
{
    //ItemSO gets set in inspector
    [SerializeField] protected ItemSO _itemSO;

    //Public getter and setter for _itemSO
    public ItemSO ItemSO 
    {
        get {return _itemSO;}
        set {_itemSO = value;}
    }
    public virtual void UseItem(){}
    public virtual void ThrowItem(){}
    public virtual void DropItem(){}
    public virtual void PickUpItem(){}
}

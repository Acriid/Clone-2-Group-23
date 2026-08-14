using UnityEngine;

public class PlayerUseItems : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = null;
    [SerializeField] private Inventory _inventory = null;
    void OnEnable()
    {
        if(_inputReader != null)
        {
            _inputReader.EnableClickAction();
            _inputReader.EnableRightClickAction();

            _inputReader.OnLeftClick += OnLeftClick;
            _inputReader.OnRightClick += OnRightClick;

        }
    }
    void OnDisable()
    {
        if(_inputReader != null)
        {
            _inputReader.DisableClickAction();
            _inputReader.DisableRightClickAction();

            _inputReader.OnLeftClick -= OnLeftClick;
            _inputReader.OnRightClick -= OnRightClick;
        }        
    }

    private void OnLeftClick()
    {
        if(_inventory != null)
        {
            Item primaryItem = _inventory.GetItem(0);
            if(primaryItem != null)
            {
                primaryItem.UseItem();
            }
        }
    }
    private void OnRightClick()
    {
        if(_inventory != null)
        {
            Item secondaryItem = _inventory.GetItem(1);
            if(secondaryItem != null)
            {
                secondaryItem.UseItem();
            }
        }        
    }
}

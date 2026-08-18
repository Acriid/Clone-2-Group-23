using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private int _slotIndex;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(
            "Inventory slot clicked: " +
            _slotIndex +
            " | Mouse button: " +
            eventData.button
        );

        if (_uiManager == null)
        {
            Debug.LogError("UIManager is NOT assigned!");
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("LEFT CLICK detected!");

            _uiManager.AssignPrimary(_slotIndex);
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("RIGHT CLICK detected!");

            _uiManager.AssignSecondary(_slotIndex);
        }
    }
}
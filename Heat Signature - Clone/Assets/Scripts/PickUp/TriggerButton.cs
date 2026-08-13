using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerButton : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject itemInformationPanel;

     void OnEnable()
    {
        inputReader.OnInteract += HandleInteract;
        inputReader.EnableInteractAction();
    }

    private void OnDisable()
    {
        inputReader.OnInteract -= HandleInteract;
        inputReader.DisableInteractAction();
    }

    private void HandleInteract()
    {
        if (uiManager != null)
        {
            global::System.Object value = uiManager.ToggleItemInformationPanel();
        }  
         
        
    }   

}


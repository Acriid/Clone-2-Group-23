using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Inputs/InputReader")]
public class InputReader : ScriptableObject
{
    #region InputAction Variables
    private InputActions _inputActions;   
    private InputAction _moveAction;
    #endregion

    #region public Event Action Variables
    public event Action<Vector2> OnMove;
    #endregion

    #region Action Variables
    private Action<InputAction.CallbackContext> movePerformed;
    private Action<InputAction.CallbackContext> moveCancelled; 
    #endregion

    void OnEnable()
    {
        _inputActions = new();

        //Player
        InitializePlayerActions();
        InitializePlayerEvents();


        SubscribePlayerActions();
    }
    void OnDisable()
    {
        UnSubscribePlayerActions();
    }
    private void InitializePlayerActions()
    {
        _moveAction = _inputActions.Player.Move;
    }
    private void InitializePlayerEvents()
    {
        movePerformed = ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        moveCancelled = ctx => OnMove?.Invoke(Vector2.zero);
    }

    #region Subscribe/UnSubscribe
    public void SubscribePlayerActions()
    {
        _moveAction.performed += movePerformed;
        _moveAction.canceled += moveCancelled;
        
    }
    public void UnSubscribePlayerActions()
    {
        _moveAction.performed -= movePerformed;
        _moveAction.canceled -= moveCancelled;
    }
    #endregion

    #region Enable/Disable Actions
    #region Move action
    public void EnableMoveAction()
    {
        _moveAction.Enable();
    }
    public void DisableMoveAction()
    {
        _moveAction.Disable();
    }
    #endregion
    #endregion

}

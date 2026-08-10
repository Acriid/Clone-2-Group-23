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
    private InputAction _clickAction;
    private InputAction _spaceAction;
    #endregion

    #region public Event Action Variables
    public event Action<Vector2> OnMove;
    public event Action OnClick;
    public event Action OnSpace;
    #endregion

    #region Action Variables
    private Action<InputAction.CallbackContext> movePerformed;
    private Action<InputAction.CallbackContext> moveCancelled; 

    private Action<InputAction.CallbackContext> clickPerformed;

    private Action<InputAction.CallbackContext> spacePerformed;
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

        _clickAction = _inputActions.UI.Click;

        _spaceAction = _inputActions.Player.Jump;
    }
    private void InitializePlayerEvents()
    {
        movePerformed = ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        moveCancelled = ctx => OnMove?.Invoke(Vector2.zero);

        clickPerformed = ctx => OnClick?.Invoke();
    }

    #region Subscribe/UnSubscribe
    public void SubscribePlayerActions()
    {
        _moveAction.performed += movePerformed;
        _moveAction.canceled += moveCancelled;
        

        _clickAction.performed += clickPerformed;
    }
    public void UnSubscribePlayerActions()
    {
        _moveAction.performed -= movePerformed;
        _moveAction.canceled -= moveCancelled;

        _clickAction.performed -= clickPerformed;
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
    #region Click action
    public void EnableClickAction()
    {
        _clickAction.Enable();
    }
    public void DisableClickAction()
    {
        _clickAction.Disable();
    }
    #endregion
    #endregion

}

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
    private InputAction _leftClickAction;
    private InputAction _rightClickAction;
    private InputAction _spaceAction;
    private InputAction _interactAction;
    #endregion

    #region public Event Action Variables
    public event Action<Vector2> OnMove;
    public event Action OnLeftClick;
    public event Action OnRightClick;
    public event Action OnSpace;
    public event Action OnInteract;
    #endregion

    #region Action Variables
    private Action<InputAction.CallbackContext> movePerformed;
    private Action<InputAction.CallbackContext> moveCancelled; 

    private Action<InputAction.CallbackContext> leftClickPerformed;
    private Action<InputAction.CallbackContext> rightClickPerformed;

    private Action<InputAction.CallbackContext> spacePerformed;

    private Action<InputAction.CallbackContext> interactPerformed;
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

        _leftClickAction = _inputActions.UI.Click;
        _rightClickAction = _inputActions.UI.RightClick;

        _spaceAction = _inputActions.Player.Jump;

        _interactAction = _inputActions.Player.Interact;
    }
    private void InitializePlayerEvents()
    {
        movePerformed = ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        moveCancelled = ctx => OnMove?.Invoke(Vector2.zero);

        leftClickPerformed = ctx => OnLeftClick?.Invoke();
        rightClickPerformed = ctx => OnRightClick?.Invoke();

        spacePerformed = ctx => OnSpace?.Invoke();

        interactPerformed = ctx => OnInteract?.Invoke();
    }

    #region Subscribe/UnSubscribe
    public void SubscribePlayerActions()
    {
        _moveAction.performed += movePerformed;
        _moveAction.canceled += moveCancelled;
        

        _leftClickAction.performed += leftClickPerformed;
        _rightClickAction.performed += rightClickPerformed;

        _spaceAction.performed += spacePerformed;

        _interactAction.performed += interactPerformed;
    }
    public void UnSubscribePlayerActions()
    {
        _moveAction.performed -= movePerformed;
        _moveAction.canceled -= moveCancelled;

        _leftClickAction.performed -= leftClickPerformed;
        _rightClickAction.performed -= rightClickPerformed;

        _spaceAction.performed -= spacePerformed;

        _interactAction.performed -= interactPerformed;
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
        _leftClickAction.Enable();
    }
    public void DisableClickAction()
    {
        _leftClickAction.Disable();
    }
    public void EnableRightClickAction()
    {
        _rightClickAction.Enable();
    }
    public void DisableRightClickAction()
    {
        _rightClickAction.Disable();
    }
    #endregion
    #region Space action
    public void EnableSpaceAction()
    {
        _spaceAction.Enable();
    }
    public void DisableSpaceAction()
    {
        _spaceAction.Disable();
    }
    #endregion
    #region Interact action
    public void EnableInteractAction()
    {
        _interactAction.Enable();
    }
    public void DisableInteractAction()
    {
        _interactAction.Disable();
    }
    #endregion
    #endregion

}

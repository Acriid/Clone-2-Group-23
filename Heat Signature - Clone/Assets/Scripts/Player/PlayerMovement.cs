using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _moveInput;

    //Variables that need to be set in unity
    [SerializeField] private Rigidbody2D _playerRigidBody = null;
    [SerializeField] private float _playerSpeed = 10f;
    [SerializeField] private InputReader _inputReader = null;


    bool _slowMotion = false;
    private void OnEnable()
    {
        if(_inputReader != null)
        EnablePlayerMovement();
    }

    private void OnDisable()
    {
        if(_inputReader != null)
        DisablePlayerMovement();
    }
    void FixedUpdate()
    {
        MovePlayer(_moveInput);
    }

    //Function that moves Player 
    private void ReadMoveInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }

    private void MovePlayer(Vector2 moveInput)
    {
        //Input reading can be done here for animations


        if(_playerRigidBody == null) return;

        if(_slowMotion)
        {
            _playerRigidBody.AddForce(_playerSpeed * moveInput); 
        }
        else
        {
            _playerRigidBody.linearVelocity = _playerSpeed * moveInput;
        }
    }

    private void EnablePlayerMovement()
    {
        _inputReader.EnableMoveAction();

        _inputReader.OnMove += ReadMoveInput;
    }

    private void DisablePlayerMovement()
    {
        _inputReader.DisableMoveAction();

        _inputReader.OnMove -= ReadMoveInput;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class MeleeWeapon : Item
{
    //Coroutine wait times
    [Header("Coroutine Wait Times")]
    [SerializeField] private float _findTargetWaitTime = 0.2f;
    [SerializeField] private float _throwRoutineWaitTime = 0.2f;


    //Collision
    [Header("Collision Components")]
    [SerializeField] private Rigidbody2D _weaponRigidBody;
    [SerializeField] private Collider2D _weaponCollider;
    [SerializeField] private LayerMask _targetLayerMask;

    //Line
    [Header("Feedback Line")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LayerMask _bounceLayerMask;

    //Internal Cooldown Clock
    private float _itemCooldown = 0f;

    //Find target Coroutine variables
    private Coroutine _targetRoutine = null;
    private GameObject _target = null;


    //Find target Coroutine variables
    private Coroutine _throwRoutine = null;
    private Vector2 _throwLine = new();
    //Change later
    private float _maxLineDistance = 8f;

    //Main camera Cache
    private Camera _mainCamera = null;

    const float ATTACKDETEACTIONWIDTH = 1f;
    const float THROWFORCE = 2f;

    void Awake()
    {
        _mainCamera = Camera.main;
    }





    /// <summary>
    /// UseItem is called twice, once to find a target, twice to act upon target found
    /// </summary>
    public override void UseItem()
    {
        if(_itemCooldown < _itemSO.ItemCooldown) return;


        //First use check
        if(_targetRoutine == null)
        {
            _targetRoutine = StartCoroutine(FindTarget());
            return;
        }

        //No target found, Stops coroutine and resets UseItem back to first use
        if(_target == null)
        {
            StopCoroutine(_targetRoutine);
            _targetRoutine = null;

            //Safeguard for if a target was found during the reset
            _target = null;

            return;
        }

        //TODO - Move character towards target over time and test if weapon hit.

        //Reset variables
        if(_targetRoutine != null)
        {
            StopCoroutine(_targetRoutine);
            _targetRoutine = null;
        }
        if(_target != null)
        {
            _target = null;
        }


        //Start Cooldown
        StartCoroutine(CooldownClock());

    }
    /// <summary>
    /// ThrowItem is called twice, once to get the where you throw, twice to throw
    /// </summary>
    public override void ThrowItem()
    {
        //First use check
        if(_throwRoutine == null)
        {
            _throwRoutine = StartCoroutine(ThrowItemPath());
            return;
        }

        //Second use Throw
        if(_throwLine != Vector2.zero)
        {
            ApplyForce(_throwLine);
            _throwLine = Vector2.zero;
        }

        //Reset 
        if(_throwRoutine != null)
        {
            StopCoroutine(_throwRoutine);
            _throwRoutine = null;
        }

        if(_lineRenderer.positionCount != 0)
        {
            _lineRenderer.positionCount = 0;
        }
        

    }
    public override void DropItem()
    {
        //Remove item from inventory and put item on the ground
        base.DropItem();
    }
    public override void PickUpItem()
    {
        //Remove item from ground and put item into inventory
        base.PickUpItem();
    }


    /// <summary>
    /// Sends a BoxCast in the direction of the mouse while giving the first target hit to _target
    /// </summary>
    private IEnumerator FindTarget()
    {
        WaitForSeconds waitTime = new(_findTargetWaitTime);
        Vector2 boxSize = new(ATTACKDETEACTIONWIDTH,ATTACKDETEACTIONWIDTH);

        while(true)
        {
            Vector2 origin = transform.position;
            Vector2 mousePosition = Pointer.current.position.ReadValue();

            //Change Mouse Position To World Position

            Vector2 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);

            Vector2 direction = (mouseWorldPosition - origin).normalized;
            float angle = Vector2.SignedAngle(origin,mouseWorldPosition);
            
            //Raycast

            RaycastHit2D hit = Physics2D.BoxCast(origin,boxSize,angle,direction,_itemSO.ItemRange,_targetLayerMask);

            _target = hit.collider.gameObject;

            yield return waitTime;
        }
    }
    private IEnumerator ThrowItemPath()
    {
        _lineRenderer.positionCount = 2;

        Vector3[] pointPositions = new Vector3[2];

        while(true)
        {
            //Get line variables
            Vector2 origin = transform.position;
            Vector2 mousePosition = Pointer.current.position.ReadValue();
            Vector2 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
            //Get throw direction
            _throwLine = mouseWorldPosition-origin;

            float mouseDistance = Vector2.Distance(origin, mouseWorldPosition);

            if(mouseDistance > _maxLineDistance)
            {
                mouseWorldPosition = origin + (mouseWorldPosition - origin).normalized * _maxLineDistance;
            }


            //Show thrown line
            pointPositions[0] = origin;
            pointPositions[1] = mouseWorldPosition;

            _lineRenderer.SetPositions(pointPositions);

            yield return null;
        }
    }
    private IEnumerator CooldownClock()
    {
        //Start item Cooldown
        _itemCooldown = 0f;
        while(_itemCooldown < _itemSO.ItemCooldown)
        {
            //TODO- Add variable to change cooldown time when in slipstream or shadow map.

            _itemCooldown += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Throws the object in the direction given
    /// </summary>
    /// <param name="direction">Direction to throw object</param>
    public void ApplyForce(Vector2 direction)
    {
        _weaponRigidBody.AddForce(direction * THROWFORCE,ForceMode2D.Impulse);
    }
}

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

    //Internal Cooldown Clock
    private float _itemCooldown = 0f;

    //Find target Coroutine variables
    private Coroutine _targetRoutine = null;
    private GameObject _target = null;


    //Find target Coroutine variables
    private Coroutine _throwRoutine = null;
    private List<Vector2> _throwLine = new();


    const float ATTACKDETEACTIONWIDTH = 2f;

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

        Debug.Log(_target.name);
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

        //Safeguard
        if(_throwLine.Count == 0)
        {
            return;
        }

        Vector2 throwDirection = (_throwLine[0] - (Vector2)transform.position).normalized;




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


    private IEnumerator FindTarget()
    {
        WaitForSeconds waitTime = new(_findTargetWaitTime);
        Vector2 boxSize = new(ATTACKDETEACTIONWIDTH,ATTACKDETEACTIONWIDTH);

        while(true)
        {
            Vector2 origin = transform.position;
            Vector2 mousePosition = Pointer.current.position.ReadValue();
            Vector2 direction = (mousePosition - origin).normalized;
            

            RaycastHit2D hit = Physics2D.BoxCast(origin,boxSize,0f,direction,_itemSO.ItemRange,_targetLayerMask);

            if(hit.collider != null)
            {
                _target = hit.collider.gameObject;
            }
            else
            {
                Debug.Log("Nope");
            }

            yield return waitTime;
        }
    }
    private IEnumerator ThrowItemPath()
    {
        WaitForSeconds waitTime = new(_throwRoutineWaitTime);
        while(true)
        {
            //TODO - Calculate the path start,end and any bounces in between
            yield return waitTime;
        }
    }
}

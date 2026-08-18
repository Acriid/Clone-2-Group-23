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
    private Coroutine _linearDampeningRoutine = null;
    private Vector2 _throwLine = new();


    private float _dampening = 1f;


    //Main camera Cache
    private Camera _mainCamera = null;
    private Pointer _mainPointer = null;

    //Constants    
    const float ATTACKDETEACTIONWIDTH = 1f;
    const float THROWFORCE = 2f;

    void Awake()
    {
        //Cache main camera
        _mainCamera = Camera.main;

        _mainPointer = Pointer.current;

        //starts off cooldown
        _itemCooldown = _itemSO.ItemCooldown + 1f;
    }


    void OnEnable()
    {
        if(_timeManager != null)
        _timeManager.OnEnemyTimeChange += ChangeTimeVariable;
    }
    void OnDisable()
    {
        if(_timeManager != null)
        _timeManager.OnEnemyTimeChange -= ChangeTimeVariable;
    }
    private void ChangeTimeVariable(float newValue)
    {
        if(!Mathf.Approximately(_timeVariable,1f))
        {
            _velocity /= _timeVariable;
        }

        _timeVariable = newValue;
        _velocity *= _timeVariable;

        if(_weaponRigidBody != null)
        _weaponRigidBody.linearVelocity = _velocity;
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

        StartCoroutine(DashToTarget(_target));

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
        if(_lineRenderer.positionCount != 0)
        {
            ResetLineRenderer();
        }


        //Start Cooldown
        StartCoroutine(CooldownClock());

    }

    public void UseItem(GameObject target)
    {
        if(_itemCooldown < _itemSO.ItemCooldown) return;

        StartCoroutine(DashToTarget(target));

        StartCoroutine(CooldownClock());
    }
    /// <summary>
    /// ThrowItem is called twice, once to get the where you throw, twice to throw
    /// </summary>
    public override void ThrowItem()
    {
        //TODO- Change to be time dependent

        //First use check
        if(_throwRoutine == null)
        {
            _throwRoutine = StartCoroutine(ThrowItemPath());
            return;
        }

        //Second use Throw
        if(_throwLine != Vector2.zero)
        {
            _weaponRigidBody.bodyType = RigidbodyType2D.Dynamic;
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
            ResetLineRenderer();
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
        _weaponRigidBody.bodyType = RigidbodyType2D.Kinematic;
    }


    /// <summary>
    /// Sends a BoxCast in the direction of the mouse while giving the first target hit to _target
    /// </summary>
    private IEnumerator FindTarget()
    {
        Debug.Log("WOW");
        WaitForSeconds waitTime = new(_findTargetWaitTime);
        Vector2 boxSize = new(ATTACKDETEACTIONWIDTH,ATTACKDETEACTIONWIDTH);

        while(true)
        {
            Vector2 origin = transform.position;
            Vector2 mousePosition = _mainPointer.position.ReadValue();

            //Change Mouse Position To World Position

            Vector2 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);


            //Max out distance.
            
            float mouseDistance = Vector2.Distance(origin, mouseWorldPosition);

            if(mouseDistance > _itemSO.ItemRange)
            {
                mouseWorldPosition = origin + (mouseWorldPosition - origin).normalized * _itemSO.ItemRange;
            }

            Vector2 direction = (mouseWorldPosition - origin).normalized;
            float angle = Vector2.SignedAngle(origin,mouseWorldPosition);
            
            //Raycast

            RaycastHit2D hit = Physics2D.BoxCast(origin,boxSize,angle,direction,_itemSO.ItemRange,_targetLayerMask);

            if(hit.collider != null)
            {
                _target = hit.collider.gameObject;  
            }

            yield return waitTime;
        }
    }
    //TODO - Change to be time dependent
    private IEnumerator ThrowItemPath()
    {
        _lineRenderer.positionCount = 2;

        Vector3[] pointPositions = new Vector3[2];

        while(true)
        {
            //Get line variables
            Vector2 origin = transform.position;
            Vector2 mousePosition = _mainPointer.position.ReadValue();
            Vector2 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
            //Get throw direction
            

            float mouseDistance = Vector2.Distance(origin, mouseWorldPosition);

            if(mouseDistance > _itemSO.ItemRange )
            {
                mouseWorldPosition = origin + (mouseWorldPosition - origin).normalized * _itemSO.ItemRange;
            }

            _throwLine = mouseWorldPosition-origin;
            _throwLine = _itemSO.ItemRange  * _throwLine.normalized;

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
            if(_enemyItem)
            {
                _itemCooldown += Time.deltaTime * _timeVariable;
            }
            else
            {
                _itemCooldown += Time.deltaTime;
            }
            yield return null;
        }
    }


    //Variable to go over unity's auto Physics.
    private Vector2 _velocity = Vector2.zero;
    /// <summary>
    /// Throws the object in the direction given
    /// </summary>
    /// <param name="direction">Direction to throw object</param>
    public void ApplyForce(Vector2 direction)
    {
        _velocity = _timeVariable * THROWFORCE * direction;
        _weaponRigidBody.linearVelocity = _velocity;
        
        if(_linearDampeningRoutine == null)
        {
            _linearDampeningRoutine = StartCoroutine(ApplyLinearDamping());
        }
        else
        {
            StopCoroutine(_linearDampeningRoutine);
            _linearDampeningRoutine = StartCoroutine(ApplyLinearDamping());
        }
    }

    private IEnumerator ApplyLinearDamping()
    {
        while (_weaponRigidBody.linearVelocity.sqrMagnitude > (0.0001f * _timeVariable))
        {
            yield return new WaitForFixedUpdate();
            _velocity *= 1.0f / (1.0f + Time.fixedDeltaTime * _dampening * _timeVariable);
            _weaponRigidBody.linearVelocity = _velocity;
        }
        _linearDampeningRoutine = null;
        _weaponRigidBody.bodyType = RigidbodyType2D.Kinematic;
    }

    private void ResetLineRenderer()
    {
        _lineRenderer.positionCount = 0;
    }

    public float GetMeleeRange()
    {
        return _itemSO.ItemRange;
    }

    private IEnumerator DashToTarget(GameObject target)
    {
        Vector2 startPosition = _parentObject.transform.position;
        Vector2 movePosition = target.transform.position;
        float moveTime = 0.15f;
        float elapsedTime = 0f;
        while(elapsedTime < moveTime)
        {
            if(!_enemyItem)
            {
                elapsedTime += Time.deltaTime;
            }
            else
            {
                elapsedTime += Time.deltaTime * _timeVariable;
            }


            float t = Mathf.Clamp01(elapsedTime / moveTime);

            _parentObject.transform.position = Vector2.Lerp(
                startPosition,
                movePosition,
                t);

            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(_parentObject.CompareTag("Enemy"))
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                if(collision.collider.TryGetComponent<PlayerHealth>(out PlayerHealth component))
                {
                    component.TakeDamage();
                }
            }
            
        }
        else
        {
            if(collision.collider.CompareTag("Enemy"))
            {
                //Todo kill enemy
                if(collision.collider.TryGetComponent<Enemy>(out Enemy component))
                {
                    component.TakeDamage(1f);
                }
            }   
        }


        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 wallNormal = collision.GetContact(0).normal;

            _velocity = Vector2.Reflect(_velocity, wallNormal);

            _weaponRigidBody.linearVelocity = _velocity;
        }

    }
}

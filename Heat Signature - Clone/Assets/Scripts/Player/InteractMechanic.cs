using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class InteractMechanic : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float _detectionRadius = 3f;
    [SerializeField] private LayerMask _interactionLayer;

    private List<Item> _itemsInRange = new();
    private Item _targetItem;
    private CircleCollider2D _detectionTrigger;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        UpdateTargetItem(transform);
    }

    private void Initialize()
    {
        _detectionTrigger = GetComponent<CircleCollider2D>();
        _detectionTrigger.radius = _detectionRadius;
        _detectionTrigger.isTrigger = true;
    }

    private void UpdateTargetItem(Transform player)
    {

        _itemsInRange.RemoveAll(item => item == null);
        
        _targetItem = null;

        if (_itemsInRange.Count == 0) return;

        float bestScore = float.MaxValue;

        foreach (Item interaction in _itemsInRange)
        {
            float distance = Vector3.Distance(player.position, interaction.transform.position);

            if (distance < bestScore)
            {
                bestScore = distance;
                _targetItem = interaction;
            }
        }
    }


    public bool Interact(GameObject player)
    {
        if (_targetItem == null) return false;


        _targetItem.PickUpItem();
        return true;
    }


    public Item GetTargetItem()
    {
        return _targetItem;
    }


    public bool HasItem()
    {
        return _targetItem != null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _interactionLayer) == 0)
            return;

        Item interaction = collision.GetComponent<Item>();

        if (interaction != null && !_itemsInRange.Contains(interaction))
            _itemsInRange.Add(interaction);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Item>(out var interaction))
        {
            _itemsInRange.Remove(interaction);
            if (_targetItem == interaction)
                _targetItem = null;
        }
    }
}

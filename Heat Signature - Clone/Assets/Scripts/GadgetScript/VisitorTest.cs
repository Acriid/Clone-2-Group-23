using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class VisitorTest : Item
{

    [Header("Visitor Settings")]
    [SerializeField] private float visitDuration = 2f;
    [SerializeField] private LayerMask _floorMask;
    [SerializeField] private GameObject _playerRef;

    private Camera mainCamera;

    private Vector3 originalPosition;

    private bool isVisiting = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public override void UseItem()
    {
        if(isVisiting) return;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera was not found.");
            return;
        }

        // Get mouse position
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // Convert mouse position from screen space to world space
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(
                mousePosition.x,
                mousePosition.y,
                -mainCamera.transform.position.z
            )
        );

        Vector2 raycastDirection = (mainCamera.transform.position - worldPosition).normalized;

        Debug.Log(worldPosition);

        // Check what the mouse clicked
        RaycastHit2D hit = Physics2D.Raycast(
            mainCamera.transform.position,
            raycastDirection,
            Mathf.Infinity,
            _floorMask
        );

        if (!hit.collider.CompareTag("Ground"))
        {
            Debug.Log("Did not hit");
            return;
        }

        Debug.Log(hit.collider.name);
        StartCoroutine(VisitDestination(worldPosition));
    }

    private IEnumerator VisitDestination(Vector2 destination)
    {
        isVisiting = true;

        // Remember where the player originally was
        originalPosition = _playerRef.transform.position;

        // Teleport to selected destination
        _playerRef.transform.position = destination;

        // Stay there for 2 seconds
        yield return new WaitForSeconds(visitDuration);

        // Return to original position
        _playerRef.transform.position = originalPosition;

        Debug.Log("Visitor finished. Returned to original position.");

        isVisiting = false;
    }
}
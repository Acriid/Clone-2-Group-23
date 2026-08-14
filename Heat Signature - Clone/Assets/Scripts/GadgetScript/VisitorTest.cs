using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class VisitorTest : MonoBehaviour
{
    [Header("Teleport Destinations")]
    [SerializeField] private Transform[] teleportDestinations;

    [Header("Visitor Settings")]
    [SerializeField] private float visitDuration = 2f;

    private Camera mainCamera;

    private Vector3 originalPosition;

    private bool isVisiting = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Left mouse click
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            !isVisiting)
        {
            CheckForDestination();
        }
    }

    private void CheckForDestination()
    {
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

        // Check what the mouse clicked
        RaycastHit2D hit = Physics2D.Raycast(
            worldPosition,
            Vector2.zero
        );

        if (hit.collider == null)
        {
            return;
        }

        Transform clickedObject = hit.collider.transform;

        // Check if the clicked object is one of our destinations
        for (int i = 0; i < teleportDestinations.Length; i++)
        {
            if (teleportDestinations[i] == clickedObject)
            {
                StartCoroutine(VisitDestination(clickedObject));
                return;
            }
        }
    }

    private IEnumerator VisitDestination(Transform destination)
    {
        isVisiting = true;

        // Remember where the player originally was
        originalPosition = transform.position;

        // Teleport to selected destination
        transform.position = destination.position;

        Debug.Log("Visitor activated. Teleported to " + destination.name);

        // Stay there for 2 seconds
        yield return new WaitForSeconds(visitDuration);

        // Return to original position
        transform.position = originalPosition;

        Debug.Log("Visitor finished. Returned to original position.");

        isVisiting = false;
    }
}
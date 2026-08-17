using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class VisitorTest : MonoBehaviour
{
    [Header("Visitor Settings")]
    [SerializeField] private float visitDuration = 2f;

    [Header("Player")]
    [SerializeField] private Transform player;

    private Camera mainCamera;

    private Vector3 originalPosition;

    private bool visitorActive = false;
    private bool isVisiting = false;

    private void Start()
    {
        mainCamera = Camera.main;

        if (player == null)
        {
            player = transform;
        }
    }

    private void Update()
    {
        if (!visitorActive || isVisiting)
        {
            return;
        }

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckForDestination();
        }
    }

    public void UseVisitor()
    {
        if (isVisiting)
        {
            return;
        }

        visitorActive = true;

        Debug.Log("Visitor activated. Click a destination.");
    }

    private void CheckForDestination()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera was not found.");
            return;
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(
                mousePosition.x,
                mousePosition.y,
                -mainCamera.transform.position.z
            )
        );

        Collider2D hit = Physics2D.OverlapPoint(worldPosition);

        if (hit == null)
        {
            return;
        }

        if (!hit.CompareTag("VisitorDestination"))
        {
            return;
        }

        StartCoroutine(VisitDestination(hit.transform));
    }

    private IEnumerator VisitDestination(Transform destination)
    {
        isVisiting = true;
        visitorActive = false;

        originalPosition = player.position;

        player.position = destination.position;

        Debug.Log("Visitor activated. Teleported to " + destination.name);

        yield return new WaitForSeconds(visitDuration);

        player.position = originalPosition;

        Debug.Log("Visitor finished. Returned to original position.");

        isVisiting = false;
    }
}
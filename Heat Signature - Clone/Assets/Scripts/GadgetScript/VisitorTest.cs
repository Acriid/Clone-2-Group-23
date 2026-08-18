using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class VisitorTest : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Visitor")]
    [SerializeField] private float visitorDuration = 2f;

    [Header("Sidewinder")]
    [SerializeField] private float sidewinderRange = 20f;

    [Header("Swapper")]
    [SerializeField] private float swapperRange = 20f;

    [Header("Slipstream")]
    [SerializeField] private float slipstreamDuration = 10f;

    private Camera mainCamera;

    private Vector3 originalPosition;

    private bool visitorActive = false;
    private bool sidewinderActive = false;
    private bool swapperActive = false;

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
        if (visitorActive && !isVisiting)
        {
            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame)
            {
                CheckVisitorDestination();
            }
        }

        if (sidewinderActive)
        {
            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame)
            {
                CheckSidewinderDestination();
            }
        }

        if (swapperActive)
        {
            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame)
            {
                CheckSwapperTarget();
            }
        }
    }

    public void UseVisitor()
    {
        if (isVisiting)
        {
            return;
        }

        visitorActive = true;
        sidewinderActive = false;
        swapperActive = false;

        Debug.Log("Visitor activated. Click a destination.");
    }

    public void UseSidewinder()
    {
        if (isVisiting)
        {
            return;
        }

        visitorActive = false;
        sidewinderActive = true;
        swapperActive = false;

        Debug.Log("Sidewinder activated. Click a destination.");
    }

    public void UseSwapper()
    {
        if (isVisiting)
        {
            return;
        }

        visitorActive = false;
        sidewinderActive = false;
        swapperActive = true;

        Debug.Log("Swapper activated. Click a target.");
    }

    public void UseSlipstream()
    {
        StartCoroutine(ActivateSlipstream());
    }

    private void CheckVisitorDestination()
    {
        Collider2D hit = GetClickedCollider();

        if (hit == null)
        {
            return;
        }

        if (!hit.CompareTag("VisitorDestination"))
        {
            Debug.Log("This is not a Visitor destination.");
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

        Debug.Log("Visitor teleported to " + destination.name);

        yield return new WaitForSeconds(visitorDuration);

        player.position = originalPosition;

        Debug.Log("Visitor returned to original position.");

        isVisiting = false;
    }

    private void CheckSidewinderDestination()
    {
        Collider2D hit = GetClickedCollider();

        if (hit == null)
        {
            return;
        }

        if (!hit.CompareTag("SidewinderDestination"))
        {
            Debug.Log("This is not a Sidewinder destination.");
            return;
        }

        Vector2 playerPosition = player.position;
        Vector2 destinationPosition = hit.transform.position;

        float distance = Vector2.Distance(
            playerPosition,
            destinationPosition
        );

        if (distance > sidewinderRange)
        {
            Debug.Log("Sidewinder destination is too far away.");
            return;
        }

        RaycastHit2D obstacle = Physics2D.Linecast(
            playerPosition,
            destinationPosition
        );

        if (obstacle.collider != null &&
            obstacle.collider.transform != hit.transform &&
            obstacle.collider.transform != player)
        {
            Debug.Log("Sidewinder path is blocked.");
            return;
        }

        player.position = destinationPosition;

        sidewinderActive = false;

        Debug.Log("Sidewinder teleported the player.");
    }

    private void CheckSwapperTarget()
    {
        Collider2D hit = GetClickedCollider();

        if (hit == null)
        {
            return;
        }

        if (!hit.CompareTag("SwapperTarget"))
        {
            Debug.Log("This is not a Swapper target.");
            return;
        }

        float distance = Vector2.Distance(
            player.position,
            hit.transform.position
        );

        if (distance > swapperRange)
        {
            Debug.Log("Swapper target is too far away.");
            return;
        }

        Vector3 targetPosition = hit.transform.position;

        hit.transform.position = player.position;

        player.position = targetPosition;

        swapperActive = false;

        Debug.Log("Swapper switched positions.");
    }

    private IEnumerator ActivateSlipstream()
    {
        Debug.Log("Slipstream activated.");

        Time.timeScale = 0.1f;

        yield return new WaitForSecondsRealtime(slipstreamDuration);

        Time.timeScale = 1f;

        Debug.Log("Slipstream finished.");
    }

    private Collider2D GetClickedCollider()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera was not found.");
            return null;
        }

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Vector3 worldPosition =
            mainCamera.ScreenToWorldPoint(
                new Vector3(
                    mousePosition.x,
                    mousePosition.y,
                    -mainCamera.transform.position.z
                )
            );

        return Physics2D.OverlapPoint(worldPosition);
    }
}
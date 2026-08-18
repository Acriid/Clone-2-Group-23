using UnityEngine;
using UnityEngine.InputSystem;

public class SidewinderTest : Item
{
    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Sidewinder")]
    [SerializeField] private float sidewinderRange = 20f;

    private Camera mainCamera;

    private bool sidewinderActive = false;

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
        if (!sidewinderActive)
        {
            return;
        }

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckSidewinderDestination();
        }
    }

    public override void UseItem()
    {
        sidewinderActive = true;

        Debug.Log(
            "Sidewinder activated. Click a destination."
        );
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
            Debug.Log(
                "This is not a Sidewinder destination."
            );

            return;
        }

        Vector2 playerPosition = player.position;

        Vector2 destinationPosition =
            hit.transform.position;

        float distance = Vector2.Distance(
            playerPosition,
            destinationPosition
        );

        if (distance > sidewinderRange)
        {
            Debug.Log(
                "Sidewinder destination is too far away."
            );

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
            Debug.Log(
                "Sidewinder path is blocked."
            );

            return;
        }

        player.position = destinationPosition;

        sidewinderActive = false;

        Debug.Log(
            "Sidewinder teleported the player."
        );
    }

    private Collider2D GetClickedCollider()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            Debug.LogWarning(
                "Main Camera was not found."
            );

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
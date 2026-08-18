using UnityEngine;
using UnityEngine.InputSystem;

public class SwapperTest : Item
{
    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Swapper")]
    [SerializeField] private float swapperRange = 20f;

    private Camera mainCamera;

    private bool swapperActive = false;

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
        if (!swapperActive)
        {
            return;
        }

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckSwapperTarget();
        }
    }

    public override void UseItem()
    {
        swapperActive = true;

        Debug.Log(
            "Swapper activated. Click a target."
        );
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
            Debug.Log(
                "This is not a Swapper target."
            );

            return;
        }

        float distance = Vector2.Distance(
            player.position,
            hit.transform.position
        );

        if (distance > swapperRange)
        {
            Debug.Log(
                "Swapper target is too far away."
            );

            return;
        }

        Vector3 targetPosition =
            hit.transform.position;

        hit.transform.position =
            player.position;

        player.position =
            targetPosition;

        swapperActive = false;

        Debug.Log(
            "Swapper switched positions."
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
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class VisitorTest : Item
{
    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Visitor")]
    [SerializeField] private float visitorDuration = 2f;

    private Camera mainCamera;

    private Vector3 originalPosition;

    private bool visitorActive = false;
    private bool isVisiting = false;

    private void Start()
    {
        mainCamera = Camera.main;

        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }
    void OnEnable()
    {
        if(_timeManager != null)
        {
            _timeManager.OnEnemyTimeChange += OnTimeChange;
        }
    }
    void OnDisable()
    {
        if(_timeManager != null)
        {
            _timeManager.OnEnemyTimeChange -= OnTimeChange;
        }        
    }
    private void OnTimeChange(float newValue)
    {
        _timeVariable = newValue;
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
            CheckVisitorDestination();
        }
    }

    public override void UseItem()
    {
        if (isVisiting)
        {
            return;
        }

        visitorActive = true;

        Debug.Log("Visitor activated. Click a destination.");
    }

    private void CheckVisitorDestination()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            Debug.LogWarning("Visitor: Main Camera not found.");
            return;
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

        Collider2D hit =
            Physics2D.OverlapPoint(worldPosition);

        if (hit == null)
        {
            Debug.Log("Visitor: No 2D collider clicked.");
            return;
        }

        Debug.Log(
            "Visitor clicked: " +
            hit.gameObject.name +
            " | Tag: " +
            hit.gameObject.tag
        );

        if (!hit.CompareTag("VisitorDestination"))
        {
            Debug.Log(
                "Visitor: " +
                hit.gameObject.name +
                " is not a VisitorDestination."
            );

            return;
        }

        Debug.Log("Visitor: Valid destination found!");

        StartCoroutine(
            VisitDestination(hit.transform)
        );
    }

    private IEnumerator VisitDestination(
        Transform destination
    )
    {
        isVisiting = true;
        visitorActive = false;

        if (player == null)
        {
            Debug.LogWarning(
                "Visitor: Player is not assigned."
            );

            isVisiting = false;
            yield break;
        }

        originalPosition = player.position;

        player.position = destination.position;

        Debug.Log(
            "Visitor teleported to " +
            destination.name
        );

        float elapsedTime = 0f;
        while(elapsedTime < visitorDuration)
        {
            elapsedTime += Time.deltaTime * _timeVariable;
            yield return null;
        }


        player.position = originalPosition;

        Debug.Log(
            "Visitor returned to original position."
        );

        isVisiting = false;
    }
}
using UnityEngine;

public class CameraRoomFollow : MonoBehaviour
{
    //Public Variables
    public Transform player;
    public float roomWidth = 16f;
    public float roomHeight = 9f;
    public float moveSpeed = 8f;

    [Header("Heat Signature style angle")]
    public float cameraHeight = 12f;      // how high above the room the camera sits
    public float tiltAngle = 40f;         // 90 = straight down, lower = more angled/perspective
    public float forwardOffset = 2f;      // pulls camera back so it's not directly overhead

    
    //Private Variables 
    private Vector3 targetPosition;

    void Start()
    {
        transform.rotation = Quaternion.Euler(tiltAngle, 0f, 0f);
        targetPosition = transform.position;
    }

    //Adjusts Camera 
    void LateUpdate()
    {
        int roomX = Mathf.RoundToInt(player.position.x / roomWidth);
        int roomY = Mathf.RoundToInt(player.position.y / roomHeight);

        Vector3 roomCenter = new Vector3(roomX * roomWidth, roomY * roomHeight, 0f);

        targetPosition = roomCenter
            + Vector3.up * cameraHeight
            + Vector3.back * forwardOffset;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime);
    }
}
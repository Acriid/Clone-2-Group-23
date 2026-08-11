using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Public Variables
    public Transform Player;

    //Adjusts Camera 
    void LateUpdate()
    {
        Vector3 cameraPosition = Player.position;
        cameraPosition.z = -10f;
        transform.position = cameraPosition;
    }
}
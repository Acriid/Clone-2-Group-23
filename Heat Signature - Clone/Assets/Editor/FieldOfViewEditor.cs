using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    // Allows us to edit the field of view parameters without editing the script
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;

        // Normal = Vector3.forward so the circle lies on the XY plane, not XZ (2D)
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.up, 360, fov.radius);

        // Actual viewing angle, for left and right edges of the cone
        Vector3 viewAngleLeft = DirectionFromAngle(fov.transform.eulerAngles.z, -fov.angle / 2);
        Vector3 viewAngleRight = DirectionFromAngle(fov.transform.eulerAngles.z, fov.angle / 2);

        // Draw them in as Gizmos
        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleLeft * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleRight * fov.radius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.pink;
            Handles.DrawLine(fov.transform.position,fov.playerRef.transform.position);
            
        }
    }

    private Vector3 DirectionFromAngle(float eulerZ, float angleInDegrees)
    {
        angleInDegrees += eulerZ;

        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}
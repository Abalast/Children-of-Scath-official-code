using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (EnemyFieldOfView))]
public class FieldOfViewEditor : Editor
{
    
    private void OnSceneGUI()
    {
        EnemyFieldOfView fov = (EnemyFieldOfView)target;
        Handles.color = UnityEngine.Color.white;
        Handles.DrawWireArc(fov.transform.position + fov.eyeOffset, UnityEngine.Vector3.forward, UnityEngine.Vector3.right, 360, fov.viewRadius);
        Vector3 viewAngleA = fov.DirFormAngle(-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFormAngle(fov.viewAngle / 2, false);

        Handles.DrawLine(fov.transform.position + fov.eyeOffset, (fov.transform.position + fov.eyeOffset + viewAngleA * fov.viewRadius));
        Handles.DrawLine(fov.transform.position + fov.eyeOffset, (fov.transform.position + fov.eyeOffset + viewAngleB * fov.viewRadius));
    }
    
}
 

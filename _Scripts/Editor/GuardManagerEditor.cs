using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//Mina Pechaux- creating a basic fov system

[CustomEditor(typeof(GuardManager))]
public class GuardManagerEditor : Editor
{
    private void OnSceneGUI()
    {
        GuardManager guard = (GuardManager)target;

        Color c = Color.green;
        if (guard.alertStage == AlertStage.Intrigued)
            c = Color.Lerp(Color.green, Color.red, guard.alertLevel / 100f);
        else if (guard.alertStage == AlertStage.Alerted)
            c = Color.red;

        Handles.color = new Color(c.r, c.g, c.b, 0.3f);
        Handles.DrawSolidArc(
            guard.transform.position,
            guard.transform.up,
            Quaternion.AngleAxis(-guard.fovAngle /2f, guard.transform.up) * guard.transform.forward,
            guard.fovAngle,
            guard.fov) ;

        Handles.color = c;
        guard.fov = Handles.ScaleValueHandle(
            guard.fov,
            guard.transform.position + guard.transform.forward * guard.fov,
            guard.transform.rotation,
            3,
            Handles.SphereHandleCap,
            1);
    }
}

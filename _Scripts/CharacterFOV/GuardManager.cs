using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Mina Pechaux- creating a basic fov system
public enum AlertStage
{
    Peaceful,
    Intrigued,
    Alerted
}

public class GuardManager : MonoBehaviour
{
    public float fov;
    [Range(0f, 360f)] public float fovAngle;        //in degrees

    public AlertStage alertStage;
    [Range(0, 100)] public float alertLevel;    //0: Peaceful, 100: Alerted

    private void Awake()
    {
        alertStage = AlertStage.Peaceful;
        alertLevel= 0;
    }

    private void Update()
    {
        bool enemyInFOV = false;
        Collider[] targetsInFOV = Physics.OverlapSphere(transform.position, fov);

        foreach(Collider c in targetsInFOV)
        {
            if(c.CompareTag("Enemy"))
            {
                float signedAngle = Vector3.Angle(
                    transform.forward, 
                    c.transform.position - transform.position);
                if (Mathf.Abs(signedAngle) < fovAngle / 2)
                    enemyInFOV = true;
                break;
            }
        }

        UpdateAlertState(enemyInFOV);
    }

    private void UpdateAlertState(bool enemyInFOV)
    {
        switch(alertStage)
        {
            case AlertStage.Peaceful:
                if (enemyInFOV)
                    alertStage = AlertStage.Intrigued;
                break;
            case AlertStage.Intrigued:
                if(enemyInFOV)
                {
                    alertLevel++;
                    if (alertLevel >= 100)
                        alertStage = AlertStage.Alerted;
                }
                else
                {
                    alertLevel--;
                    if(alertLevel <= 0)
                        alertStage = AlertStage.Peaceful;
                }
                break;
            case AlertStage.Alerted:
                if (!enemyInFOV)
                    alertStage = AlertStage.Intrigued;
              break;
        }
    }
}

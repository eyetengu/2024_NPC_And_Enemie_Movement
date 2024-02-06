using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMap_New : MonoBehaviour
{
    [SerializeField] private Transform[] _accessiblePoints;

    public Transform[] HereIsTheInformationYouHaveRequested()
    {
        if(_accessiblePoints != null)
            return _accessiblePoints;
        else
            return null;
    }
}

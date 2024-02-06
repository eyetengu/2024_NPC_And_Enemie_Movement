using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private int _damageAmount;


    private void OnTriggerEnter(Collider other)
    {
        IDamageable hit = other.GetComponent<IDamageable>();
    
        if(hit != null)
        {
            hit.Damage(_damageAmount);
        }
    }
}

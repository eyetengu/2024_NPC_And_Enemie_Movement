using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable, IHealable
{
    public int maxPlayerHealth = 100;
    public int health = 50;

    public int Health { get; set; }

    public int MaxPlayerHealth { get; set; }
    
    public void AddHealth(int health)
    {
        this.health += health;
    }

    public void Damage(int damageAmount)
    {
        Health -= damageAmount;
    }

    public void Heal(int healAmount)
    {
        Health += healAmount;
    }
}

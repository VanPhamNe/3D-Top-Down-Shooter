using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathController : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    protected virtual void Awake()
    {
        // Initialization code can go here
        currentHealth = maxHealth;
    }
    public virtual void ReduceHealth(int damage)
    {
        currentHealth-=damage;

    }
    public virtual void IncreaseHealth()
    {
        currentHealth++;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public bool IsDead()
    {
        return currentHealth <= 0;
    }

}

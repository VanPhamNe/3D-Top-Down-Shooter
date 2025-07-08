using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowDamageArea : MonoBehaviour
{
    private EnemyBoss enemy;
    private float damageCooldown;
    private float lastTimeDamage;
    private int flameDamage;
    private void Awake()
    {
        enemy = GetComponentInParent<EnemyBoss>();
        damageCooldown = enemy.flameDamageCooldown;
        flameDamage = enemy.flamDamage; // Assuming EnemyBoss has a property for flame damage
    }
    private void OnTriggerStay(Collider other)
    {
        if (enemy.flameThrowerActive == false)
        {
            return; // If the flame thrower is not active, do nothing
        }
        if (Time.time - lastTimeDamage < damageCooldown)
        {
            return; // If the cooldown period has not passed, do nothing
        }
        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null) { 
            damagable.TakeDamage(flameDamage); // Check if the object has IDamagable component and apply damage if it does
            lastTimeDamage = Time.time; // Update the last time damage was applied
            //damageCooldown = enemy.flameDamageCooldown; // Test: Reset the damage cooldown to the enemy's flame damage cooldown
        }
    }
}

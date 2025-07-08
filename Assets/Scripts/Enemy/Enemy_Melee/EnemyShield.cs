using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour,IDamagable
{
    private Enemy_Melee enemy;
    [SerializeField] private int durability; // do ben cua shield
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy_Melee>(); // Lay Enemy_Melee tu EnemyShield
        durability = enemy.shieldDurability; // Lay do ben cua shield tu Enemy_Melee

    }
    public void ReduceDurability(int damage)
    {
        durability -= damage;
        if (durability <= 0)
        {
            enemy.anim.SetFloat("ChaseIndex", 0); // Reset the chase index to 0 when shield is destroyed
            gameObject.SetActive(false); // Deactivate the shield when durability reaches 0
        }
    }

    public void TakeDamage(int damage)
    {
        ReduceDurability(damage); // Reduce the shield's durability when it takes damage
    }

   
}

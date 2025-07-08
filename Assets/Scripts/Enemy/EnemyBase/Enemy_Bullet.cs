using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : Bullet
{
    protected override void OnCollisionEnter(Collision collision)
    {
        
        CreateImpactVFX(); 
        ReturnToPool(); 
        Player player = collision.gameObject.GetComponentInParent<Player>();

        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>(); // Lay IDamagable neu co
        damagable?.TakeDamage(bulletDamage); // Goi ham TakeDamage cua IDamagable neu co, de thuc hien viec bi thuong
        if (player != null)
        {
            Debug.Log("Shoot Player");
        }
    }
}

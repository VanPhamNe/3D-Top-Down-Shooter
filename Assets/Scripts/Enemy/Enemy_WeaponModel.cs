using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WeaponModel : MonoBehaviour
{
    public Enemy_MeleeWeaponType weaponType; // Type of the weapon model
    public AnimatorOverrideController animOverrideController; // Animator override controller for the weapon model
    [SerializeField] private GameObject[] traileffect;
    //public Enemy_MeleeWeaponData weaponData; // Data for the melee weapon
    [Header("Damage atribute")]
    public Transform[] damagePoint;
    public float attackRadius;
    [ContextMenu("Assign damage point transform")]
    private void GetDamagePoints() {
        damagePoint = new Transform[traileffect.Length];
        for (int i = 0; i < traileffect.Length; i++)
        {
            damagePoint[i] = traileffect[i].transform;
        }
    }
    public void EnableTrailEffect(bool enable)
    {
        foreach (var effect in traileffect)
        {
            effect.SetActive(enable);
        }
    }
    private void OnDrawGizmos()
    {
        if(damagePoint.Length > 0)
        {
            foreach (var point in damagePoint)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(point.position, attackRadius); // Draw a wire sphere at each damage point
            }
        }
    }
}

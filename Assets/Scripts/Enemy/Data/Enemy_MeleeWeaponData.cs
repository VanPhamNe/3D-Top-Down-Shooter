using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Enemy data/Enemy Melee Data")]
public class Enemy_MeleeWeaponData : ScriptableObject
{
   public List<MeleeAttackData> attackData; // List of melee attack data

  
}

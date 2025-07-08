using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum  HoldType_EnemyRange
{
    Common, LowHold,HighHold
};
public class EnemyRange_WeaponModel : MonoBehaviour
{
    public Transform gunPoint; // Point from which the bullet will be fired
    [Space]
    public Enemy_RangeWeaponType weaponType;
    public HoldType_EnemyRange holdType;

    public Transform leftHandTarget;
    public Transform leftElbowTarget;


}

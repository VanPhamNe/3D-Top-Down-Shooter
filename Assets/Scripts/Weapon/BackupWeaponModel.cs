using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum HangType {LowBackHand,BackHang,SideHang}
public class BackupWeaponModel : MonoBehaviour
{
    public WeaponType weaponType; // Type of the weapon
    [SerializeField] private HangType hangType; // Type of the hang
    public bool HangTypeIs(HangType type)
    {
        return hangType == type;
    }
    public void Active(bool active) =>gameObject.SetActive(active); // Activate or deactivate the weapon model
}

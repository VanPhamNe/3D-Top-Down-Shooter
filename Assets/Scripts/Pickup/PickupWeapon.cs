using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : Interacable
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels; //Danh sach cac model backup vu khi
    [SerializeField] private Weapon weapon;
    private bool oldWeapon;
    private void Start()
    {
       
        if(oldWeapon == false)
        {
            weapon = new Weapon(weaponData);
        }
        UpdateGameObject();
    }
    public override void Interaction()
    {
       playerWeaponController.PickupWeapon(weapon);
       ObjectPooling.Instance.ReturnObject(gameObject); //Tra ve doi tuong vao object pool
    }
    
    private void SetUpWeaponModel()
    {
        foreach (BackupWeaponModel backupModel in backupWeaponModels)
        {
            backupModel.gameObject.SetActive(false);
            if(backupModel.weaponType == weaponData.weaponType)
            {
                backupModel.gameObject.SetActive(true);
                UpdateMeshAndMat(backupModel.GetComponent<MeshRenderer>());                       
            }
        }
    }
    [ContextMenu("Update Item Model")]
    public void UpdateGameObject()
    {
        gameObject.name = "Pick up " + weaponData.weaponType.ToString();
        SetUpWeaponModel();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(playerWeaponController == null)
        {
            playerWeaponController = other.GetComponent<PlayerWeaponController>();
        }

    }
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
    public void SetupPickupWeapon(Weapon weapon,Transform transform)
    {
        oldWeapon = true; //Danh dau la vu khi cu
        this.weapon = weapon;
        weaponData = weapon.weaponData;
        this.transform.position = transform.position + new Vector3(0,0.75f,0);
    }
}

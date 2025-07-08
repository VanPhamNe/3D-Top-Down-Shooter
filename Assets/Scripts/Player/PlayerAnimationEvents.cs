using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerWeaponVisual weaponVisualController;
    private PlayerWeaponController weaponController;
    private void Start()
    {
        weaponVisualController = GetComponentInParent<PlayerWeaponVisual>();
        weaponController = GetComponentInParent<PlayerWeaponController>();
    }
    public void ReloadOver()
    {
        weaponVisualController.MaxWeightRigWeight(); //bat rig khi bat animation reload xong
        weaponController.GetCurrentWeapon().ReloadBullet(); //nap dan cho vu khi hien tai
        weaponController.SetWeaponReady(true); //dat trang thai vu khi la san sang
    }
    public void ReturnRig()
    {
        weaponVisualController.MaxWeightRigWeight();
        weaponVisualController.MaxLeftHandIKWeight(); //bat rig khi bat animation grab xong
    }
    public void GrabOver()
    {
       
        //weaponVisualController.setBusy(false); //tat flag busy khi bat animation grab xong
        weaponController.SetWeaponReady(true); //dat trang thai vu khi la san sang
    }
    public void SwitchOnWeaponModel() => weaponVisualController.SwitchOnModelCurrentGun(); //bat model vu khi hien tai
}

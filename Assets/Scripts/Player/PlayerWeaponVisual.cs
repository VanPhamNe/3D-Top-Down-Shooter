using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisual : MonoBehaviour
{
    private Animator animator;
    private Player player;
    [SerializeField] private WeaponModel[] weaponModels; //cac weapon model
    [SerializeField] private BackupWeaponModel[] backupWeaponModels; //cac weapon model backup
    [Header("LeftHand")]
    [SerializeField] private TwoBoneIKConstraint leftHandIK; //constraint tay trai
    [SerializeField] private Transform leftHandtarget;
    private Transform currentGun;
    [SerializeField] private float leftHandIKWeightIncrement; //bien kiem soat tang weight tay trai
    private bool shouldIncreaseLeftHandIKWeight; //bien kiem soat tang weight tay trai 
    //private bool isGrabbingWeapon; //bien kiem soat grab

    [Header("Rig")]
    private Rig rig;
    [SerializeField] private float rigWeightIncrement; //rigweight
    private bool rigBeIncrementing;
    private void Start()
    {
        
        player = GetComponent<Player>(); //lay player component
        animator = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true); //lay tat ca weapon model trong scene
        backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true); //lay tat ca backup weapon model trong scene
    }
    private void Update()
    {
        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    public void PlayReloadAnimation()
    {
        //if (isGrabbingWeapon)
        //    return; //neu dang grab thi khong cho reload
        float reloadSpeed = player.weaponController.GetCurrentWeapon().reloadSpeed; //lay thoi gian nap dan
        animator.SetFloat("ReloadSpeed", reloadSpeed); //cap nhat thoi gian nap dan vao animator
        animator.SetTrigger("Reload"); //bat animation reload
        PauseRig(); //tat rig khi bat animation reload
    }
    public WeaponModel GetCurrentWeaponModel()
    {
        WeaponModel currentWeaponModel = null;
        WeaponType weaponType = player.weaponController.GetCurrentWeapon().weaponType; //lay weapon type hien tai
        for(int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i].weaponType == weaponType) //neu weapon type trong weapon model bang voi weapon type hien tai
            {
                currentWeaponModel = weaponModels[i]; //gan current weapon model bang voi weapon model hien tai
            }
        }
        return currentWeaponModel; //tra ve current weapon model
    }
    #region Animation Rig
    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncreaseLeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIKWeightIncrement * Time.deltaTime; //tang weight tay trai
            if (leftHandIK.weight >= 1f)
            {
                shouldIncreaseLeftHandIKWeight = false; //tat flag tang weight tay trai
            }
        }
    }

    private void UpdateRigWeight()
    {
        if (rigBeIncrementing)
        {
            rig.weight += rigWeightIncrement * Time.deltaTime; //tang rig weight
            if (rig.weight >= 1f)
            {
                rigBeIncrementing = false; //tat flag increment
            }
        }
    }

    private void PauseRig()
    {
        rig.weight = 0.15f; 
    }

   
    public void MaxWeightRigWeight() => rigBeIncrementing = true; //bat flag increment khi bat animation reload xong
    public void MaxLeftHandIKWeight() => shouldIncreaseLeftHandIKWeight = true; //bat flag tang weight tay trai khi bat animation grab xong
    #endregion
    //public void setBusy(bool busy)
    //{
    //    isGrabbingWeapon = busy;
    //    animator.SetBool("BusyGrab", isGrabbingWeapon); //cap nhat animator flag grab
    //}
    public void PlayerEquipWeaponAnimation()
    {
        GrabType grabType = GetCurrentWeaponModel().grabType; //lay grab type hien tai
        float equipmentSpeed = player.weaponController.GetCurrentWeapon().equipSpeed; //lay thoi gian trang bi vu khi
       

        leftHandIK.weight = 0f; //reset weight tay trai ve 0 khi bat animation grab
        PauseRig(); //tat rig khi bat animation grab
        animator.SetFloat("WeaponGrabType", ((float)grabType)); //bat animation grab theo loai grab
        animator.SetFloat("GrabSpeed", equipmentSpeed); //cap nhat thoi gian trang bi vu khi
        animator.SetTrigger("GrabWeapon"); //bat animation grab
        //isGrabbingWeapon = true; //bat flag grab
        //setBusy(true);

    }
    public void PlayFireAnimation() => animator.SetTrigger("Fire"); //bat animation ban


    public void SwitchOnModelCurrentGun()
    {

        SwitchAnimationLayer((int)GetCurrentWeaponModel().holdType);
        SwitchGunOff(); //tat gun hien tai
        SwitchOffBackupWeaponModels(); //tat tat ca backup weapon model
        if(player.weaponController.HasOneWeapon()==false)
            SwitchOnBackupWeaponModels(); //bat backup weapon model theo weapon type hien tai
       
        //gunTransform.gameObject.SetActive(true); //bat gun hien tai len
        //currentGun = gunTransform; //cap nhat gun hien tai
        GetCurrentWeaponModel().gameObject.SetActive(true);
        AttachLeftHand(); //cap nhat tay trai theo gun hien tai
    }

    public void SwitchGunOff()
    {
        for(int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false); //tat weapon hien tai
        }
    }
    public void SwitchOnBackupWeaponModels()
    {
        //WeaponType weaponType = player.weaponController.BackupWeapon().weaponType; //lay weapon type hien tai
        SwitchOffBackupWeaponModels(); //tat tat ca backup weapon model
        BackupWeaponModel lowHangWeapon = null; 
        BackupWeaponModel backHangWeapon = null;
        BackupWeaponModel sideHangWeapon = null;
        foreach (BackupWeaponModel backupWeaponModel in backupWeaponModels)
        {
            if (player.weaponController.HasWeaponInSlot(backupWeaponModel.weaponType)!=null)
            {
                if(backupWeaponModel.weaponType == player.weaponController.GetCurrentWeapon().weaponType)
                {
                    continue; //neu weapon type cua backup weapon model bang voi weapon type hien tai thi bo qua
                }
                if (backupWeaponModel.HangTypeIs(HangType.LowBackHand))
                {
                    lowHangWeapon = backupWeaponModel; //gan low hang weapon
                }
                else if (backupWeaponModel.HangTypeIs(HangType.BackHang))
                {
                    backHangWeapon = backupWeaponModel; //gan back hang weapon
                }
                else if (backupWeaponModel.HangTypeIs(HangType.SideHang))
                {
                    sideHangWeapon = backupWeaponModel; //gan side hang weapon
                }
            }
        }
        lowHangWeapon?.Active(true); //bat low hang weapon neu co
        backHangWeapon?.Active(true); //bat back hang weapon neu co
        sideHangWeapon?.Active(true); //bat side hang weapon neu co
    }
    private void SwitchOffBackupWeaponModels()
    {
        foreach(BackupWeaponModel backupWeaponModel in backupWeaponModels)
        {
            backupWeaponModel.Active(false); //tat tat ca backup weapon model
        }
    }
    private void AttachLeftHand()
    {
        //Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;
        Transform targetTransform = GetCurrentWeaponModel().holdPont;
        leftHandtarget.localPosition = targetTransform.localPosition; //cap nhat vi tri tay trai
        leftHandtarget.localRotation = targetTransform.localRotation; //cap nhat huong tay trai
        //leftHandtarget.position = targetTransform.position;
        //leftHandtarget.rotation = targetTransform.rotation;
    }
    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
           animator.SetLayerWeight(i, 0f); //tat tat ca cac layer
        }
        animator.SetLayerWeight(layerIndex, 1f); //bat layer hien tai
    }
    
}

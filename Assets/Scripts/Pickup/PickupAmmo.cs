using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public enum AmmoBoxType
{
    BoxAmmoSmall, BoxAmmoBig
}
[System.Serializable]
public struct AmmoBoxData
{
    public WeaponType weaponType; //Loai vu khi
                                  //public int amount; //So luong dan trong hop

    [Range(10, 100)] public int minAmount; //So luong dan toi thieu
    [Range(10, 100)] public int maxAmount; //So luong dan toi da
}
public class PickupAmmo : Interacable
{
   
    [SerializeField] private AmmoBoxType ammoBoxType;
 
    [SerializeField] private List<AmmoBoxData> smallBoxAmmo;
    [SerializeField] private List<AmmoBoxData> bigBoxAmmo;
    [SerializeField] private GameObject[] boxModel;
    private void Start()
    {
        SetupBoxModel();
    }

    private void SetupBoxModel()
    {
        for (int i = 0; i < boxModel.Length; i++)
        {
            boxModel[i].SetActive(false); //Tat cac cac hop dan khac
            if (i == (int)ammoBoxType)
            {
                boxModel[i].SetActive(true);
                UpdateMeshAndMat(boxModel[i].GetComponent<MeshRenderer>()); //Cap nhat mesh va material cho hop dan
            }

        }
    }

    public override void Interaction()
    {
        List<AmmoBoxData> currentAmmoList = smallBoxAmmo; // Danh sach dan mac dinh la hop dan nho
        if (ammoBoxType == AmmoBoxType.BoxAmmoBig)
        {
            currentAmmoList = bigBoxAmmo;  // Neu la hop dan lon thi lay danh sach hop dan lon
        }
        foreach (AmmoBoxData ammoData in currentAmmoList)
        {
            Weapon weapon = playerWeaponController.HasWeaponInSlot(ammoData.weaponType); //Lay vu khi theo loai
            AddBulletToWeapon(weapon, GetBulletAmount(ammoData)); //Them dan vao vu khi
        }
        ObjectPooling.Instance.ReturnObject(gameObject); //Tra ve doi tuong vao object pool
    }
    private void AddBulletToWeapon(Weapon weapon, int amount)
    {
        if(weapon == null) return; //Neu vu khi khong ton tai thi ket thuc
        weapon.totalBullet += amount; //Them so luong dan vao tong so luong dan cua vu khi
    }
    private int GetBulletAmount(AmmoBoxData ammoData)
    {
        float min = Mathf.Min(ammoData.minAmount, ammoData.maxAmount); //Lay so luong dan toi thieu
        float max = Mathf.Max(ammoData.minAmount, ammoData.maxAmount); //Lay so luong dan toi da
        float randomAmmoAmount = Random.Range(min, max);
        return Mathf.RoundToInt(randomAmmoAmount); //Lay so luong dan ngau nhien trong khoang tu min den max
    }


}

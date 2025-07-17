using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponSlots : MonoBehaviour
{
    public Image weaponIcon;
    public TextMeshProUGUI ammoText;
    private void Awake()
    {
        weaponIcon = GetComponentInChildren<Image>();
        ammoText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void UpdateWeaponSlots(Weapon myWeapon,bool isActive)
    {
       if(myWeapon == null)
        {
            weaponIcon.color = Color.clear;
            ammoText.text = "";
            return;
       }
       Color newColor = isActive ? Color.white : new Color(1,1,1,.35f);
        weaponIcon.color = newColor;
        weaponIcon.sprite = myWeapon.weaponData.weaponIcon;
        ammoText.text = myWeapon.bulletInMagazines + "/" + myWeapon.totalBullet;


    }
}

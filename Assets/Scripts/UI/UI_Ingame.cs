using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ingame : MonoBehaviour
{
    [Header("Heath")]
    [SerializeField] private Image heathbar;

    [Header("Weapons")]
    [SerializeField] private UI_WeaponSlots[] weaponSlots_UI;

    [Header("Mission")]
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private TextMeshProUGUI missionDescriptionText;
    private void Awake()
    {
        weaponSlots_UI = GetComponentsInChildren<UI_WeaponSlots>();
    }
    public void UpdateWeaponUI(List<Weapon> weaponSlot,Weapon currentWeapon)
    {
        for(int i = 0; i < weaponSlots_UI.Length; i++)
        {
           if(i < weaponSlot.Count)
            {
                weaponSlots_UI[i].UpdateWeaponSlots(weaponSlot[i], weaponSlot[i] == currentWeapon);
            }
            else
            {
                weaponSlots_UI[i].UpdateWeaponSlots(null, false);
            }
        }
    }
    public void UpdateMissionInfo(string missionText, string missionDescription)
    {
        this.missionText.text = missionText;
        this.missionDescriptionText.text = missionDescription;
    }
    public void UpdateHeathUI(float currentHeath,float maxHeath)
    {
        heathbar.fillAmount = currentHeath / maxHeath;
    }
   
}

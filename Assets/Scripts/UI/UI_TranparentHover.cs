using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_TranparentHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Dictionary<Image, Color> originalImageColors = new Dictionary<Image, Color>();
    private Dictionary<TextMeshProUGUI , Color> originalTextColors = new Dictionary<TextMeshProUGUI, Color>();
    private bool hasUIWeaponSlot;
    private PlayerWeaponController playerWeaponController;
    private void Start()
    {
        hasUIWeaponSlot = GetComponentInChildren<UI_WeaponSlots>();
        if(hasUIWeaponSlot)
        {
            playerWeaponController = FindObjectOfType<PlayerWeaponController>();
         
        }
        foreach (var image in GetComponentsInChildren<Image>(true))
        {
            originalImageColors[image] = image.color;

        }
        foreach (var text in GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            originalTextColors[text] = text.color;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var image in GetComponentsInChildren<Image>(true))
        {
            var color = image.color;
            color.a = 0.5f; // Set alpha to 50% transparent
            image.color = color;

        }
        foreach (var text in GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            var color = text.color;
            color.a = 0.5f; // Set alpha to 50% transparent
            text.color = color;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var image in originalImageColors.Keys)
        {
            image.color = originalImageColors[image];
        }
        foreach (var text in originalTextColors.Keys)
        {
            text.color = originalTextColors[text];
        }
        if (hasUIWeaponSlot)
        {
            playerWeaponController?.UpdateWeaponUI();
        }
    }
}

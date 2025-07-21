using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxtext;
    [Header("BGM")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TextMeshProUGUI bgmtext;
    public void SFXSliderValue(float value)
    {
        sfxtext.text = Mathf.RoundToInt(value * 100) + "%";
    }
    public void BGMSliderValue(float value)
    {
        bgmtext.text = Mathf.RoundToInt(value * 100) + "%";
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    [SerializeField] private float sliderMultiplier = 25;
    [Header("SFX")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxtext;
    [SerializeField] private AudioMixer audiomixer;
    [SerializeField] private string sfxParameter;
    [Header("BGM")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TextMeshProUGUI bgmtext;
    [SerializeField] private string bgmParameter;
    public void SFXSliderValue(float value)
    {
        sfxtext.text = Mathf.RoundToInt(value * 100) + "%";
        float newValue = Mathf.Log10(value) * sliderMultiplier;
        audiomixer.SetFloat(sfxParameter, newValue);
        PlayerPrefs.SetFloat(sfxParameter, value);
        PlayerPrefs.Save();
    }
    public void BGMSliderValue(float value)
    {
        bgmtext.text = Mathf.RoundToInt(value * 100) + "%";
        float newValue = Mathf.Log10(value) * sliderMultiplier;
        audiomixer.SetFloat(bgmParameter, newValue);
        PlayerPrefs.SetFloat(bgmParameter, value);
        PlayerPrefs.Save();
    }
    public void LoadValues()
    {
        sfxSlider.value= PlayerPrefs.GetFloat(sfxParameter,.7f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, .7f);
    }

}

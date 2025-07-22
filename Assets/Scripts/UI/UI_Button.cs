using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class UI_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Mouse Hover")]
    public float scaleSpeed = 1;
    public float scaleRate= 1.2f;
    private Vector3 defaultScale;
    private Vector3 targetScale;
    private Image buttonImage;
    private TextMeshProUGUI buttonText;

    [Header("Audio")]
    [SerializeField] private AudioSource pointEnterSFX;
    [SerializeField] private AudioSource pointPressSFX;
    public virtual void Start()
    {
        defaultScale = transform.localScale;
        targetScale = defaultScale;
        buttonImage = GetComponent<Button>().image;
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public virtual void Update()
    {
       if(Mathf.Abs(transform.lossyScale.x - targetScale.x) > 0.01f)
        {
           float scaleValue = Mathf.Lerp(transform.localScale.x, targetScale.x, scaleSpeed * Time.deltaTime);
            transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        }
       
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = defaultScale * scaleRate;
        if (pointEnterSFX != null)
        {
            pointEnterSFX.Play();
        }
        if (buttonImage != null)
        {
            buttonImage.color = Color.yellow;
        }
        if (buttonText != null)
        {
            buttonText.color = Color.yellow;
        }

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        targetScale = defaultScale;
        buttonImage.color = Color.white;
        buttonText.color = Color.white;
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (pointPressSFX != null)
        {
            pointPressSFX.Play();
        }
    }
    public void AssignAudioSource()
    {
        pointEnterSFX = GameObject.Find("UI_PointEnter").GetComponent<AudioSource>();
        pointPressSFX = GameObject.Find("UI_ButtonPress").GetComponent<AudioSource>();
    }
}

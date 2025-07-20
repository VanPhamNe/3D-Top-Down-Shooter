using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ComicPanel : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image[] image;
    private int imageIndex;
    [SerializeField] private GameObject buttonEnable;
    [SerializeField] private Image myImage;
    private bool comicOver;

    private void Start()
    {
        myImage = GetComponentInChildren<Image>();
        ShowNextImage();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ShowNextImageOnClick();
    }
    private void ShowNextImageOnClick()
    {
        image[imageIndex].color = Color.white;
        imageIndex++;
        if (imageIndex >= image.Length)
            FinishComic();
        if (comicOver)
        {
            return;
        }
        ShowNextImage();
    }

    private void ShowNextImage()
    {
        if (comicOver) return;
        StartCoroutine(ChangeImageAlpha(1, 1.5f, ShowNextImage));
        
    }
    private IEnumerator ChangeImageAlpha(float targetAlpha, float duration, System.Action onComplete)
    {
        float time = 0;
        Color currentColor = image[imageIndex].color;
        float startAlpha = currentColor.a;
        while (time < duration) { 
            time+= Time.deltaTime;  
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time/duration);
            image[imageIndex].color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
        image[imageIndex].color = new Color(currentColor.r,currentColor.g,currentColor.b,targetAlpha);
        imageIndex++;
        if (imageIndex >= image.Length) { 
            FinishComic();    
        }
        onComplete?.Invoke();
    }
    private void FinishComic()
    {
        StopAllCoroutines();
        comicOver = true;
        buttonEnable.SetActive(true);
        myImage.raycastTarget = false;
    }
}

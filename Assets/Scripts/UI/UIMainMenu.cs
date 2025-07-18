using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public GameObject[] uiElements;
    public void SwitchTo(GameObject gameObject)
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
        gameObject.SetActive(true);
    }
    public void ChangeToHuntLevel()
    {
        GameManager.instance.LoadScene("HuntLevel");
    }

    public void ChangeToTimeLevel()
    {
        GameManager.instance.LoadScene("TimeLevel");
    }

}

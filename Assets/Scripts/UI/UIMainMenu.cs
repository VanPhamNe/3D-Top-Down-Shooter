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
        Time.timeScale = 1f; // Reset time scale to normal before switching scenes
    }

    public void ChangeToTimeLevel()
    {
        GameManager.instance.LoadScene("TimeLevel");
        Time.timeScale = 1f; // Reset time scale to normal before switching scenes
    }
    public void MainMenu()
    {
        GameManager.instance.LoadScene("MainMenu");
        Time.timeScale = 1f; // Reset time scale to normal before switching scenes
    }
    [ContextMenu("Assign Audio Button")]
    public void AssignAudioButton()
    {
        UI_Button[] buttons = FindObjectsOfType<UI_Button>(true);
        foreach (var button in buttons)
        {
            button.AssignAudioSource();
        }
    }

}

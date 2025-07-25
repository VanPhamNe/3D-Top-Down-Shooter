using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public GameObject[] uiElements;
    public UI_Settings settingsUI {  get; private set; }
    private void Awake()
    {
        settingsUI = GetComponentInChildren<UI_Settings>(true);
    }
    public void SwitchTo(GameObject gameObject)
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
        gameObject.SetActive(true);
        if(gameObject == settingsUI.gameObject)
        {
            settingsUI.LoadValues();
        }
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
    public void QuiteGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

}

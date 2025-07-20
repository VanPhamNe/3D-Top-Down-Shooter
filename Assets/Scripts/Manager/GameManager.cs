using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player; // Reference to the Player script
    public bool isGameComplete { get; private set; }
    private void Awake()
    {
        instance = this;
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void RestartScene() { 
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GameOver()
    {
        UI.Instance.ShowGameOverUI(); // Show Game Over UI with a message
    }
    public void GameComplete()
    {
        isGameComplete = true;
        UI.Instance.ShowVictoryScene();
        ControlsController.Instance.controls.Character.Disable();
        player.heath.currentHealth += 99999999;

    }

}

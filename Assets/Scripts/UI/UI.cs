using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI Instance;
    public UI_Ingame ingameUI { get; private set; }
    public GameObject pauseUI;
    public GameObject[] uiElements;
    public UI_GameOver gameOverUI;
    [SerializeField] private Image fadeimage;
    public GameObject victorySceneUI;

    private void Awake()
    {
        Instance = this;
        ingameUI = GetComponentInChildren<UI_Ingame>(true);
        gameOverUI = GetComponentInChildren<UI_GameOver>(true);
    }
    private void Start()
    {
        Time.timeScale = 1f; // Ensure the game starts with normal time scale
        AssignUI(); // Assign UI controls
        StartCoroutine(ChangeImageAlpha(0, 1.5f, null));
    }
    public void SwitchTo(GameObject gameObject)
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
        gameObject.SetActive(true);
    }
    public void PauseSwitch()
    {
        bool gamePaused = pauseUI.activeSelf;
        if(gamePaused)
        {
            SwitchTo(ingameUI.gameObject);
            ControlsController.Instance.SwitchToCharacterControls(); // Enable controls when unpausing
            Time.timeScale = 1f;
        }
        else
        {
            SwitchTo(pauseUI);
            ControlsController.Instance.SwitchToUIControls(); // Disable controls when pausing
            Time.timeScale = 0f;
            
        }
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale to normal before switching scenes
        GameManager.instance.LoadScene("MainMenu");
    }
    public void GoToEndScene()
    {
        GameManager.instance.LoadScene("TheEnd");
    }
    public void RestartLevel()
    {
        GameManager.instance.RestartScene(); // Restart the current level
      
    }
    private void AssignUI()
    {
        PlayerControls controls = GameManager.instance.player.controls;
        controls.UI.UIPause.performed += ctx => PauseSwitch(); // Register the UIPause event to switch to pause UI
    }
    public void ShowGameOverUI(string message = "Game Over")
    {
        SwitchTo(gameOverUI.gameObject); // Switch to the Game Over UI
        gameOverUI.UpdateGameOverText(message);

    }
    private IEnumerator ChangeImageAlpha(float targetAlpha, float duration, System.Action onComplete)
    {
        float time = 0;
        Color currentColor = fadeimage.color;
        float startAlpha = currentColor.a;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            fadeimage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
        fadeimage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
        onComplete?.Invoke();
    }
    public void ShowVictoryScene()
    {
        StartCoroutine(ChangeImageAlpha(1, 1.5f, SwitchToVictoryScene));
    }
    public void SwitchToVictoryScene()
    {
        GoToEndScene();
    }

}

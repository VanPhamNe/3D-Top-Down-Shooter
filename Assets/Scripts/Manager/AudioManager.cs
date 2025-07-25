using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private AudioSource[] sfx; 
    private int bgmIndex;
    private string currentScene;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        PlayMusicForScene(currentScene);
    }
    private void Update()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        if (activeScene != currentScene)
        {
            currentScene = activeScene;
            PlayMusicForScene(currentScene);
        }
    }
    private void PlayMusicForScene(string sceneName)
    {
        if (sceneName == "MainMenu")
        {
            PlayBGM(0);
        }
        else if (sceneName == "HuntLevel"||sceneName == "TimeLevel")
        {
            PlayBGM(2);
        }
        else {
            StopAllBGM();
        }
    }


    public void PlayBGM(int index)
    {
        StopAllBGM();
        bgmIndex = index;
        bgm[index].Play();
    }
    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
    public void PlaySFX(int index)
    {
        if (index < 0 || index >= sfx.Length)
        {
            return;
        }
        sfx[index].Play();
    }

    public void StopSFX(int index)
    {
        if (index < 0 || index >= sfx.Length)
        {
            return;
        }
        sfx[index].Stop();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public void ChangeSceneInGame()
    {
        GameManager.instance.LoadScene("HuntLevel");
    }
}

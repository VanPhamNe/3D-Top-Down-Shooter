using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI Instance;
    public UI_Ingame ingameUI;
    private void Awake()
    {
        Instance = this;
        ingameUI = GetComponentInChildren<UI_Ingame>();
    }
}

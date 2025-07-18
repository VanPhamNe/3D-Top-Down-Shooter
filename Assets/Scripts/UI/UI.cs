using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI Instance;
    public UI_Ingame ingameUI { get; private set; }
    private void Awake()
    {
        Instance = this;
        ingameUI = GetComponentInChildren<UI_Ingame>();
    }
}

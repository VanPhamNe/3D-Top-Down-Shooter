using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObjectHuntTarget : MonoBehaviour
{
    public static event Action<EnemyType> OnTargetKilled;
    public EnemyType enemyType; // Loai ke thu can san sang
    private bool isKilled = false;
    public void InvokeInTargetKill()
    {
        if (isKilled) return;  
        isKilled = true;
        OnTargetKilled?.Invoke(enemyType);
    } 
}

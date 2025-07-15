using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObjectHuntTarget : MonoBehaviour
{
    public static event Action<EnemyType> OnTargetKilled;
    public EnemyType enemyType; // Loai ke thu can san sang
    public void InvokeInTargetKill()
    {
        OnTargetKilled?.Invoke(enemyType);
    } 
}

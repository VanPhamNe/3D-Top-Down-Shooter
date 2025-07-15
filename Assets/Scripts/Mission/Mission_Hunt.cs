using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class EnemyKillRequirement
{
    public EnemyType enemyType;
    public int amountToKill;
    [HideInInspector] public int killedCount; 
}
[CreateAssetMenu(fileName = "New Hunt Mission", menuName = "Mission/Hunt Mission")]
public class Mission_Hunt : Mission
{
    //public int amountToKill; // So luong quai vat can tieu diet
    //public EnemyType[] enemyType; // Loai quai vat can tieu diet
    //private int killToGo; // So luong quai vat can tieu diet con lai
    //public override void StartMission()
    //{
    //    killToGo = amountToKill; // Khoi tao so luong quai vat can tieu diet con lai
    //    MissionObjectHuntTarget.OnTargetKilled += TargetEliminated; // Dang ky su kien khi quai vat bi tieu diet
    //    List<Enemy> validEnemy = new List<Enemy>();


    //    foreach (Enemy enemy in LevelGeneration.Instance.getEnemyList())
    //    {
    //        // Check if enemy type is in the required types
    //        foreach (var type in enemyType)
    //        {
    //            if (enemy.enemyType == type)
    //            {
    //                validEnemy.Add(enemy);
    //                break;
    //            }
    //        }
    //    }
        
   

    //    for (int i = 0; i < amountToKill; i++)
    //    {
    //        if(validEnemy.Count <=0 )
    //        {
    //            return;
    //        }
    //        int randomIndex = Random.Range(0, validEnemy.Count);
    //        validEnemy[randomIndex].AddComponent<MissionObjectHuntTarget>(); // Them component de danh dau quai vat la muc tieu cua nhiem vu
    //        validEnemy.RemoveAt(randomIndex);
    //    }
    //}
    //public override bool IsMissionComplete()
    //{
    //    return killToGo <= 0; // Kiem tra xem so luong quai vat can tieu diet con lai co bang 0 khong
    //}
    //public void TargetEliminated()
    //{
    //    killToGo--;
    //    if(killToGo <= 0)
    //    {
    //        MissionObjectHuntTarget.OnTargetKilled -= TargetEliminated; // Huy dang ky su kien khi nhiem vu hoan thanh
    //    }
    //}
    
    public EnemyKillRequirement[] killRequirements; 

    public override void StartMission()
    {
        foreach (var req in killRequirements)
        {
            req.killedCount = 0; 
        }
        MissionObjectHuntTarget.OnTargetKilled += TargetEliminated;

        foreach (var req in killRequirements)
        {
            List<Enemy> validEnemies = new List<Enemy>();
            foreach (Enemy enemy in LevelGeneration.Instance.getEnemyList())
            {
                if (enemy.enemyType == req.enemyType)
                {
                    validEnemies.Add(enemy);
                }
            }
            for (int i = 0; i < req.amountToKill && validEnemies.Count > 0; i++)
            {
                int randomIndex = Random.Range(0, validEnemies.Count);
                //validEnemies[randomIndex].AddComponent<MissionObjectHuntTarget>();
                var target = validEnemies[randomIndex].AddComponent<MissionObjectHuntTarget>();
                target.enemyType = req.enemyType;
                validEnemies.RemoveAt(randomIndex);
            }
        }
    }

    public override bool IsMissionComplete()
    {
        foreach (var req in killRequirements)
        {
            if (req.killedCount < req.amountToKill)
                return false;
        }
        return true;
    }

    public void TargetEliminated(EnemyType type)
    {
        foreach (var req in killRequirements)
        {
            if (req.enemyType == type && req.killedCount < req.amountToKill)
            {
                req.killedCount++;
                break;
            }
        }
        if (IsMissionComplete())
        {
            MissionObjectHuntTarget.OnTargetKilled -= TargetEliminated;
        }
    }

}

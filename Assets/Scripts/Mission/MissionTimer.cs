using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Timer Mission",menuName ="Mission/Timer Mission")]
public class MissionTimer : Mission
{
    public float time;
    private float currentTime;
    public override void StartMission()
    {
        currentTime = time; // Khoi tao thoi gian con lai
    }
    public override void UpdateMission()
    {
       
        currentTime -= Time.deltaTime; // Giam thoi gian con lai
        
        if(currentTime < 0)
        {
            Debug.Log("GAME OVER");
        }
        string timeText = System.TimeSpan.FromSeconds(currentTime).ToString(@"mm\:ss"); // Chuyen doi thoi gian con lai sang dinh dang mm:ss
        Debug.Log(timeText);
    }
    public override bool IsMissionComplete()
    {
        return currentTime > 0;
    }

   
}

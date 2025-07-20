using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Timer Mission",menuName ="Mission/Timer Mission")]
public class MissionTimer : Mission
{
    public float time;
    private float currentTime;
    private bool isStarted = false;




    public override void StartMission()
    {
        currentTime = time; // Khoi tao thoi gian con lai
        isStarted = true;

    }
    public override void UpdateMission()
    {
        if (!isStarted || GameManager.instance.isGameComplete) return;


        currentTime -= Time.deltaTime; // Giam thoi gian con lai

        if (currentTime <= 0)
        {
            GameManager.instance.GameOver(); // Neu thoi gian con lai nho hon 0, ket thuc tro choi
            ControlsController.Instance.SwitchToUIControls(); // Disable controls when showing Game Over UI
            Time.timeScale = 0f; // Pause the game when showing Game Over UI
        }
        string timeText = System.TimeSpan.FromSeconds(currentTime).ToString(@"mm\:ss"); // Chuyen doi thoi gian con lai sang dinh dang mm:ss
        //Debug.Log(timeText);
        string missionText = "Chay ngay di";
        string missionDescription = "Thoi gian con lai: " + timeText;
        UI.Instance.ingameUI.UpdateMissionInfo(missionText, missionDescription); // Cap nhat thong tin nhiem vu tren giao dien nguoi dung
    }
    public override bool IsMissionComplete()
    {
        return currentTime > 0;
    }
   

}

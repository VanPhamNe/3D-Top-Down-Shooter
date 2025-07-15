using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;
    public Mission currentMission; // Nhiem vu hien tai
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (currentMission != null)
        {
            Invoke(nameof(StartMission), 2); // Bat dau nhiem vu hien tai neu co
        }
    }
    private void Update()
    {
        currentMission?.UpdateMission(); // Cap nhat nhiem vu hien tai neu co
    }
    private void StartMission() => currentMission.StartMission(); // Bat dau nhiem vu hien tai neu co
    public bool MissionComplete() => currentMission.IsMissionComplete(); // Kiem tra xem nhiem vu hien tai da hoan thanh chua
    
}

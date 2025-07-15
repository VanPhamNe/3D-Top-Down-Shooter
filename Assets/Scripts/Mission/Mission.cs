using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mission : ScriptableObject
{
    public string missionName; // Ten nhiem vu
    [TextArea]
    public string missionDescription; // Mo ta nhiem vu
    public abstract void StartMission(); // Bat dau nhiem vu
    public abstract bool IsMissionComplete(); // Kiem tra xem nhiem vu da hoan thanh chua
    public virtual void UpdateMission()
    {
        // Phuong thuc co the duoc ghi de trong cac lop ke thua
        // De cap nhat trang thai nhiem vu neu can thiet
    }
}

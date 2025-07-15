using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission_End_Trigger : MonoBehaviour
{
    private GameObject player; // Doi tuong nguoi choi
    private void Start()
    {
        player = GameObject.Find("Player"); // Tim doi tuong nguoi choi
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != player)
        {
            return; // Neu doi tuong va nguoi choi khong phai la mot thi tra ve
        }
        if(MissionManager.Instance.MissionComplete())
        {
            Debug.Log("Mission Complete");
        }
      
    }
}

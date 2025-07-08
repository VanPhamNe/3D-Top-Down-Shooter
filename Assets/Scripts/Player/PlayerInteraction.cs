using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<Interacable> interactables=new List<Interacable>(); //Danh sach cac doi tuong co the tuong tac
    private Interacable closestInteractable; //Doi tuong tuong tac gan nhat 
    private void Start()
    {
        Player player = GetComponent<Player>(); //Lay Player component
        player.controls.Character.Interaction.performed += ctx => InteractClosest(); //Dang ky su kien khi nguoi choi nhan nut tuong tac
    }
    public void UpdateInteracble()
    {
        closestInteractable?.Highlight(false); //Tat highlight cua doi tuong tuong tac gan nhat neu co
        closestInteractable = null; //Reset doi tuong tuong tac gan nhat
        float closestDistance = float.MaxValue; //Khoang cach gan nhat ban dau la vo cuc
        foreach (Interacable interacable in interactables) //Duyet qua danh sach cac doi tuong tuong tac
        {
            
            float distance = Vector3.Distance(transform.position, interacable.transform.position); //Tinh khoang cach tu nhan vat toi doi tuong
            //Neu khoang cach nho hon khoang cach gan nhat hien tai
            if (distance < closestDistance)
            {
                closestDistance = distance; //Neu khoang cach gan hon thi cap nhat khoang cach gan nhat
                closestInteractable = interacable; //Cap nhat doi tuong tuong tac gan nhat
            }
                
        }
        closestInteractable?.Highlight(true); //Neu co doi tuong tuong tac gan nhat thi bat highlight cua no
    }
    private void InteractClosest()
    {
        closestInteractable?.Interaction(); //Neu co doi tuong tuong tac gan nhat thi thuc hien hanh dong tuong tac
        interactables.Remove(closestInteractable); //Xoa doi tuong tuong tac gan nhat khoi danh sach interactables
        UpdateInteracble(); //Cap nhat lai danh sach interactables
    }
    public List<Interacable> GetInteractables()
    {
        return interactables; //Tra ve danh sach cac doi tuong tuong tac
    }
}

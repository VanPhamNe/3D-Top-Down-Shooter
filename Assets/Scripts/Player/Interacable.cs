using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacable : MonoBehaviour
{
    protected MeshRenderer meshRenderer;
    private Material defaultMat;
    [SerializeField] protected Material highlightMat;
    protected PlayerWeaponController playerWeaponController; //Controller vu khi cua nguoi choi
    private void Start()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
    
        defaultMat = meshRenderer.sharedMaterial;
        
    }
 
    //Khi nguoi choi va cham doi tuong nay thi no se duoc them vao danh sach interactables cua PlayerInteraction
    protected virtual void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter: " + other.gameObject.name);
        if (playerWeaponController == null)
        {
            playerWeaponController = other.GetComponent<PlayerWeaponController>();
        }
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if (playerInteraction == null)
        {
            return;
        }
        playerInteraction.GetInteractables().Add(this);
        playerInteraction.UpdateInteracble(); //Cap nhat lai danh sach interactables khi co doi tuong moi
    }
    //Khi nguoi choi roi khoi doi tuong nay thi no se bi xoa khoi danh sach interactables cua PlayerInteraction
    protected virtual void OnTriggerExit(Collider other)
    {
        //Debug.Log("OnTriggerExit: " + other.gameObject.name);
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if (playerInteraction == null)
        {
            return;
        }
        playerInteraction.GetInteractables().Remove(this);
        playerInteraction.UpdateInteracble(); //Cap nhat lai danh sach interactables khi co doi tuong bi xoa
    }
    // Khi nguoi choi nhan nut tuong tac, no se thuc hien hanh dong tuong tac voi doi tuong nay
    public virtual void Interaction()
    {
        //Thuc hien hanh dong tuong tac voi doi tuong nay
        Debug.Log("Interacting with " + gameObject.name);
        //Them logic

    }
    public void Highlight(bool active)
    {
        if (active)
        {
            meshRenderer.material = highlightMat;
        }
        else
        {
            meshRenderer.material = defaultMat;
        }
    }
    protected void UpdateMeshAndMat(MeshRenderer newMesh)
    {
        meshRenderer = newMesh;
        defaultMat = newMesh.sharedMaterial;
    }
}

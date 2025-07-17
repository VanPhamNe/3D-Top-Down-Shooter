using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHeal : Interacable
{
    
    [SerializeField] private GameObject bigBoxModel;
    [SerializeField] private GameObject player;
    [SerializeField] private int healAmount;
    private void Start()
    {
        player = FindObjectOfType<Player>()?.gameObject;
        if (bigBoxModel != null)
        {
            bigBoxModel.SetActive(true);
            UpdateMeshAndMat(bigBoxModel.GetComponent<MeshRenderer>());
        }
    }
    public override void Interaction()
    {
        if (player == null) return;

        PlayerHeath playerHealth = player.GetComponent<PlayerHeath>();
        if (playerHealth != null && !playerHealth.IsDead())
        {
            playerHealth.IncreaseHealth(healAmount);
            
        }
        UI.Instance.ingameUI.UpdateHeathUI(playerHealth.currentHealth, playerHealth.maxHealth); // Cap nhat giao dien UI khi nhan vat nhan duoc suc khoe
        Destroy(gameObject); //Xoa doi tuong sau khi nhan vat nhan vat da nhan duoc suc khoe

    }

}

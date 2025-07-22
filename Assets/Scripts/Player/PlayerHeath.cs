using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeath : HeathController
{
    private Player player;
    public bool isDead { get; private set; } // Bien kiem tra xem nhan vat da chet chua
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
        
    }
    public override void ReduceHealth(int damage)
    {
        base.ReduceHealth(damage);
        if (IsDead())
        {
            Die();
        }
        UI.Instance.ingameUI.UpdateHeathUI(currentHealth, maxHealth); // Cap nhat giao dien UI khi bi thuong
    }
    private void Die() { 
        isDead = true; // Dat isDead la true khi chet
        player.ragdoll.RagdollActive(true); // Kich hoat ragdoll khi chet
        player.animator.enabled = false; // Tat Animator de tranh viec nhan vat van hoat dong
        AudioManager.Instance.StopSFX(0);
        GameManager.instance.GameOver(); // Ket thuc tro choi khi chet
    }
}

  

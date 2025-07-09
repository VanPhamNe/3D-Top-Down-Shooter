using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HitBox : HitBox
{
    private Player player;
    protected override void Awake()
    {
        base.Awake();
        // Additional initialization for Player_HitBox can be added here
        player = GetComponentInParent<Player>();
    }
    public override void TakeDamage(int damage)
    {
        int newDamage = Mathf.RoundToInt(damage * damageMultiplier); // Apply damage multiplier
        player.heath.ReduceHealth(newDamage); // Assuming ReduceHealth is a method in Player's health management script
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityState_Boss : EnemyState
{
    private EnemyBoss enemy;
    public AbilityState_Boss(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as EnemyBoss;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.flameDuration; // Set the state timer to the duration of the flame thrower ability
        enemy.agent.isStopped = true; // Stop the NavMeshAgent when entering the ability state
        enemy.agent.velocity = Vector3.zero; // Ensure the agent's velocity is zero to prevent movement during the ability
        enemy.bossVisuals.EnableWeaponTrail(true); // Disable the weapon trail effect when exiting the ability state

    }

    public override void Exit()
    {
        base.Exit();
        enemy.SetAbilityOnCooldown(); // Set the ability on cooldown when exiting the state

    }

    public override void Update()
    {
        base.Update();
        enemy.transform.rotation = enemy.FaceTarget(enemy.player.transform.position); // Face the player while using the ability
        if (stateTimer < 0)
        {
            DisableFlamethrower();

        }
        if (triggered)
        {
            stateMachine.ChangeState(enemy.moveState); // Change to move state when the ability is triggered
        }
    }
    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
        if(enemy.bossWeaponType == BossWeaponType.Fist)
        {
            enemy.ActiveFlameThrower(true); // Activate the flame thrower ability
            enemy.bossVisuals.EnableWeaponTrail(false); // Disable the weapon trail effect when exiting the ability state
        }
        if (enemy.bossWeaponType == BossWeaponType.Hammer)
        {
            enemy.ActiveHammerAbility();
        }

    }
    public void DisableFlamethrower()
    {
        if(enemy.bossWeaponType != BossWeaponType.Fist)
        {
            return; // If the boss is not using the fist weapon type, do nothing
        }
        if (enemy.flameThrowerActive == false)
        {
            return; // If the flame thrower is already inactive, do nothing
        }
        enemy.ActiveFlameThrower(false); // Deactivate the flame thrower ability
    }
}
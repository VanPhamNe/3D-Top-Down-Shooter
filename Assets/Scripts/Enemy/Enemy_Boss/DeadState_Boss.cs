using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState_Boss : EnemyState
{
    private EnemyBoss enemy;
    public DeadState_Boss(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as EnemyBoss; // Cast the enemy to EnemyBoss
    }

    public override void Enter()
    {
        base.Enter();
        enemy.abilityState.DisableFlamethrower(); // Ensure the flamethrower ability is disabled when entering the dead state
        enemy.anim.enabled = false; // Disable animation when dead
        enemy.agent.isStopped = true; // Stop the NavMeshAgent when dead
        enemy.ragdoll.RagdollActive(true); // Activate ragdoll physics
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

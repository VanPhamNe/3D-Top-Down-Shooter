using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_Boss : EnemyState
{
    private EnemyBoss enemy;
    public float lastTimeAttack; // Last time the boss attacked, used to manage attack cooldown
    public AttackState_Boss(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as EnemyBoss; // Cast to EnemyBoss to access specific properties or methods
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetFloat("AttackAnimIndex",Random.Range(0, 2)); // Random tan cong khi mang co 0 1 (2 phan tu)
        enemy.agent.isStopped = true; // Stop the NavMeshAgent when entering the attack state
        stateTimer = 1f;
        enemy.bossVisuals.EnableWeaponTrail(true); // Enable the weapon trail effect when entering the attack state

    }

    public override void Exit()
    {
        base.Exit();
        lastTimeAttack = Time.time; // Record the time when the attack state is exited
        enemy.bossVisuals.EnableWeaponTrail(false); // Enable the weapon trail effect when entering the attack state

    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            enemy.transform.rotation = enemy.FaceTarget(enemy.player.position,20); // Face the player while attacking
        }
        if (triggered)
        {
            if (enemy.IsPlayerInAttackRange())
            {
                stateMachine.ChangeState(enemy.idleState);
            }
            else { 
                stateMachine.ChangeState(enemy.moveState);
            }
        }
    }

}

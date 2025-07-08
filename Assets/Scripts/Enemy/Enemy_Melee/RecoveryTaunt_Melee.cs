using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryTaunt_Melee : EnemyState
{
    private Enemy_Melee enemy; // Reference to the specific melee enemy type
    public RecoveryTaunt_Melee(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as Enemy_Melee; // Cast the generic Enemy to Enemy_Melee
    }

    public override void Enter()
    {
        base.Enter();
        enemy.agent.isStopped = true; // Stop the NavMeshAgent to prevent movement during recovery taunt
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.transform.rotation = enemy.FaceTarget(enemy.player.position); // Rotate the enemy to face the player
        if (triggered)
        {
            if (enemy.IsPlayerInAttackRange())
            {
                stateMachine.ChangeState(enemy.attackState); // Change to chase state when the taunt is triggered
            }
            else
            {
                stateMachine.ChangeState(enemy.chaseState); // Change to chase state if the player is not in attack range
            }
           
        }
    }
}

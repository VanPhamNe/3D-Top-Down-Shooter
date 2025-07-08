using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Boss : EnemyState
{
    private EnemyBoss enemy;
    public IdleState_Boss(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as EnemyBoss; // Cast to EnemyBoss to access specific properties or methods
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(enemy.inBattleMode && enemy.IsPlayerInAttackRange())
        {
            stateMachine.ChangeState(enemy.attackState); // Transition to AttackState_Boss if player is in attack range
        }
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState); // Transition to MoveState_Boss after idle time
        }
      
    }
}

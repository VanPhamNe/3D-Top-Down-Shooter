using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Range : EnemyState
{
    private EnemyRange enemy;

    public IdleState_Range(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as EnemyRange;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime; // Set the timer for idle state
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer <= 0)
        {
            // Transition to the next state after idle time is over
            stateMachine.ChangeState(enemy.moveState);
        }

    }
}

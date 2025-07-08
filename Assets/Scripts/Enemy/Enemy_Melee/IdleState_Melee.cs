using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Melee : EnemyState
{
    private Enemy_Melee enemy; 
    public IdleState_Melee(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as Enemy_Melee; // Chuyen doi Enemy sang Enemy_Melee
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemyBase.idleTime; // Set the idle time for this state
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0) //Neu thoi gian trang thai <0 (hoan thanh trang thai) thi chuyen trang thai hoac lam gi do
        {
            //Debug.Log("Idle state finished");
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}

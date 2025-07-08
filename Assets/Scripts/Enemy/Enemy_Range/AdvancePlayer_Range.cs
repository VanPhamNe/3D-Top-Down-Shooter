using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancePlayer_Range : EnemyState
{
    private EnemyRange enemy;
    private Vector3 playerPos;
    public float lastTimeAdvance { get; private set; } // Last time the enemy advanced towards the player
    public AdvancePlayer_Range(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as EnemyRange;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.agent.isStopped = false; // Allow the NavMeshAgent to move
        enemy.agent.speed = enemy.advanceSpeed; // Set the speed of the NavMeshAgent to run speed
    }

    public override void Exit()
    {
        base.Exit();
        lastTimeAdvance = Time.time; // Update the last time the enemy advanced towards the player
    }

    public override void Update()
    {
        base.Update();
        playerPos = enemy.player.transform.position; // Get the player's position
        enemy.UpdateAimPos();
        enemy.agent.SetDestination(playerPos); // Set the destination to the player's position
        enemy.FaceTarget(enemy.agent.steeringTarget);
        if(canEnterBattleState())
        {
            enemy.stateMachine.ChangeState(enemy.battleState); // Change to battle state when the player is within stop distance
        }
    }
    private bool canEnterBattleState()
    {
        return Vector3.Distance(enemy.transform.position, playerPos) < enemy.advanceStopDistance && enemy.IsSeeingPlayer(); // Check if the player is within stop distance and the enemy can see the player
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState_Range : EnemyState
{
    private EnemyRange enemy;
    private Vector3 destination; // Destination for the enemy to move towards
    public MoveState_Range(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as EnemyRange;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.agent.isStopped = false; // Ensure the NavMeshAgent is not stopped when entering the move state

        destination = enemy.GetPatrolDestination(); // Get the next patrol point as the destination
        enemy.agent.speed = enemy.moveSpeed; // Set the speed of the NavMeshAgent
        enemy.agent.SetDestination(destination); // Set the destination for the NavMeshAgent
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.transform.rotation = enemy.FaceTarget(enemy.agent.steeringTarget); // Rotate the enemy to face the destination
        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance + 0.05f)
        {
            stateMachine.ChangeState(enemy.idleState); // Change to idle state when the destination is reached

        }
    }
}

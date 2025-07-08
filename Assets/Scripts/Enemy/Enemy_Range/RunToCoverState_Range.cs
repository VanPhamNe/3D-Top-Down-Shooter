using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunToCoverState_Range : EnemyState
{
    public EnemyRange enemy;
    private Vector3 destination; // Destination for the enemy to run to cover
    public float lastTimeTookCover { get; private set; } // Last time the enemy took cover
    public RunToCoverState_Range(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as EnemyRange;
    }

    public override void Enter()
    {
        base.Enter();
        destination = enemy.currentCover.transform.position; // Get the last cover position as the destination
        enemy.enemyVisual.EnableIK(false); // Disable IK for the enemy visual
        enemy.agent.isStopped = false; // Allow the NavMeshAgent to move
        enemy.agent.speed = enemy.runSpeed; // Set the speed of the NavMeshAgent to run speed
       
        enemy.agent.SetDestination(destination); // Set the destination to the last cover position
    }

    public override void Exit()
    {
        base.Exit();
        lastTimeTookCover = Time.time; // Update the last time the enemy took cover
    }

    public override void Update()
    {
        base.Update();
        enemy.transform.rotation = enemy.FaceTarget(enemy.agent.steeringTarget); // Rotate the enemy to face the destination
        if(Vector3.Distance(enemy.transform.position,destination) <= 0.75)
        {
            enemy.stateMachine.ChangeState(enemy.battleState); // Change to battle state when the destination is reached
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState_Melee : EnemyState
{
    private Enemy_Melee enemy; // Reference to the specific melee enemy type
    private Vector3 destination; // Destination for the enemy to move towards
    public MoveState_Melee(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as Enemy_Melee; // Cast the generic Enemy to Enemy_Melee
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
    private Vector3 GetNextPathPoint()
    {
        NavMeshAgent agent = enemy.agent;
        NavMeshPath path = new NavMeshPath();
        if(path.corners.Length < 2)
        {
            return agent.destination; // If there are no corners, return the destination
        }
        for(int i = 0; i < path.corners.Length; i++)
        {
            if (Vector3.Distance(enemy.transform.position, path.corners[i]) < 1)
            {
                return path.corners[i+1]; // Return the next point in the path that is beyond the stopping distance
            }
        }
        return agent.destination; // If no next point is found, return the destination
    }
}

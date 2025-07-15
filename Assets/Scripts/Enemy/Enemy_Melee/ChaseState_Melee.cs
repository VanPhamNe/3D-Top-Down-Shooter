using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState_Melee : EnemyState
{   
    private Enemy_Melee enemy; // Reference to the specific melee enemy type
    private float lastTimeUpdateDistance;
    public ChaseState_Melee(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as Enemy_Melee; // Cast the generic Enemy to Enemy_Mele
    }

    public override void Enter()
    {
        base.Enter();
        CheckChaseAnimation(); // Check and set the chase animation based on the enemy type
        enemy.agent.speed = enemy.runSpeed; // Set the speed of the NavMeshAgent for chasing
        enemy.agent.isStopped = false; // Ensure the NavMeshAgent is not stopped
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.position);
        float maxChaseRange = 10f;

        
        if (enemy.inBattleMode && distanceToPlayer > maxChaseRange)
        {
            enemy.ExitBattleMode();

           
            enemy.agent.destination = enemy.initialPosition;
            enemy.agent.speed = enemy.moveSpeed;

            
            if (Vector3.Distance(enemy.transform.position, enemy.initialPosition) < 0.5f)
            { 
                enemy.agent.isStopped = true;
                stateMachine.ChangeState(enemy.moveState);
            }

            return;
        }

        if (enemy.IsPlayerInAttackRange())
        {
            Debug.Log("Player in attack range, switching to attack state.");
            stateMachine.ChangeState(enemy.attackState); // Switch to attack state if player is in attack range
        }
        enemy.transform.rotation = enemy.FaceTarget(enemy.agent.steeringTarget); // Rotate the enemy to face the player
        if (CanUpdateDestination())
        {
            enemy.agent.destination = enemy.player.position; // Update the destination to the player's position

        }
    }
    private bool CanUpdateDestination()
    {
        if(Time.time > lastTimeUpdateDistance + .25f){
            lastTimeUpdateDistance = Time.time;
            return true;
        }
        return false;
    }
    private void CheckChaseAnimation()
    {
        if (enemy.meleeType == EnemyMelee_Type.Shield && enemy.shieldTransform == null)
        {
            enemy.anim.SetFloat("ChaseIndex", 0);
        }
    }
}

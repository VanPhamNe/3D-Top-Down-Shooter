
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState_Boss : EnemyState
{
    private EnemyBoss enemy;
    private Vector3 destination; // Destination for the enemy to move towards
    private float actionTimer;
    private float timeBeforeSpeedUp = 5;
    private bool isSpeedUp;
    public MoveState_Boss(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as EnemyBoss; // Cast to EnemyBoss to access specific properties or methods
    }

    public override void Enter()
    {
        base.Enter();

        actionTimer = enemy.actionCooldown; // Initialize the action timer for the move state
        destination = enemy.GetPatrolDestination(); // Get the next patrol point as the destination
        SpeedReset();
        enemy.agent.isStopped = false; // Ensure the NavMeshAgent is not stopped when entering the move state
        enemy.agent.SetDestination(destination); // Set the destination for the NavMeshAgent
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        actionTimer -= Time.deltaTime; // Decrease the action timer
        enemy.transform.rotation = enemy.FaceTarget(enemy.agent.steeringTarget); // Rotate the enemy to face the destination
        if (enemy.inBattleMode)
        {
            if (ShouldSpeedUp())
            {
                SpeedUp();
            }
            Vector3 playerPosition = enemy.player.position; // Get the player's position
            enemy.agent.SetDestination(playerPosition); //Duoi theo player
            if(actionTimer < 0) // Check if the action timer has reached zero
            {
                PerformRandomAction(); // Perform a random action (ability or jump attack)
            }
            
            else if (enemy.IsPlayerInAttackRange())
            {
                stateMachine.ChangeState(enemy.attackState);

            }
        }
        else
        {
            if (Vector3.Distance(enemy.transform.position,destination) < .25f)
            {
                stateMachine.ChangeState(enemy.idleState); // Change to idle state when the destination is reached

            }
        }
    

    }
    private void SpeedReset()
    {
        isSpeedUp = false; // Reset the speed-up flag
        enemy.anim.SetFloat("MoveAnimIndex", 0);
        enemy.anim.SetFloat("MoveAnimSpeedMultiplier", 1); // Speed up the animation

        enemy.agent.speed = enemy.moveSpeed; // Set the speed of the NavMeshAgent
    }
    private void SpeedUp()
    {
        enemy.agent.speed = enemy.runSpeed;
        enemy.anim.SetFloat("MoveAnimIndex", 1); // Set the animation index for running
        enemy.anim.SetFloat("MoveAnimSpeedMultiplier", 1.5f); // Speed up the animation
        isSpeedUp = true; // Set the speed-up flag to true
    }

    private void PerformRandomAction()
    {
        actionTimer = enemy.actionCooldown; // Reset the action timer

        if (Random.Range(0, 2) == 0) // random 0 toi 1
        {
            TryUseAbility();
        }
        else
        {
            if (enemy.canJumpAttack())
            {
                stateMachine.ChangeState(enemy.jumpAttackState); // Change to jump attack state if conditions are met
            }
            else if (enemy.bossWeaponType == BossWeaponType.Hammer)
                TryUseAbility();
        }
    }

    private void TryUseAbility()
    {
        if (enemy.canDoAbility())
        {
            stateMachine.ChangeState(enemy.abilityState); // Change to ability state if conditions are met
        }
    }

    private bool ShouldSpeedUp()
    {
        if (isSpeedUp)
        {
            return false; // If already speed up, do not change speed again
        }
        if(Time.time > enemy.attackState.lastTimeAttack + timeBeforeSpeedUp)
        {
            return true;
        }
        return false;
    }
}

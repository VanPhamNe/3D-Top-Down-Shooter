using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState_Melee : EnemyState
{
    private Enemy_Melee enemy;
   
    public DeadState_Melee(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as Enemy_Melee;
        
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.enabled = false; // Disable animation when dead
        enemy.agent.isStopped = true; // Stop the NavMeshAgent when dead
        enemy.ragdoll.RagdollActive(true); // Activate ragdoll physics
        if (enemy.shieldTransform != null)
        {
            enemy.shieldTransform.gameObject.SetActive(false);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

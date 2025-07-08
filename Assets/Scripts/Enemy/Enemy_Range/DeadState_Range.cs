using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState_Range : EnemyState
{
    public EnemyRange enemy;

    public DeadState_Range(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as EnemyRange;
    }

    public override void Enter()
    {
        base.Enter();
        //enemy.anim.enabled = false; // Disable animation when dead
        //enemy.agent.isStopped = true; // Stop the NavMeshAgent when dead
        //enemy.ragdoll.RagdollActive(true); // Activate ragdoll physics

        if (enemy == null)
        {
            Debug.LogError("enemy is NULL!");
            return;
        }

        if (enemy.anim == null)
        {
            Debug.LogError("enemy.anim is NULL!");
        }
        else
        {
            enemy.anim.enabled = false;
        }

        if (enemy.agent == null)
        {
            Debug.LogError("enemy.agent is NULL!");
        }
        else
        {
            enemy.agent.isStopped = true;
        }

        if (enemy.ragdoll == null)
        {
            Debug.LogError("enemy.ragdoll is NULL!");
        }
        else
        {
            enemy.ragdoll.RagdollActive(true);
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

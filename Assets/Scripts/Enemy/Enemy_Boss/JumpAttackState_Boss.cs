using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackState_Boss : EnemyState
{
    private EnemyBoss enemy;
    private Vector3 lastPlayerPosition; //Vi tri cuoi cung khi nhay toi player
    private float jumpAttackMovementSpeed; // Toc do di chuyen khi nhay toi player
    public JumpAttackState_Boss(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as EnemyBoss;
    }

    public override void Enter()
    {
        base.Enter();
        lastPlayerPosition = enemy.player.position; // Luu vi tri player luc bat dau nhay
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero; // Dung NavMeshAgent khi nhay toi player
        enemy.bossVisuals.PlaceLandingZone(lastPlayerPosition); // Dat vi tri cua landing zone khi nhay toi player
        enemy.bossVisuals.EnableWeaponTrail(true); // Bat dau hieu ung vu khi khi nhay toi player
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, lastPlayerPosition);
        jumpAttackMovementSpeed = distanceToPlayer / enemy.timeToTarget; // Tinh toan toc do di chuyen toi player
        enemy.transform.rotation = enemy.FaceTarget(lastPlayerPosition, 1000); // Quay Enemy ve huong player
        if(enemy.bossWeaponType == BossWeaponType.Hammer)
        {
            enemy.agent.isStopped = false;
            enemy.agent.speed = enemy.moveSpeed;
            enemy.agent.SetDestination(lastPlayerPosition); // Bat dau di chuyen toi player
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.SetJumpAttackOnCooldown(); // Dat thoi gian choi lai cho nhay toi player
        enemy.bossVisuals.EnableWeaponTrail(false); // Tat hieu ung vu khi sau khi nhay toi player
    }

    public override void Update()
    {
        base.Update();
        Vector3 myPos = enemy.transform.position;
        enemy.agent.enabled = !enemy.ManualMovementActive(); // Disable NavMeshAgent if manual movement is active
        if (enemy.ManualMovementActive())
        {
            enemy.transform.position =
                Vector3.MoveTowards(myPos, lastPlayerPosition, jumpAttackMovementSpeed * Time.deltaTime);
        }
        if (triggered)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}

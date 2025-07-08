using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_Melee : EnemyState
{
    private Enemy_Melee enemy; // Reference to the specific melee enemy type
    private Vector3 attackDirection;
    private const float MAX_ATTACK_DISTANCE = 50f; // Maximum distance for the attack to be effective
    private float attackMoveSpeed;
    public AttackState_Melee(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as Enemy_Melee; // Cast the generic Enemy to Enemy_Melee
    }

    public override void Enter()
    {
        base.Enter();
        attackMoveSpeed = enemy.attackData.moveSpeed; // Set the attack move speed from the attack data
        enemy.anim.SetFloat("AttackAnimationSpeed", enemy.attackData.animationSpeed); // Set the attack index for the animation
        enemy.anim.SetFloat("AttackIndex", enemy.attackData.attackIndex); // Set the attack index for the animation
        enemy.anim.SetFloat("SlashAttackIndex",Random.Range(0, 6)); // Randomly select a slash attack index for the animation
        enemy.EnableWeaponModel(true); // Pull out the weapon when entering attack state
        enemy.enemyVisual.EnableWeaponTrail(true); // Enable the weapon trail effect for the current attack
        enemy.agent.isStopped = true; // Stop the NavMeshAgent when entering attack state
        enemy.agent.velocity = Vector3.zero; // Ensure the agent's velocity is zero
        attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE); // Tinh toan huong tan cong toi Player
    }

    public override void Exit()
    {
        base.Exit();
        int recoveryIndex = PlayerClose() ? 1 : 0; // Determine the recovery taunt index based on player proximity
        enemy.anim.SetFloat("RecoveryIndex",recoveryIndex); // Set the recovery index for the animation
        enemy.attackData = UpdateAttackData(); // Update the attack data for the next attack
        enemy.enemyVisual.EnableWeaponTrail(false); // Disable the weapon trail effect after the attack

    }

    public override void Update()
    {
        base.Update();
        if (enemy.ManualRotationActive())
        {
            enemy.transform.rotation = enemy.FaceTarget(enemy.player.position); // Rotate the enemy to face the player when exiting attack state
            attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE); // Tinh toan huong tan cong toi Player

        }
        if (enemy.ManualMovementActive()) 
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, attackDirection, attackMoveSpeed * Time.deltaTime); // Di chuyen Enemy toi huong tan cong
        }
       
        if (triggered)
        {
            if(enemy.IsPlayerInAttackRange()) // Check if the player is still in attack range
            {
               stateMachine.ChangeState(enemy.recoveryTaunt); // Transition to recovery taunt state if player is in range
            }
            else
            {
                stateMachine.ChangeState(enemy.chaseState);
            }
           
        }
    }
    private bool PlayerClose() => Vector3.Distance(enemy.transform.position, enemy.player.position) < 1; // Check if the player is close enough for the attack
    private MeleeAttackData UpdateAttackData() {
        List<MeleeAttackData> attackList = new List<MeleeAttackData>(enemy.attackList); // tao mot ban sao cua danh sach attackList
      
        if (PlayerClose())
        {
            attackList.RemoveAll(parameter => parameter.attackType == AttackType_Melee.Charge); //  Xoa tat ca cac AttackData co AttackType_Melee.Charge neu Player o gan Enemy
        }
        int random = Random.Range(0, attackList.Count); //Chon ngau nhien mot chi so trong danh sach attackList
        return attackList[random]; // Tra ve AttackData ngau nhien tu danh sach attackList

    }
}

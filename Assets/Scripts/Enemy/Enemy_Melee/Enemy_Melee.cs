using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct MeleeAttackData 
{
    public float attackRange;
    public float moveSpeed;
    public float attackIndex;
    [Range(1f, 2f)]
    public float animationSpeed;
    public AttackType_Melee attackType;
    public string attackName;
    public int attackDamage;
}
public enum AttackType_Melee
{
    Close,
    Charge
}
public enum EnemyMelee_Type
{
    Regular,
    Shield,
    Dodge
}
public class Enemy_Melee : Enemy
{
   
    public IdleState_Melee idleState { get; private set; }
    public MoveState_Melee moveState { get; private set; }
    public RecoveryTaunt_Melee recoveryTaunt { get; private set; }
    public ChaseState_Melee chaseState { get; private set; }
    public AttackState_Melee attackState { get; private set; }
    public DeadState_Melee deadState { get; private set; }
   

    [Header("Attack Data")]
    public MeleeAttackData attackData;
    public List<MeleeAttackData> attackList;
    private Enemy_WeaponModel currenWeapon; // Reference to the weapon model for melee attacks
    private bool isAttackReady;
    [SerializeField] private GameObject meleeAttackVfx;

    [Header("Enemy Type")]
    public EnemyMelee_Type meleeType; // Type of the melee enemy, can be Regular or Shield
    public Enemy_MeleeWeaponType weaponType;

    [Header("Shield")]
    public Transform shieldTransform; // Reference to the player transform
    public int shieldDurability;
    public bool isDead { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState_Melee(this, stateMachine, "Idle");
        moveState = new MoveState_Melee(this, stateMachine, "Move");
        recoveryTaunt = new RecoveryTaunt_Melee(this, stateMachine, "Recovery");
        chaseState = new ChaseState_Melee(this, stateMachine, "Chase");
        attackState = new AttackState_Melee(this, stateMachine, "Attack");
        deadState = new DeadState_Melee(this, stateMachine, "Idle"); // Idle state for dead enemies, su dung ragdoll 
       
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        InializeSpecial(); // Initialize special properties based on the enemy type
        enemyVisual.SetUpLook(); // thay doi mau sac cua enemy
        UpdateAttackData(); // Update the attack data from the weapon model
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        MeleeAttackCheck(currenWeapon.damagePoint,currenWeapon.attackRadius,meleeAttackVfx,attackData.attackDamage); // Check for melee attacks
            


    }
    public override void EnterBattleMode()
    {
        if(inBattleMode) return; // If already in battle mode, do nothing
        base.EnterBattleMode();
        stateMachine.ChangeState(recoveryTaunt); // Change to chase state when entering battle mode
    }
    public void UpdateAttackData() //Update loai tan cong can chien dam boc cua Enemy
    {
        currenWeapon = enemyVisual.currentWeaponModel.GetComponent<Enemy_WeaponModel>(); // Get the current weapon model
      
    }
    
    protected override void InializeSpecial()
    {
        if(meleeType == EnemyMelee_Type.Shield)
        {
            anim.SetFloat("ChaseIndex", 1);
            shieldTransform.gameObject.SetActive(true); // Activate the shield transform for shield type enemies
            weaponType = Enemy_MeleeWeaponType.OneHand; // Set the weapon type to OneHand for shield enemies
        }
        if (meleeType == EnemyMelee_Type.Dodge)
        {
            weaponType = Enemy_MeleeWeaponType.Unarm; // Set the weapon type to Unarm for dodge enemies
        }



    }
    public override void Death()
    {
        if (isDead) return;

        isDead = true;
        base.Death();
        if(stateMachine.currentState != deadState)
        {
            stateMachine.ChangeState(deadState); // Change to dead state when the enemy dies
        }
    }
    public void EnableWeaponModel(bool active)
    {
        enemyVisual.currentWeaponModel.SetActive(active); // Activate the current weapon model
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green; // Set color for the Gizmos
        Gizmos.DrawWireSphere(transform.position, attackData.attackRange); // Draw a wire sphere to visualize the attack range
    }
    public bool IsPlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackData.attackRange; // Check if the player is within the attack range
}

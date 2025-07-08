using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
public enum BossWeaponType { Fist, Hammer }
public class EnemyBoss : Enemy
{
    [Header("Boss details")]
    public float attackRange;
    public float actionCooldown; //Thoi gian hanh vi cua boss
    public BossWeaponType bossWeaponType; // Type of weapon the boss uses
    [Header("Ability")]
  
    public float abilityCooldown; // Cooldown time for the ability
    private float lastAbilityTime; // Last time the ability was used
    public float minAbilityDistance;
    [Header("Flame Thrower Ability")]
    public float flameDuration; // Thoi gian keo dai cua Ability
    public ParticleSystem flameThrower; // Particle system for the flame thrower ability
    public float flameDamageCooldown; // Thoi gian giua cac dot danh cua Flame Thrower
    public int flamDamage; // Damage dealt by the flame thrower ability

    [Header("Hammer Ability")]
    public GameObject hammerFxPrefab; // Prefab for the hammer ability effect
    [SerializeField] private float hammerCheckRadius;
    public int hammerDamage; // Damage dealt by the hammer ability

    public bool flameThrowerActive { get; private set; } // Flag to check if the flame thrower ability is active
    [Header("Jump Attack")]
    public float timeToTarget = 1; // Thoi gian toi player khi nhay
    public float jumpAttackCooldown = 10;
    private float lastTimeJump;
    public float minJumpDisRequired; // Khoang cach toi thieu de nhay toi player
    public int jumpAttackDamage; // Damage dealt by the jump attack

    [Space]
    public float impactRadius = 2.5f;
    public float impactPower = 5;
    [SerializeField] private float upForceMultiplier = 10; // Multiplier for the upward force when jumping
    public Transform impactPoint;
    [SerializeField] private LayerMask whatToIgnore;

    [Header("Attack Check")]
    [SerializeField] private Transform[] damagePoints;
    [SerializeField] private float attackRadius;
    [SerializeField] private GameObject meleeAttackFx;
    [SerializeField] private int meleeAttackDamage;

    public EnemyBoss_Visuals bossVisuals; // Reference to the visuals component for the boss
    public IdleState_Boss idleState { get; private set; }
    public MoveState_Boss moveState { get; private set; }
    public AttackState_Boss attackState { get; private set; }
    public JumpAttackState_Boss jumpAttackState { get; private set; }
    public DeadState_Boss deadState { get; private set; } // Dead state for the boss, can be used for ragdoll or other effects
    public AbilityState_Boss abilityState { get; private set; } // Ability state for the boss, if needed
    protected override void Awake()
    {
        base.Awake();
        bossVisuals = GetComponent<EnemyBoss_Visuals>(); // Initialize the EnemyBoss_Visuals component
        // Initialize the EnemyBoss specific components or properties here if needed
        idleState = new IdleState_Boss(this, stateMachine, "Idle");
        moveState = new MoveState_Boss(this, stateMachine, "Move");
        attackState = new AttackState_Boss(this, stateMachine, "Attack");
        jumpAttackState = new JumpAttackState_Boss(this, stateMachine, "JumpAttack"); // Initialize the jump attack state
        abilityState = new AbilityState_Boss(this, stateMachine, "Ability"); // Initialize the ability state if needed
        deadState = new DeadState_Boss(this, stateMachine, "Idle"); // Initialize the dead state for the boss
    }
    protected override void Start()
    {
        base.Start();
        // Additional initialization for EnemyBoss can be done here
        stateMachine.Initialize(idleState); // Start with the idle state
    }
    protected override void Update()
    {
        base.Update();
        //if (Input.GetKeyDown(KeyCode.V)) // Example input to trigger battle mode
        //{
        //    stateMachine.ChangeState(abilityState); // Change to ability state when V is pressed
        //}
        stateMachine.currentState.Update(); // Ensure the current state is updated
       
        if (ShouldEnterBattleMode())
        {
            EnterBattleMode(); // Enter battle mode if conditions are met
        }
        MeleeAttackCheck(damagePoints, attackRadius,meleeAttackFx,meleeAttackDamage); // Check for melee attacks using the damage points and attack radius
    }
    public override void EnterBattleMode()
    {
        if(inBattleMode) return; // If already in battle mode, do nothing
        base.EnterBattleMode();

        stateMachine.ChangeState(moveState); // Change to move state when entering battle mode
    }
    public void ActiveFlameThrower(bool active)
    {
        flameThrowerActive = active; // Set the flame thrower active state
        if (!active)
        {
            flameThrower.Stop(); // Stop the particle system if not active
            anim.SetTrigger("StopFlameAbility");
            Debug.Log("Stop Flame Ability");
   
            return;
        }
        var mainModule = flameThrower.main; // Get the main module of the particle system
        var childModule = flameThrower.transform.GetChild(0).GetComponent<ParticleSystem>().main; // Get the main module of the child particle system

        mainModule.duration = flameDuration; // Set the duration of the particle system to the flame duration
        childModule.duration = flameDuration; // Set the duration of the child particle system to the flame duration
        flameThrower.Clear(); // Clear any previous particles
        flameThrower.Play(); // Start the particle system
    }
    public void ActiveHammerAbility()
    {
        GameObject newActive = ObjectPooling.Instance.GetObject(hammerFxPrefab, impactPoint);
        ObjectPooling.Instance.ReturnObject(newActive, 1); // Return the object to the pool after 1 second
        DamageArea(damagePoints[0].position, hammerCheckRadius,hammerDamage);
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red; // Set the color for the Gizmos
        Gizmos.DrawWireSphere(transform.position, attackRange); // Draw a wire sphere to visualize the attack range
        Gizmos.color = Color.blue; // Set the color for the jump attack range
        Gizmos.DrawWireSphere(transform.position, minJumpDisRequired); // Draw a wire sphere to visualize the minimum jump distance required
        Gizmos.color = Color.green; // Set the color for the impact radius
        Gizmos.DrawWireSphere(transform.position, impactRadius); // Draw a wire sphere to visualize the impact radius of the jump attack
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, minAbilityDistance); // Draw a wire sphere at the impact point to visualize the impact radius
        if (damagePoints.Length > 0)
        {
            foreach (var damage in damagePoints)
            {
                Gizmos.DrawWireSphere(damage.position, attackRadius);
            }
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(damagePoints[0].position, hammerCheckRadius); // Draw a wire sphere to visualize the hammer check radius
        }
     
        
    }
    public bool canDoAbility()
    {
        bool playerWithinDistance = Vector3.Distance(transform.position, player.position) < minAbilityDistance; // Check if the player is within the minimum distance required for the ability
        if(playerWithinDistance == false)
        {
            return false; // Cannot use ability if player is not within distance
        }

        if (Time.time > lastAbilityTime + abilityCooldown) // Check if the cooldown period has passed
        {
            
            return true; // Allow the ability to be used
        }
        return false; // Cooldown not finished, cannot use ability
    }
    public void SetAbilityOnCooldown() => lastAbilityTime = Time.time; // Set the last ability time to the current time to start the cooldown
    public void JumpImpact()
    {
        Transform impactPoint = this.impactPoint;
        if (impactPoint == null) // Check if the impact point is set
        {
            impactPoint = transform; // Use the enemy's position as the impact point if not set
        }
        DamageArea(impactPoint.position, impactRadius,jumpAttackDamage);
    }
    private void DamageArea(Vector3 impactPoint, float impactRadius,int damage)
    {
        HashSet<GameObject> uniqueEntities = new HashSet<GameObject>(); // Use a HashSet to store unique entities hit by the explosion
        Collider[] hitColliders = Physics.OverlapSphere(impactPoint, impactRadius,~whatIsAlly);
        foreach (Collider hitCollider in hitColliders)
        {
           
            IDamagable damageable = hitCollider.GetComponent<IDamagable>(); // Get the IDamagable component from the collider
            if (damageable != null)
            {
                GameObject rootEntity = hitCollider.transform.root.gameObject; // Get the root entity of the collider
                if (uniqueEntities.Add(rootEntity) == false) // Check if the entity has already been hit
                {
                    continue; // Skip to the next collider if the entity has already been hit
                }
                Debug.Log(hitCollider.transform.root.name + "Damage");
                damageable.TakeDamage(damage); // Apply damage to the entity if it has IDamagable component
            }
            ApplyPhysicForce(impactPoint, impactRadius, hitCollider);
        }
    }

    private void ApplyPhysicForce(Vector3 impactPoint, float impactRadius, Collider hitCollider)
    {
        Rigidbody rb = hitCollider.GetComponent<Rigidbody>(); // Get the Rigidbody component of the collider
        if (rb != null)
        {
            rb.AddExplosionForce(impactPower, impactPoint, impactRadius, upForceMultiplier, ForceMode.Impulse); // Apply explosion force to the Rigidbody
        }
    }

    public bool canJumpAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calculate distance to player
        if(distanceToPlayer < minJumpDisRequired) // Check if the distance is less than the minimum required distance
        {
            return false; // Cannot jump attack if too close
        }
        if (Time.time  > jumpAttackCooldown + lastTimeJump && IsPlayerInClearSight()) // Check if the cooldown period has passed
        {
            
            return true; // Allow jump attack
        }
        return false; // Cooldown not finished, cannot jump attack
    }
    public void SetJumpAttackOnCooldown() => lastTimeJump = Time.time; // Set the last jump time to the current time to start the cooldown
    public bool IsPlayerInClearSight() {
        Vector3 myPos= transform.position + new Vector3(0,1.5f,0);
        Vector3 playerPos = player.position + Vector3.up; 
        Vector3 dirToPlayer = (playerPos - myPos).normalized; // Calculate the direction to the player
        if (Physics.Raycast(myPos,dirToPlayer,out RaycastHit hit,100,~whatToIgnore))
        {
            if(hit.transform == player ||hit.transform.parent == player) // Check if the raycast hit the player or its parent
            {
                Debug.Log(hit.transform.name);
                return true; // Player is in clear sight
            }
        }
        return false;

    }
    public override void Death()
    {
        base.Death();
        if (stateMachine.currentState != deadState)
        {
            stateMachine.ChangeState(deadState); // Change to dead state when hit
        }
    }
    public bool IsPlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackRange; // Check if the player is within the attack range

}

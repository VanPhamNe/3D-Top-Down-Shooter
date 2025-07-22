using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum CoverPerk { Unavalible, CanTakeCover , CanTakeAndChangeCover } // cac the loai nhu (Dung im ban, kiem cover ban va vua kien cover vua doi cover)
public class EnemyRange : Enemy
{
    [Header("Weapon details")]
    public Enemy_RangeWeaponType weaponType; // Type of weapon used by the enemy
    public Enemy_RangeWeaponData weaponData; // Data for the weapon used by the enemy
    [Space]
    public Transform weaponHolder;
    public GameObject bulletPrefab; // Prefab of the bullet to be fired
    public Transform gunPoint; // Point from which the bullet will be fired

    [Header("Enemy Perks")]
    public CoverPerk coverPerk;
    

    [Header("Cover System")]
    //public bool canUseCover = true; // Flag to check if the enemy can use cover
    public CoverPoint currentCover; // Current cover point the enemy is using
    public CoverPoint lastCover; // Last cover point the enemy used
    public float safeDistance;
    public float minStayInCoverTime;

    [Header("Advance")]
    public float advanceSpeed;
    public float advanceStopDistance; // Distance at which the enemy stops advancing towards the player
    public float advanceTime = 2.5f;

    [Header("Aim Details")]
    public float slowAim = 4;
    public float fastAim = 20;
    public Transform aim;
    public Transform playersBody; // Reference to the player transform
    public LayerMask whatToIgnore; // Layer mask to ignore certain layers when aiming


    [SerializeField] List<Enemy_RangeWeaponData> availableWeaponData; // List of available weapon data for the enemy
                                                                      //public float fireRate = 1f; // Time between shots
                                                                      //public float bulletSpeed = 20f; // Speed of the bullet
                                                                      //public int bulletToShoot = 5; // Number of bullets to shoot in a burst
                                                                      //public float weaponCooldown = 1.5f; // Cooldown time for the weapon

    public IdleState_Range idleState { get; private set; } // State when the enemy is idle
    public MoveState_Range moveState { get; private set; } // State when the enemy is patrolling
    public BattleState_Range battleState { get; private set; } // State when the enemy is in battle mode\
    public RunToCoverState_Range runToCoverState { get; private set; } // State when the enemy is running to cover
    public AdvancePlayer_Range advancePlayerState { get; private set; } // State when the enemy is advancing towards the player
    public DeadState_Range deadState { get; private set; } // State when the enemy is dead
    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState_Range(this, stateMachine, "Idle");
        moveState = new MoveState_Range(this, stateMachine, "Move");
        battleState = new BattleState_Range(this, stateMachine, "Battle"); // Initialize the battle state
        runToCoverState = new RunToCoverState_Range(this, stateMachine, "RunToCover"); // Initialize the run to cover state
        advancePlayerState = new AdvancePlayer_Range(this, stateMachine, "Advance"); // Initialize the advance player state
        deadState = new DeadState_Range(this, stateMachine, "Idle"); // Initialize the dead state

    }
    protected override void Start()
    {
        base.Start();
        playersBody = player.GetComponent<Player>().playerBody;
        aim.parent = null;
        stateMachine.Initialize(idleState);
        enemyVisual.SetUpLook(); // Set up the visual appearance of the enemy
        SetUpWeaponData(); // Set up the weapon data for the enemy

    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update(); // Update the current state of the enemy
    }
    public override void EnterBattleMode()
    {
        if (inBattleMode) return; // If already in battle mode, do nothing
        base.EnterBattleMode();
        if (CanGetCover())
        {
            stateMachine.ChangeState(runToCoverState); // Change to run to cover state if the enemy can use cover

        }
        else
        {
            stateMachine.ChangeState(battleState); // Change to battle state when entering battle mode
        }
    }
    public override void Death()
    {
        base.Death();
        if(stateMachine.currentState != deadState) // If the current state is not dead state
        {
            stateMachine.ChangeState(deadState); // Change to dead state
        }
    }
    public void FireSingleBullet()
    {
        Debug.Log("FireSingleBullet Called");
        AudioManager.Instance.PlaySFX(3);
        anim.SetTrigger("Shoot");
        Vector3 bulletDir = (aim.position - gunPoint.position).normalized; // Calculate the direction to the player
        GameObject newbullet = ObjectPooling.Instance.GetObject(bulletPrefab,gunPoint); // Get a bullet from the object pool
        //newbullet.transform.position = gunPoint.position; // Set the position of the bullet to the gun point
        newbullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward); // Set the rotation of the bullet to face the player
        newbullet.GetComponent<Enemy_Bullet>().BulletSetup(weaponData.bulletDamage); // Setup the bullet with a fly distance and impact force
        Rigidbody rbnewbullet = newbullet.GetComponent<Rigidbody>(); // Get the Rigidbody component of the bullet
        Vector3 bulletDirWithSpread = weaponData.ApplyWeaponSpread(bulletDir); // Apply weapon spread to the bullet direction
        rbnewbullet.mass = 20 / weaponData.bulletSpeed;
        rbnewbullet.velocity = bulletDirWithSpread * weaponData.bulletSpeed; // Set the velocity of the bullet to fire it towards the player
    }
    private void SetUpWeaponData()
    {
        List<Enemy_RangeWeaponData> filteredWeapons = new List<Enemy_RangeWeaponData>();
        foreach (var weaponData in availableWeaponData)
        {
            if (weaponData.weaponType == weaponType) // Check if the weapon type matches
            {
                filteredWeapons.Add(weaponData); // Add the matching weapon to the list
            }
        }
        if (filteredWeapons.Count > 0)
        {
            int random = Random.Range(0, filteredWeapons.Count); // Get a random index from the filtered weapons
            weaponData = filteredWeapons[random]; // Set the weapon data to a random weapon from the filtered list
        }
        else
        {
            Debug.LogWarning("Khong tim thay weapon data hop le");

        }
        gunPoint = enemyVisual.currentWeaponModel.GetComponent<EnemyRange_WeaponModel>().gunPoint; // Get the gun point from the current weapon model
    }
    #region EnemyAim
    public bool AimOnPlayer()
    {
        float distanceAimToPlayer = Vector3.Distance(aim.position, playersBody.position); // Calculate the distance from the aim to the player

        return distanceAimToPlayer < 2;
    }
    public void UpdateAimPos()
    {
        float aimSpeed = AimOnPlayer() ? fastAim : slowAim; // Set the aim speed based on whether the aim is on the player or not
        aim.position = Vector3.Lerp(aim.position, playersBody.position, aimSpeed * Time.deltaTime); // Smoothly move the aim towards the player's position
    }

    public bool IsSeeingPlayer()
    {
        Vector3 myPos = transform.position + Vector3.up;
        Vector3 directionToPlayer = playersBody.position - myPos;
        if (Physics.Raycast(myPos, directionToPlayer, out RaycastHit hit, Mathf.Infinity, ~whatToIgnore)) // Ve mot raycast tu Enemy toi Player, bo qua cac layer trong whatToIgnore
        {
            if (hit.transform.root == player.root)
            {
                UpdateAimPos(); // Cap nhat vi tri aim neu raycast cham vao Player
                return true; // Tra ve true neu Enemy co the thay Player
            }
        }
        return false;

    }
    #endregion
    #region Cover System
    private List<Cover> CollectNearbyCover()
    {
        float coverCollectRadiusCheck = 30;
        Collider[] hitcolliders = Physics.OverlapSphere(transform.position, coverCollectRadiusCheck); // Get all colliders in the cover layer within a certain radius
        List<Cover> collectCovers = new List<Cover>();
        foreach (Collider hit in hitcolliders)
        {
            Cover cover = hit.GetComponent<Cover>(); // Get the Cover component from the collider
            if (cover != null && collectCovers.Contains(cover) == false)
            {
                collectCovers.Add(cover); // Add the cover to the list if it is not null
            }
        }
        return collectCovers; // Return the list of collected covers
    }
    public bool CanGetCover()
    {
        if (coverPerk == CoverPerk.Unavalible)
        {
            return false;
        }
        currentCover = FindCover()?.GetComponent<CoverPoint>();
        if (lastCover != currentCover && currentCover != null)
        {
            return true;
        }
        Debug.Log("No cover found");
        return false; // Return false if no cover is found or if the current cover is the same as the last cover
    }
    public Transform FindCover()
    {
        List<CoverPoint> collectCoverPoints = new List<CoverPoint>();
        foreach (Cover cover in CollectNearbyCover())
        {
            collectCoverPoints.AddRange(cover.GetValidCoverPoints(transform)); // Add the cover points to the list

        }
        CoverPoint closestCoverPoint = null; // Initialize the closest cover point
        float shortestDistance = float.MaxValue; // Initialize the shortest distance to infinity
        foreach (CoverPoint coverPoint in collectCoverPoints)
        {
            float currentDistance = Vector3.Distance(transform.position, coverPoint.transform.position); // Calculate the distance to the cover point
            if (currentDistance < shortestDistance) // If the distance is shorter than the current shortest distance
            {
                shortestDistance = currentDistance; // Update the shortest distance
                closestCoverPoint = coverPoint; // Update the closest cover point
            }
        }
        if (closestCoverPoint != null)
        {
            lastCover?.SetOccupied(false); // Set the last cover point as not occupied
            lastCover = currentCover;
            currentCover = closestCoverPoint; // Set the current cover point to the closest cover poin
            currentCover.SetOccupied(true); // Set the closest cover point as occupied
            return currentCover.transform; // Return the closest cover point
        }
        return null; // If no cover point is found, return null


    }
    #endregion
 
  
}

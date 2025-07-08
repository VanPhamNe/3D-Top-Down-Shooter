using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState_Range : EnemyState
{
    private EnemyRange enemy;
    private float lastTimeShot = -10;
    private int bulletShot = 0; // Number of bullets shot in the current burst
    private int bulletsPerAttack; // Number of bullets to shoot in the current burst
    private float weaponCooldown; // Cooldown time for the weapon
    private float coverCheckTimer;
    public BattleState_Range(Enemy enemy, EnemyStateMachine stateMachine, string boolName) : base(enemy, stateMachine, boolName)
    {
        this.enemy = enemy as EnemyRange;
    }

    public override void Enter()
    {
        base.Enter();
      
            bulletsPerAttack = enemy.weaponData.GetBulletPerAttack(); // Get the number of bullets to shoot in the current burst
            weaponCooldown = enemy.weaponData.GetWeaponCooldown(); // Get the cooldown time for the weapon
      
       
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero; // Stop the NavMeshAgent from moving
        enemy.enemyVisual.EnableIK(true); // Enable IK for the enemy visual
    }

    public override void Exit()
    {
        base.Exit();
        enemy.enemyVisual.EnableIK(false); // Enable IK for the enemy visual
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsSeeingPlayer())
        {
            enemy.transform.rotation = enemy.FaceTarget(enemy.aim.position); // Rotate the enemy to face the player
        }
        //Neu player o tam agressive == false  doi sang state khac
        if (enemy.IsPlayerInAggressiveRange()==false && ReadyToLeaveCover())
        {
            stateMachine.ChangeState(enemy.advancePlayerState);
        }

        ChangeCoverToShoot();
        if (WeaponOutOfBullet()) // Check if the weapon is out of bullets
        {
            if (WeaponOnCooldown()) // If the weapon is on cooldown, do not shoot
            {
                AttempToResetWeapon();
            }
            return;
        }
        if (Time.time > lastTimeShot + 1 / enemy.weaponData.fireRate && enemy.AimOnPlayer())
        {
            Shoot();
        }

    }
  
    #region Weapon
    private void AttempToResetWeapon()
    {
        bulletShot = 0; // Reset the bullet shot count
        bulletsPerAttack = enemy.weaponData.GetBulletPerAttack(); // Get a new number of bullets to shoot in the next burst
        weaponCooldown = enemy.weaponData.GetWeaponCooldown(); // Get a new cooldown time for the weapon
    }

    private bool WeaponOnCooldown() => Time.time > lastTimeShot + weaponCooldown; // Check if the weapon is on cooldown
    private void Shoot()
    {
        enemy.FireSingleBullet(); // Fire a bullet at the player
        lastTimeShot = Time.time; // Update the last time shot
        bulletShot++; // Increment the number of bullets shot
    }
    private bool WeaponOutOfBullet() => bulletShot >= bulletsPerAttack; // Check if the number of bullets shot has reached the limit for a burst
    #endregion
    #region Cover
    private bool IsPlayerInClearSight()
    {
        Vector3 dirToPlayer = enemy.player.transform.position - enemy.transform.position; // Calculate the direction to the player
        if(Physics.Raycast(enemy.transform.position,dirToPlayer,out RaycastHit hit))
        {
            if(hit.transform == enemy.player || hit.transform.parent == enemy.player) // Check if the raycast hit the player or the player's parent (in case of a child object)
            {
                return true; // Player is in clear sight
            }
        }
        return false;
    }
    private bool IsPlayerClose()
    {
        return Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.safeDistance; // Check if the player is within the aggressive range of the enemy
    }
    private void ChangeCoverToShoot()
    {
        if (enemy.coverPerk != CoverPerk.CanTakeAndChangeCover) return; // If the enemy cannot change cover, do nothing
        coverCheckTimer -= Time.deltaTime; // Decrease the cover check timer
        if (coverCheckTimer < 0)
        {
            coverCheckTimer = 0.5f; //check cover moi 0.5s
            if (ReadyToChangeCover() && ReadyToLeaveCover())
            {
                if (enemy.CanGetCover())
                {
                    stateMachine.ChangeState(enemy.runToCoverState); // Change to run to cover state if the player is in clear sight
                }
            }
        }

    }
    private bool ReadyToChangeCover()
    {
        bool inDanger = IsPlayerInClearSight() || IsPlayerClose(); // Check if the player is in clear sight or close to the enemy
        bool advanceTimeOver = Time.time > enemy.advancePlayerState.lastTimeAdvance + enemy.advanceTime;
        return inDanger && advanceTimeOver; // Return true if the enemy is in danger and has advanced towards the player for a certain amount of time
    }
    private bool ReadyToLeaveCover()
    {
        return Time.time > enemy.minStayInCoverTime + enemy.runToCoverState.lastTimeTookCover; // Check if the enemy has stayed in cover for a minimum amount of time
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    private Enemy enemy; // Reference to the Enemy component
    private EnemyBoss enemyBoss; // Reference to the EnemyBoss component, if needed
    //private Enemy_Melee enemyMelee;
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>(); // Get the Enemy component attached to the same GameObject
        //enemyMelee = GetComponentInParent<Enemy_Melee>(); // Get the Enemy_Melee component if it exists
        enemyBoss = GetComponentInParent<EnemyBoss>(); // Get the EnemyBoss component if it exists
    }
    public void AnimationTrigger() => enemy.AnimationTrigger(); // Call the AnimationTrigger method on the Enemy component
    public void StartManualMovement() => enemy.ActiveManualMovement(true);
    public void StopManualMovement() => enemy.ActiveManualMovement(false); // Stop manual movement when the animation ends
    public void StartManualRotation() => enemy.ActiveManualRotation(true); // Start manual rotation when the animation starts
    public void StopManualRotation() => enemy.ActiveManualRotation(false); // Stop manual rotation when the animation ends
    public void AbilityEvent() => enemy.AbilityTrigger();
    public void BossJumpImpact()
    {
        if(enemyBoss == null)
        {
            enemyBoss = GetComponentInParent<EnemyBoss>(); // Get the EnemyBoss component if it exists
        }
        enemyBoss?.JumpImpact();
    }
    public void BeginMeleeAttackCheck() {
       
        enemy?.EnableAttackCheck(true); // Call the BeginMeleeAttackCheck method on the Enemy_Melee component

        AudioManager.Instance.PlaySFX(1); // Play the melee attack sound effec

    }
    public void EndMeleeAttackCheck()
    {
        enemy?.EnableAttackCheck(false); // Call the EndMeleeAttackCheck method on the Enemy_Melee component
    }
}   
   


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Visuals : MonoBehaviour
{
    private EnemyBoss enemyBoss; // Reference to the EnemyBoss component
    [SerializeField] private ParticleSystem landindZone;
    [SerializeField] private GameObject[] weaponTrail;
    private void Awake()
    {
        enemyBoss = GetComponent<EnemyBoss>(); // Get the EnemyBoss component attached to this GameObject
        landindZone.transform.parent = null;
        landindZone.Stop(); // Ensure the landing zone particle system is stopped initially
    }
    public void PlaceLandingZone(Vector3 target)
    {
        Vector3 fixedPosition = target + new Vector3(0, 0.1f, 0);

        landindZone.transform.position = fixedPosition;
        landindZone.Clear();
        var mainModule = landindZone.main; // Get the main module of the particle system
        mainModule.startLifetime = enemyBoss.timeToTarget * 2; // Set the duration of the particle system to the jump attack duration
        landindZone.Play(); // Start the particle system
    }
    public void EnableWeaponTrail(bool active)
    {
        if(weaponTrail.Length <= 0)
        {
            Debug.LogWarning("No weapon trails assigned to the boss visuals.");
            return;
        }
        foreach (var trail in weaponTrail)
        {
            trail.gameObject.SetActive(active);
        }
    }

}

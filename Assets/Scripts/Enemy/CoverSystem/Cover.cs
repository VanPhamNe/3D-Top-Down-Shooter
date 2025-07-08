using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    [Header("CoverPoint")]
    [SerializeField] private GameObject coverPointPrefabs;
    [SerializeField] private List<CoverPoint> coverPoints = new List<CoverPoint>(); // List of cover points in the scene
    [SerializeField] private float xOffset = 1; // Offset for the cover point position on the x-axis
    [SerializeField] private float yOffset = .2f; // Offset for the cover point position on the y-axis
    [SerializeField] private float zOffset = 1; // Offset for the cover point position on the z-axis
    private Transform playerTransform;

    private void Start()
    {
       GenerateCoverPoint(); // Generate cover points when the script starts
        playerTransform = FindObjectOfType<Player>().transform; // Find the player transform in the scene
    }
    private void GenerateCoverPoint()
    {
        Vector3[] localcoverPoint =
        {
            new Vector3(0,yOffset,zOffset), //Truoc
            new Vector3(0,yOffset,-zOffset), //Sau
            new Vector3(xOffset,yOffset,0), //Phai
            new Vector3(-xOffset,yOffset,0), //Trai
        };
        foreach (Vector3 localPoint in localcoverPoint)
        {
            Vector3 worldPoint = transform.TransformPoint(localPoint); // Convert local position to world position
            CoverPoint coverPoint = Instantiate(coverPointPrefabs, worldPoint, Quaternion.identity,transform).GetComponent<CoverPoint>(); // Instantiate a new cover point
            coverPoints.Add(coverPoint); // Add the cover point to the list
        }
    }
    public List<CoverPoint> GetValidCoverPoints(Transform enemyTransform)
    {
        List<CoverPoint> validCoverPoints = new List<CoverPoint>(); // List to store valid cover points
        foreach (CoverPoint coverPoint in coverPoints)
        {
            if(IsCoverPointValid(coverPoint, enemyTransform)) // Check if the cover point is valid
            {
                validCoverPoints.Add(coverPoint); // Add the cover point to the valid list
            }
        
            
        }
        return validCoverPoints; // Return the list of cover points
    }
    private bool IsCoverPointValid(CoverPoint coverPoint,Transform enemyTransform)
    {
        if(coverPoint.occupied) // Check if the cover point is occupied
        {
            return false; // If occupied, return false
        }
        if (IsFurtherFromPlayer(coverPoint) == false)
        {
            return false; // If further from the player, return false
        }
        if (IsCoverBehindPlayer(coverPoint, enemyTransform))
        {
            return false; // If behind the player, return false
        }
        if (IsCoverCloseToPlayer(coverPoint))
        {
            return false; // If too close to the player, return false
        }
        if (IsCoverCloseToLastCover(coverPoint, enemyTransform))
        {
            return false; // If too close to the last cover point, return false
        }
     
        return true; // If not occupied, return true
    }
    private bool IsCoverBehindPlayer(CoverPoint coverPoint,Transform enemyTransform)
    {
        float distanceToPlayer = Vector3.Distance(coverPoint.transform.position, playerTransform.position ); // Calculate distance to player
        float distanceToEnemy = Vector3.Distance(coverPoint.transform.position, enemyTransform.position); // Calculate distance to enemy
        return distanceToPlayer < distanceToEnemy; // Return true if the cover point is closer to the player than to the enemy
    }
    private bool IsCoverCloseToPlayer(CoverPoint coverPoint)
    {
        return Vector3.Distance(coverPoint.transform.position, playerTransform.position) < 2f; // Check if the cover point is close to the player
    }
    private bool IsCoverCloseToLastCover(CoverPoint coverPoint,Transform enemyTransform)
    {
        CoverPoint lastCover = enemyTransform.GetComponent<EnemyRange>().currentCover; // Get the last cover point used by the enemy
        return lastCover != null && Vector3.Distance(coverPoint.transform.position, lastCover.transform.position) < 3f; // Check if the cover point is close to the last cover point
    }
    private bool IsFurtherFromPlayer(CoverPoint coverPoint)
    {
        CoverPoint furtherestPoint = null;
        float furthestDistance = 0f; // Initialize the furthest distance to 0
        foreach (CoverPoint point in coverPoints)
        {
            float distance = Vector3.Distance(point.transform.position, playerTransform.position); // Calculate distance to player
            if (distance > furthestDistance) // Check if this point is further than the current furthest point
            {
                furthestDistance = distance; // Update the furthest distance
                furtherestPoint = point; // Update the furtherest point
            }
        }
        return furtherestPoint == coverPoint; // Return true if the cover point is the furtherest from the player

    }
}

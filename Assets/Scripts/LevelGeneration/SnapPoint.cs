using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SnapPointType
{
   Enter,Exit
}
public class SnapPoint : MonoBehaviour
{
   public SnapPointType pointType;
    private void Start()
    {
        GetComponent<BoxCollider>().enabled = false; // Disable the collider to prevent unwanted interactions
        GetComponent<MeshRenderer>().enabled = false; // Disable the mesh renderer to hide the snap point
    }
    private void OnValidate()
    {
        gameObject.name = " Snap Point -"+ pointType.ToString() ;
    }
}

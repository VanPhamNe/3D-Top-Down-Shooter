using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollParent;
    [SerializeField] private Collider[] ragdollColliders;
    [SerializeField] private Rigidbody[] ragdollRigidbodies;
    private void Awake()
    {
        ragdollColliders = ragdollParent.GetComponentsInChildren<Collider>();
        ragdollRigidbodies = ragdollParent.GetComponentsInChildren<Rigidbody>();
        RagdollActive(false); // Bat dau voi ragdoll khong hoat dong
    }
    public void RagdollActive(bool active)
    {
        foreach(Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = !active; // dat isKinematic cho rigidbody de bat hoat dong ragdoll
        }
    }
}

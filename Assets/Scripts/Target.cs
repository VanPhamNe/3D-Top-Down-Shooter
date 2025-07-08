using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy"); //Dat layer cho target
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

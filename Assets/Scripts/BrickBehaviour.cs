using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehaviour : MonoBehaviour
{
    Rigidbody rigidbody;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    public void removeBrick() {
        rigidbody.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

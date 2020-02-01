using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehaviour : MonoBehaviour
{
    Rigidbody rigidbody;
    public Vector2 wallPos;
    public WallBehaviour wall;

    public Material inWallMaterial;
    public Material outWallMaterial;

    private bool removed = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    public void removeBrick() {
        removeBrick(transform.position + new Vector3(0, 0, 1), 5);
    }
    public void removeBrick(Vector3 fistPos, float fistSize) {
        rigidbody.isKinematic = false;
        
        rigidbody.AddExplosionForce(-25*rigidbody.mass, fistPos, fistSize + 1, 0, ForceMode.Impulse);
        //rigidbody.AddForce(new Vector3(0, 0, -15*rigidbody.mass), ForceMode.Impulse);
        removed = true;
        
        wall.brickPositions[(int)wallPos[0],(int)wallPos[1]] = null;

        GetComponentInChildren<MeshRenderer>().material = outWallMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (removed) {
            
        }
    }
}

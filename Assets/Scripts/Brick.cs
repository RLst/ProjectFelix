using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    Rigidbody rigidbody;

    public Vector2 wallPos;
    internal Wall wall;

    public ParticleSystem cloudParticlePrefab;

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
        
        rigidbody.AddExplosionForce(10*rigidbody.mass, fistPos, fistSize + 0.5f, 0, ForceMode.Impulse);
        //rigidbody.AddForce(new Vector3(0, 0, -15*rigidbody.mass), ForceMode.Impulse);
        removed = true;
        
        wall.removeBrick(new Vector2((int)wallPos[0], (int)wallPos[1]));

        GetComponentInChildren<MeshRenderer>().material = outWallMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (removed) {
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        
        if (collision.impulse.magnitude > 20) {
            Instantiate(cloudParticlePrefab, contact.point, Quaternion.identity);
        }
    }
}

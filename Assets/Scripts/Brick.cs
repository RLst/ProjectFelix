﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Brick : MonoBehaviour
{
	public Vector2 wallPos;
	internal Wall wall;

	public ParticleSystem cloudParticlePrefab;

	public Material inWallMaterial;
	public Material outWallMaterial;

	public Transform brickModel;

	public int rocky; //Int value between 0 and 2 inclusive, 0 least rocky 2 is most

	public bool removed { get; private set; } = false;

	internal new Rigidbody rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.isKinematic = true;
		rocky = 0;
	}

	public void removeBrick()
	{
		removeBrick(transform.position + new Vector3(0, 0, 1), 0);
	}
	public void removeBrick(Vector3 fistPos, float fistSize)
	{
		rigidbody.isKinematic = false;

		rigidbody.AddExplosionForce(10 * rigidbody.mass, fistPos, fistSize + 0.5f, 0, ForceMode.Impulse);
		//rigidbody.AddForce(new Vector3(0, 0, -15*rigidbody.mass), ForceMode.Impulse);
		removed = true;

		wall.removeBrick(new Vector2((int)wallPos[0], (int)wallPos[1]));

		GetComponentInChildren<MeshRenderer>().material = outWallMaterial;
	}

	public void reInsertBrick() {
		rigidbody.isKinematic = true;
		removed = false;
		GetComponentInChildren<MeshRenderer>().material = inWallMaterial;
	}

	// Update is called once per frame
	void Update()
	{
		if (!removed) {
			if (rocky == 1)
			{
				brickModel.localPosition = new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f), 0);
			}
			else if (rocky == 2)
			{
				brickModel.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		ContactPoint contact = collision.contacts[0];

		if (collision.impulse.magnitude > 20)
		{
			Instantiate(cloudParticlePrefab, contact.point, Quaternion.identity);
		}
	}
}

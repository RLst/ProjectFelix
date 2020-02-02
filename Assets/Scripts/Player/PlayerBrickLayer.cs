using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace ProjectFelix
{
	public class PlayerBrickLayer : MonoBehaviour
	{
		[SerializeField] Transform brickDetector = null;
		[SerializeField] int backpackSize = 3;

		[Header("Repair")]
		[SerializeField] float repair;

		[Header("Throw")]
		[SerializeField] float armsLength = 1f;
		[SerializeField] float maxThrowForce = 30f;
		[SerializeField] float maxThrowAngle = 60f;
		[SerializeField] float maxThrowRate = 0.5f;
		[SerializeField] ForceMode throwForceMode = ForceMode.Force;
		float lastThrowTime;

		[Header("Events")]
		public UnityEvent onPickup = null;
		public UnityEvent onThrow = null;
		public UnityEvent onLayBrick = null;

		//Members
		PlayerMover mover;
		PlayerInputActions input;
		Queue<Brick> brickBackPack = new Queue<Brick>();


		//Core
		void Awake()
		{
			input = new PlayerInputActions();
			mover = GetComponent<PlayerMover>();
		}
		void OnEnable()
		{
			input.Gameplay.Enable();
			input.Gameplay.Use.started += PickupBrick;
			input.Gameplay.Throw.started += PrepareThrow;
			input.Gameplay.Throw.performed += ThrowBrick;
		}
		void OnDisable() => input.Gameplay.Disable();

		/// <summary>
		/// Pickup or catch a brick
		/// </summary>
		/// <param name="ctx"></param>
		void PickupBrick(InputAction.CallbackContext ctx)
		{
			print("pickup");

			if (brickBackPack.Count == 0) {
				//Check if dropped brick detected
				var hits =
					(Physics.OverlapBox(
						brickDetector.position,
							brickDetector.localScale * 0.5f,
								brickDetector.rotation));

				//Filter out dropped bricks
				var hitBricks = hits.
					Select(x => x.GetComponent<Brick>()).
						Where(x => x != null && x.removed).
							ToArray();

				//Get the closest brick
				var closestBrick = hitBricks.
					OrderBy(x => Vector3.SqrMagnitude(x.transform.position - transform.position)).
						FirstOrDefault();

				//Load backpack if enough capacity
				if (closestBrick)
				{
					if (brickBackPack.Count < backpackSize)
					{
						brickBackPack.Enqueue(closestBrick);
						closestBrick.gameObject.SetActive(false);
					}
				}
			} else {
				LayBrick();
			}
		}

		void PrepareThrow(InputAction.CallbackContext ctx)
		{
			print("prepare");	

			if (brickBackPack.Count > 0) {
				//Pause player movement if brick found in order to allow aim
				mover.enabled = false;
			}
		}

		void ThrowBrick(InputAction.CallbackContext ctx)
		{
			print("throw");

			///If there are bricks in the back pack then throw brick in movement direction
			//Always resume movement
			mover.enabled = true;

			//Check for available bricks
			if (brickBackPack.Count == 0) return;

			//Throw with timed delay and based on charge time
			//Aim too small to fire just shoot up
			var aim = Vector3.Normalize(input.Gameplay.Move.ReadValue<Vector2>());
			if (Mathf.Approximately(Vector3.SqrMagnitude(aim), 0)) aim = Vector3.up;

			//Limit rate of fire
			if (Time.time - lastThrowTime < maxThrowRate) return;	
			lastThrowTime = Time.time;

			//Can finally throw
			var brick = brickBackPack.Dequeue();
			brick.gameObject.SetActive(true);
			brick.transform.position = transform.position + Vector3.up * armsLength + aim * armsLength;
			brick.rigidbody.AddForce(aim * maxThrowForce, throwForceMode);
		}

		void LayBrick()
		{
			print("lay");
			//Check if a brick is available in backpack
			Brick brick = brickBackPack.Dequeue();
			brick.gameObject.SetActive(true);
			if (!Wall.current.insertBrickAtClosest(transform.position, armsLength, brick)) {
				brickBackPack.Enqueue(brick);
				brick.gameObject.SetActive(false);
			} else {
				//brick.transform.SetParent(transform);
				print("lay for real");
			}
		}

		#region Debug
		void OnDrawGizmos()
		{
			if (!brickDetector) return;

			Gizmos.color = new Color(1, 0.5f, 0, 0.5f);
			Gizmos.DrawCube(brickDetector.position, brickDetector.localScale);
		}
		#endregion
	}
}

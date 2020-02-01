using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

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
		void Start()
		{
			input.Gameplay.Use.started += PickupBrick;
			input.Gameplay.Use.performed += ThrowBrick;
		}
		void OnEnable() => input.Gameplay.Enable();
		void OnDisable() => input.Gameplay.Disable();

		void Update()
		{
		}


		/// <summary>
		/// Pickup or catch a brick
		/// </summary>
		/// <param name="ctx"></param>
		void PickupBrick(InputAction.CallbackContext ctx)
		{
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

			//Pause player movement if brick found in order to allow aim
			mover.enabled = false;

			//Load backpack if enough capacity
			if (closestBrick)
			{
				if (brickBackPack.Count < backpackSize)
				{
					brickBackPack.Enqueue(closestBrick);
					closestBrick.gameObject.SetActive(false);
				}
			}
		}



		void ThrowBrick(InputAction.CallbackContext ctx)
		{
			///If there are bricks in the back pack then throw brick in movement direction
			//Always resume movement
			mover.enabled = true;

			//Check for available bricks
			if (brickBackPack.Count == 0) return;

			//Throw with timed delay and based on charge time
			var direction = (input.Gameplay.Move.ReadValue<Vector2>()).normalized;
			var dir3 = new Vector3(direction.x, direction.y, 0);
			//if (Vector2.SignedAngle(Vector2.up, direction) > maxThrowAngle)
			var brick = brickBackPack.Dequeue();
			brick.gameObject.SetActive(true);
			brick.transform.position = transform.position + dir3 * armsLength;
			brick.rigidbody.AddForce(dir3 * maxThrowForce, throwForceMode);

			//

		}

		void LayBrick(InputAction.CallbackContext ctx)
		{

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

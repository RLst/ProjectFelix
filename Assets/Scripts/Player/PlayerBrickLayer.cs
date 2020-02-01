using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ProjectFelix
{
	public class PlayerBrickLayer : MonoBehaviour
	{
		[SerializeField] Transform brickDetector = null;

		[Header("Repair")]
		[SerializeField] float repair;

		[Header("Throw")]
		[SerializeField] float throwSpeed = 30f;
		[SerializeField] float maxThrowAngle = 60f;

		//Members
		PlayerInputActions input;

		//Core
		void Awake()
		{
			input = new PlayerInputActions();
		}
		void OnEnable() => input.Gameplay.Enable();
		void OnDisable() => input.Gameplay.Disable();
		void Update()
		{
			if (input.Gameplay.Use.triggered)
				PickupBrick();

			//HandleThrowing();
			//HandleBrickLaying();
		}

		public Brick PickupBrick()
		{
			print("Picking up");

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

			if (closestBrick) closestBrick.transform.position = new Vector3(0, 15, 0);
			return closestBrick;
		}

		//private void HandleBrickLaying()
		//{
		//	throw new NotImplementedException();
		//}

		//private void HandleThrowing()
		//{
		//	//

		//}

		void OnDrawGizmos()
		{
			if (!brickDetector) return;

			Gizmos.color = new Color(1, 0.5f, 0, 0.5f);
			Gizmos.DrawCube(brickDetector.position, brickDetector.localScale);
		}
	}
}

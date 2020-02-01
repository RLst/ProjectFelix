using UnityEngine;

namespace ProjectFelix
{
	public class Player : MonoBehaviour
	{
		///Inspector
		[Header("Move")]
		[SerializeField] float maxSpeed = 5f;
		[SerializeField] float smoothing = 0.5f;

		[Header("Dash")]
		[SerializeField] float dashMaxSpeed = 10f;
		[SerializeField] float dashDrag = 1.5f;
		[SerializeField] float gravity = 3.5f;
		[SerializeField] float jumpSpeed = 23.5f;

		[Header("Tossing")]
		[SerializeField] float tossMaxForce = 40f;
		[SerializeField] float tossMaxAngle = 45f;

		//Public accessible
		[HideInInspector] public Vector3 currentMoveVector = Vector3.zero;
		[HideInInspector] public bool isClimbing;// { get; private set; }

		///Members
		BrickDetector bd = null;
		CharacterController cc = null;
		PlayerInputActions input;
		float currentDashSpeed;
		Vector3 verticalMotion;
		bool isDashing = false;
		bool isFalling = false;
		bool prevDash = false;
		bool dash = false;
		bool dashed = false;

		void Awake()
		{
			bd = GetComponentInChildren<BrickDetector>();
			cc = GetComponent<CharacterController>();
			cc.enableOverlapRecovery = true;	//Maybe?
			input = new PlayerInputActions();
		}

		//Core
		void OnEnable() => input.Gameplay.Enable();
		void OnDisable() => input.Gameplay.Disable();
		void Update()
		{
			CheckIfClimbing();
			Move();
		}
		void LateUpdate() => FinalMove();

		void CheckIfClimbing()
		{
			if (!cc.isGrounded)
				isClimbing = bd.isColliding;
			else
				isClimbing = false;
		}

		/// <summary>
		/// Climb, dash/jump, fall
		/// </summary>
		void Move()
		{
			var speedTotal = maxSpeed;
			dash = input.Gameplay.Dash.triggered;

			//Calculate dash boost
			if (isDashing)
			{
				//Apply dash drag
				currentDashSpeed -= dashDrag;

				//Reset dash if speed is too slow
				if (currentDashSpeed < 0)
				{
					isDashing = false;
					currentDashSpeed = 0;
				}
			}
			if (dash && isDashing == false)
			{
				isDashing = true;
				currentDashSpeed = dashMaxSpeed;
			}
			speedTotal += currentDashSpeed;

			//you're falling if you're not grounded or climbing
			isFalling = !(cc.isGrounded || isClimbing);

			//Apply gravity if you're falling
			verticalMotion.y = isFalling ? verticalMotion.y - gravity : 0;

			//Movement
			var m = input.Gameplay.Move.ReadValue<Vector2>();
			Vector3 destMoveVector = new Vector3(m.x * speedTotal, m.y * speedTotal + verticalMotion.y, 0);

			//Final
			currentMoveVector = Vector3.Lerp(currentMoveVector, destMoveVector, smoothing);
		}

		void FinalMove() => cc.Move(currentMoveVector * Time.deltaTime);

		void OnGUI()
		{
			if (isClimbing) GUILayout.Label("Climbing...");
			if (isDashing) GUILayout.Label("Dashing...");
			if (isFalling) GUILayout.Label("Falling...");
			if (cc.isGrounded) GUILayout.Label("Grounded...");
		}
	}
}
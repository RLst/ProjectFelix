using UnityEngine;

namespace ProjectFelix
{
	public class PlayerMover : MonoBehaviour
	{
		///Inspector
		[Header("Move")]
		[SerializeField] float maxSpeed = 5f;
		[SerializeField] float smoothing = 0.5f;

		[Header("Dash")]
		[SerializeField] float dashMaxSpeed = 10f;
		[SerializeField] float dashDrag = 1.5f;
		[SerializeField] float gravity = 0.5f;

		//Public accessible
		[HideInInspector] public Vector3 currentMoveVector = Vector3.zero;
		public bool isClimbing;// { get; private set; }

		///Members
		ClimbDetector bd = null;
		CharacterController cc = null;
		PlayerInputActions input;
		float currentDashSpeed;
		Vector3 verticalMotion;
		public bool isDashing = false;
		public bool isFalling = false;

		void Awake()
		{
			bd = GetComponentInChildren<ClimbDetector>();
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
			HandleMovement();
		}
		void LateUpdate() => FinalMove();

		void CheckIfClimbing() => isClimbing = bd.isClimbing;

		/// <summary>
		/// Climb, dash/jump, fall
		/// </summary>
		void HandleMovement()
		{
			var speedTotal = maxSpeed;
			var dash = input.Gameplay.Dash.triggered;

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
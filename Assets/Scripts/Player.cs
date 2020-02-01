using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace ProjectFelix
{
	public class Player : MonoBehaviour
	{
		//Inspector
		[SerializeField] float speed = 5f;
		[SerializeField] float dashSpeed = 10f;
		[SerializeField] float smoothing = 0.5f;

		//Members
		PlayerInput input;


		Vector3 desPos;
		Vector3 pos;
		Vector2 desAim;

		InputAction movement;
		InputAction use;
		InputAction dash;
		InputAction toss;

		void Awake()
		{
			input = GetComponent<PlayerInput>();
		}

		void Start()
		{
			//Set initial position
			desPos = transform.position;
		}

		void OnEnable()
		{
			movement = input.actions.FindAction("Move");
			use = input.actions.FindAction("Use");
			dash = input.actions.FindAction("Dash");
			toss = input.actions.FindAction("Toss");
		}

		void OnDisable()
		{
			movement = null; use = null; dash = null; toss = null;
		}

		void Update()
		{
			HandleMove();
		}

		void LateUpdate()
		{
			FinalMove();
		}

		void HandleMove()
		{
			var m = movement.ReadValue<Vector2>();

			//Movement
			desPos += new Vector3(m.x, m.y, 0) * Time.deltaTime * speed;
			pos = Vector3.Lerp(pos, desPos, smoothing);

		}

		void FinalMove()
		{
			transform.position = pos;
		}
	}
}


//void tReadInput()
//{
//	inMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
//	inUse = Input.GetKey(useKey);
//	inDash = Input.GetKey(dashKey);
//	inThrow = Input.GetKey(throwKey);
//}
//void OnMove(CallbackContext context) => inMovement = context.ReadValue<Vector2>();
//void OnUse(CallbackContext context) => context.ReadValue<bool>();
//void OnDash(CallbackContext context) => context.ReadValue<bool>();
//void OnThrow(CallbackContext context) => context.ReadValue<bool>();

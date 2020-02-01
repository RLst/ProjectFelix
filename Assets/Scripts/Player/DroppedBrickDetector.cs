using UnityEngine;

namespace ProjectFelix
{
	public class DroppedBrickDetector : MonoBehaviour
	{
		//Properties
		public bool brickFound { get; private set; } = false;
		public Brick getClosestBrick
		{
			get
			{
				return null;
			}
		}

		//Members
		Collider col;

		void Awake()
		{
			//input = new PlayerInputActions()
			col = GetComponent<Collider>();
			if (col) col.isTrigger = true;
		}

		void OnTriggerStay(Collider col)
		{
			var brickIsHit = col.GetComponent<Brick>();
			if (brickIsHit)
			{
				//Get closest brick
			}
		}
	}
}
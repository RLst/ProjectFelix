using UnityEngine;

namespace ProjectFelix
{
	public class ClimbDetector : MonoBehaviour
	{
		//Properties
		[HideInInspector] public bool isClimbing;// { get; private set; }

		//Members
		Collider col = null;

		void Awake()
		{
			col = GetComponent<Collider>();
			if (col) col.isTrigger = true;	//Always set trigger
		}

		void OnTriggerStay(Collider col)
		{
			var hit = col.GetComponent<Brick>();
			isClimbing = (hit) ? true : false;
		}
	}
}

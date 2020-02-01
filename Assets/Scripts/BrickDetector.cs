using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectFelix
{
	public class BrickDetector : MonoBehaviour
	{
		//Properties
		public bool isColliding;// { get; private set; }

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
			isColliding = (hit) ? true : false;
		}
	}
}

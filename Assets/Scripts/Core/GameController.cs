using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectFelix
{
    public class GameController : MonoBehaviour
    {
		public static GameController current;

		void Awake()
		{
			if (!GameController.current)
				current = this;
		}
    }
}

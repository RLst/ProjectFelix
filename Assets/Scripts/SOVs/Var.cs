using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectFelix.Scriptables
{
	public abstract class Var<T> : ScriptableObject
	{
		public T value;
	}
}

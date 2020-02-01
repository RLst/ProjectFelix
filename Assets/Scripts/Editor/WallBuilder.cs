using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectFelix.Editors
{
	[CustomEditor(typeof(Wall))]
	public class WallEditor : Editor
	{
		Wall w;

		void OnEnable()
		{
			w = target as Wall;
			w.brickPositions = new Brick[w.nBricksWide, w.nBricksTall];
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			GUILayout.Space(3);

			//Create wall button
			if (GUILayout.Button("Create Wall"))
				w.generateWall();

			//Delete wall button
			//if (GUILayout.Button("Delete Wall"))
			//	foreach (var x in w.brickPositions)
			//		DestroyImmediate(x);
		}


	}
}

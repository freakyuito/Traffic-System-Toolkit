using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TrafficSystem.Builder
{
	[CustomEditor (typeof(BezierHandle))]
	public class BezierHandleEditor : Editor
	{

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

		}
	}
}


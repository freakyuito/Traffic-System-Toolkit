using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficSystem.Builder
{
	public class BezierHandle : MonoBehaviour
	{
		//		public int handleId;
		//		public GameObject parent = null;
		//
		//		public BezierHandle (int _handleId, GameObject _parent)
		//		{
		//			handleId = _handleId;
		//			parent = _parent;
		//		}
		//
		//		public BezierHandle ()
		//		{
		//
		//		}

		void OnDrawGizmos ()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere (transform.position, 0.5f);
		}
	}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficSystem.Base
{
	public class Node : MonoBehaviour
	{
		[SerializeField] private int nodeId;
		[SerializeField] private GameObject parentLane;

		public GameObject ParentLane {
			get{ return parentLane; }
			set{ parentLane = value; }
		}

		public int NodeID {
			get{ return nodeId; }
			set{ nodeId = value; }
		}

		void OnDrawGizmos ()
		{
			Gizmos.color = Color.white;
			Gizmos.DrawSphere (transform.position, 0.25f);
		}
	}
}


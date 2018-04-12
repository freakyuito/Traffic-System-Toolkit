using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TrafficSystem.Base
{
	public class Lane : MonoBehaviour
	{
		[SerializeField] private int laneId;
		[SerializeField] private DataStruct.LaneType laneType;
		[SerializeField] private List<GameObject> childrenNodes = new List<GameObject> ();
		[SerializeField] private int laneDiv;

		public int LaneID {
			get{ return laneId; }
			set{ laneId = value; }
		}

		public DataStruct.LaneType LaneType {
			get{ return laneType; }
			set{ laneType = value; }
		}

		public List<GameObject> ChildrenNodes {
			get{ return childrenNodes; }
			set{ childrenNodes = value; }
		}

		public int LaneDiv {
			get{ return laneDiv; }
			set{ laneDiv = value; }
		}
	}
}


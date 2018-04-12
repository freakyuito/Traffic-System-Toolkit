using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficSystem.Builder
{
	public class LaneIndicator : MonoBehaviour
	{
		public int laneDiv;
		public int laneId;
		public GameObject parent = null;

		public LaneIndicator (int _laneDiv, int _laneId, GameObject _parent)
		{
			laneDiv = _laneDiv;
			laneId = _laneId;
			parent = _parent;
		}

		public LaneIndicator ()
		{
			
		}

	}
}


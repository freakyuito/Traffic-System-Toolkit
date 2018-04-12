using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficSystem.Base
{
	public class Intersection : MonoBehaviour
	{
		[SerializeField] private List<GameObject> prevLanes, nextLanes = new List<GameObject> ();

		public List<GameObject> PrevLanes {
			get{ return prevLanes; }
		}

		public List<GameObject> NextLanes {
			get{ return nextLanes; }
		}
	}
}


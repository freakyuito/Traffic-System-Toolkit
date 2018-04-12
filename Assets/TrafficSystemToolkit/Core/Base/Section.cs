using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficSystem.Base
{
	public class Section : MonoBehaviour
	{
		[SerializeField] private GameObject prevLane, nextLane = null;
		[SerializeField] private GameObject fromLane, toLane = null;
		[SerializeField] private int seq;

		public GameObject PrevLane {
			get{ return prevLane; }
			set{ prevLane = value; }
		}

		public GameObject NextLane {
			get{ return nextLane; }
			set{ nextLane = value; }
		}

		public GameObject FromLane {
			get{ return fromLane; }
			set{ fromLane = value; }
		}

		public GameObject ToLane {
			get{ return toLane; }
			set{ toLane = value; }
		}

		public int Sequence {
			get{ return seq; }
			set{ seq = value; }
		}
	}
}


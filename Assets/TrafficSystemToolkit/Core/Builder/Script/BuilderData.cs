using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using TrafficSystem.IOStreamer;

namespace TrafficSystem.Builder
{
	[ExecuteInEditMode]
	public class BuilderData : MonoBehaviour
	{
		public static BuilderData Data = null;

		public Transform builderNodeParent, sectionParent, handleParent;

		public List<BuilderNode> builderNodes = new List<BuilderNode> ();
		public List<List<int>> linkGraph = new List<List<int>> ();
		public List <BuilderCore.LineSet> lineSetMap = new List<BuilderCore.LineSet> ();
		public List<BuilderCore.BezierSegment> bezierSetMap = new List<BuilderCore.BezierSegment> ();

		public List<LaneIndicator> laneIndicators = new List<LaneIndicator> ();

		public List<SectionNode> sectionNodes = new List<SectionNode> ();

		public List<GameObject> bezierHandles = new List<GameObject> ();

		public Mesh builderNodeMesh = null;

		//		public List<BuilderNode> BuilderNodes {
		//			get{ return builderNodes; }
		//		}
		//
		//		public List<List<int>> LinkGraph {
		//			get{ return linkGraph; }
		//		}

		void OnEnable ()
		{
			Data = this;
			if (GameObject.Find ("Builder Nodes") == null) {
				builderNodeParent = new GameObject ("Builder Nodes").transform;
			} else {
				builderNodeParent = GameObject.Find ("Builder Nodes").transform;
			}

			if (GameObject.Find ("Sections") == null) {
				sectionParent = new GameObject ("Sections").transform;
			} else {
				sectionParent = GameObject.Find ("Sections").transform;
			}

			if (GameObject.Find ("Handles") == null) {
				handleParent = new GameObject ("Handles").transform;
			} else {
				handleParent = GameObject.Find ("Handles").transform;
			}
		}

		void OnDrawGizmos ()
		{
			for (int i = 0; i < linkGraph.Count; i++) {
				for (int j = 0; j < linkGraph [i].Count - 1; j++) {
					if (linkGraph [i] [j] != 999) {
						Gizmos.DrawLine (builderNodes [i].transform.position,
							builderNodes [j].transform.position);
					}
				}
			}
			Gizmos.color = Color.cyan;
			if (bezierSetMap.Count > 0 && sectionNodes.Count > 0) {
				foreach (BuilderCore.BezierSegment bs in bezierSetMap) {
					Gizmos.DrawLine (bs.startNode.transform.position, bs.sectionNodes [0].transform.position);
					for (int i = 0; i < bs.sectionNodes.Count - 1; i++) {
						Gizmos.DrawLine (bs.sectionNodes [i].transform.position, bs.sectionNodes [i + 1].transform.position);
					}
					Gizmos.DrawLine (bs.sectionNodes [bs.sectionNodes.Count - 1].transform.position,
						bs.endNode.transform.position);
				}
				BuilderCore.RefreshBezierSegment ();
			}
		}

		public void ClearData ()
		{
			int count;

			count = builderNodes.Count;
			for (int i = 0; i < count; i++) {
				BuilderCore.DeleteNode (BuilderData.Data.builderNodes [0]);
			}

			count = sectionNodes.Count;
			for (int i = 0; i < count; i++) {
				GameObject o = BuilderData.Data.sectionNodes [0].gameObject;
				sectionNodes.RemoveAt (0);
				DestroyImmediate (o);
			}

			count = BuilderData.Data.bezierHandles.Count;
			for (int i = 0; i < count; i++) {
				GameObject o = BuilderData.Data.bezierHandles [0].gameObject;
				BuilderData.Data.bezierHandles.RemoveAt (0);
				DestroyImmediate (o);
			}
		}

	}
}


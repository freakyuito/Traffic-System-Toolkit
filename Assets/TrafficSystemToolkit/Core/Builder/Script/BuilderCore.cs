using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TrafficSystem.Base;
using TrafficSystem.IOStreamer;
using System.Text.RegularExpressions;

namespace TrafficSystem.Builder
{
	public class BuilderCore : MonoBehaviour
	{
		public class Line
		{
			public string Name { get; set; }

			public Vector3 start = Vector3.zero;

			public Vector3 end = Vector3.zero;

			public Line (Vector3 _start, Vector3 _end)
			{
				start = _start;
				end = _end;
			}

			public float Length {
				get {
					return Vector3.Distance (start, end);
				}
			}
		}

		public class LineSet
		{
			public GameObject startNode;
			public GameObject endNode;


			public LineSet (GameObject _start, GameObject _end)
			{
				startNode = _start;
				endNode = _end;

			}

			public Line GetLine {
				get{ return new Line (startNode.transform.position, endNode.transform.position); }
			}
		}

		public class BezierSegment
		{
			public GameObject startNode;
			public GameObject endNode;
			public GameObject bezierHandle;
			public List<GameObject> sectionNodes = new List<GameObject> ();

			public BezierSegment (GameObject _startNode, GameObject _endNode, GameObject _bezierHandle)
			{
				startNode = _startNode;
				endNode = _endNode;
				bezierHandle = _bezierHandle;
				bezierHandle.transform.position = _startNode.transform.position + (_endNode.transform.position -
				_startNode.transform.position) / 2;
			}
		}

		public enum BuilderNodeType
		{
			Section,
			Intersection
		}

		public static GameObject CreateLaneIndicator (Vector3 _pos, GameObject _parent)
		{
			GameObject newNode = new GameObject ("Lane Indicator " + (BuilderData.Data.laneIndicators.Count + 1).ToString ());
			LaneIndicator newComp = (LaneIndicator)newNode.AddComponent (typeof(LaneIndicator));
	
			newNode.transform.position = _pos;
			newComp.laneId = BuilderData.Data.laneIndicators.Count;
			newComp.laneDiv = 1;
			newComp.parent = _parent;

			return newNode;
		}

		public static Vector3 ClampPointToLine (Vector3 pointToClamp, Line lineToClampTo)
		{
			Vector3 clampedPoint = new Vector3 ();
			float minX, minY, minZ, maxX, maxY, maxZ;
			if (lineToClampTo.start.x <= lineToClampTo.end.x) {
				minX = lineToClampTo.start.x;
				maxX = lineToClampTo.end.x;
			} else {
				minX = lineToClampTo.end.x;
				maxX = lineToClampTo.start.x;
			}
			if (lineToClampTo.start.y <= lineToClampTo.end.y) {
				minY = lineToClampTo.start.y;
				maxY = lineToClampTo.end.y;
			} else {
				minY = lineToClampTo.end.y;
				maxY = lineToClampTo.start.y;
			}
			if (lineToClampTo.start.z <= lineToClampTo.end.z) {
				minZ = lineToClampTo.start.z;
				maxZ = lineToClampTo.end.z;
			} else {
				minZ = lineToClampTo.end.z;
				maxZ = lineToClampTo.start.z;
			}
			clampedPoint.x = (pointToClamp.x < minX) ? minX : (pointToClamp.x > maxX) ? maxX : pointToClamp.x;
			clampedPoint.y = (pointToClamp.y < minY) ? minY : (pointToClamp.y > maxY) ? maxY : pointToClamp.y;
			clampedPoint.z = (pointToClamp.z < minZ) ? minZ : (pointToClamp.z > maxZ) ? maxZ : pointToClamp.z;
			return clampedPoint;
		}

		public static Line DistBetweenLines (Line l1, Line l2)
		{
			Vector3 p1, p2, p3, p4, d1, d2;
			p1 = l1.start;
			p2 = l1.end;
			p3 = l2.start;
			p4 = l2.end;
			d1 = p2 - p1;
			d2 = p4 - p3;
			float eq1nCoeff = (d1.x * d2.x) + (d1.y * d2.y) + (d1.z * d2.z);
			float eq1mCoeff = (-(Mathf.Pow (d1.x, 2)) - (Mathf.Pow (d1.y, 2)) - (Mathf.Pow (d1.z, 2)));
			float eq1Const = ((d1.x * p3.x) - (d1.x * p1.x) + (d1.y * p3.y) - (d1.y * p1.y) + (d1.z * p3.z) - (d1.z * p1.z));
			float eq2nCoeff = ((Mathf.Pow (d2.x, 2)) + (Mathf.Pow (d2.y, 2)) + (Mathf.Pow (d2.z, 2)));
			float eq2mCoeff = -(d1.x * d2.x) - (d1.y * d2.y) - (d1.z * d2.z);
			float eq2Const = ((d2.x * p3.x) - (d2.x * p1.x) + (d2.y * p3.y) - (d2.y * p2.y) + (d2.z * p3.z) - (d2.z * p1.z));
			float[,] M = new float[,] { { eq1nCoeff, eq1mCoeff, -eq1Const }, { eq2nCoeff, eq2mCoeff, -eq2Const } };
			int rowCount = M.GetUpperBound (0) + 1;
			// pivoting
			for (int col = 0; col + 1 < rowCount; col++)
				if (M [col, col] == 0) {				// check for zero coefficients
					// find non-zero coefficient
					int swapRow = col + 1;
					for (; swapRow < rowCount; swapRow++)
						if (M [swapRow, col] != 0)
							break;

					if (M [swapRow, col] != 0) { // found a non-zero coefficient?
						// yes, then swap it with the above
						float[] tmp = new float[rowCount + 1];
						for (int i = 0; i < rowCount + 1; i++) {
							tmp [i] = M [swapRow, i];
							M [swapRow, i] = M [col, i];
							M [col, i] = tmp [i];
						}
					} else
						return null; // no, then the matrix has no unique solution
				}

			// elimination
			for (int sourceRow = 0; sourceRow + 1 < rowCount; sourceRow++) {
				for (int destRow = sourceRow + 1; destRow < rowCount; destRow++) {
					float df = M [sourceRow, sourceRow];
					float sf = M [destRow, sourceRow];
					for (int i = 0; i < rowCount + 1; i++)
						M [destRow, i] = M [destRow, i] * df - M [sourceRow, i] * sf;
				}
			}

			// back-insertion
			for (int row = rowCount - 1; row >= 0; row--) {
				float f = M [row, row];
				if (f == 0)
					return null;

				for (int i = 0; i < rowCount + 1; i++)
					M [row, i] /= f;
				for (int destRow = 0; destRow < row; destRow++) {
					M [destRow, rowCount] -= M [destRow, row] * M [row, rowCount];
					M [destRow, row] = 0;
				}
			}
			float n = M [0, 2];
			float m = M [1, 2];
			Vector3 i1 = new Vector3 (p1.x + (m * d1.x), p1.y + (m * d1.y), p1.z + (m * d1.z));
			Vector3 i2 = new Vector3 (p3.x + (n * d2.x), p3.y + (n * d2.y), p3.z + (n * d2.z));
			Vector3 i1Clamped = ClampPointToLine (i1, l1);
			Vector3 i2Clamped = ClampPointToLine (i2, l2);
			return new Line (i1Clamped, i2Clamped);
		}

		public static GameObject Extrude (BuilderNode _startNode)
		{
			BuilderNode endNode = AddBuilderNode (_startNode.transform.position + Vector3.forward * 1f);
			LinkNode (_startNode.builderNodeId, endNode.builderNodeId);
	
			return endNode.gameObject;
		}

		public static BuilderNode CreateNode (Vector3 _pos)
		{
			if (BuilderData.Data.builderNodes.Count == 0) {
				return CreateRootNode (_pos);
			} else {
				return AddBuilderNode (_pos);
			}
		}

		public static BuilderNode CreateRootNode (Vector3 _position)
		{
			GameObject newNode = new GameObject ("Builder Node " + (BuilderData.Data.builderNodes.Count + 1).ToString (),
				                     new System.Type[]{ typeof(BuilderNode) });
			newNode.transform.SetParent (BuilderData.Data.builderNodeParent);
			newNode.transform.position = _position;
			BuilderNode builderNodeComp = newNode.GetComponent<BuilderNode> ();
			BuilderData.Data.builderNodes.Add (builderNodeComp);

			builderNodeComp.builderNodeId = BuilderData.Data.builderNodes.Count;

			BuilderData.Data.linkGraph.Add (new List<int> ());
			BuilderData.Data.linkGraph [0].Add (0);

			return builderNodeComp;
		}

		public static BuilderNode AddBuilderNode (Vector3 _position)
		{
			GameObject newNode = new GameObject ("Builder Node " + (BuilderData.Data.builderNodes.Count + 1).ToString (),
				                     new System.Type[]{ typeof(BuilderNode) });
			newNode.transform.SetParent (BuilderData.Data.builderNodeParent);
			newNode.transform.position = _position;
			BuilderNode builderNodeComp = newNode.GetComponent<BuilderNode> ();
			BuilderData.Data.builderNodes.Add (builderNodeComp);
			builderNodeComp.builderNodeId = BuilderData.Data.builderNodes.Count;

			int row = BuilderData.Data.linkGraph.Count;

			BuilderData.Data.linkGraph.Add (new List<int> ());

			for (int i = 0; i < row + 1; i++) {
				if (i != row) {
					BuilderData.Data.linkGraph [row].Add (999);
				} else {
					BuilderData.Data.linkGraph [row].Add (0);
				}
			}

			return builderNodeComp;
		}

		public static void LinkNode (int _startId, int _endId)
		{
			if (_startId > _endId) {
				BuilderData.Data.linkGraph [_startId - 1] [_endId - 1] = 1;
			} else {
				BuilderData.Data.linkGraph [_endId - 1] [_startId - 1] = 1;
			}

			Debug.Log (_startId + "->" + _endId);

			RefreshLinkGraph ();
			RefreshLineSetGroup ();
			RefreshSectionNodes ();
		}

		public static void BreakLink (int _startId, int _endId)
		{
			if (_startId > _endId) {
				BuilderData.Data.linkGraph [_startId - 1] [_endId - 1] = 999;
			} else {
				BuilderData.Data.linkGraph [_endId - 1] [_startId - 1] = 999;
			}

			Debug.Log (_startId + "|->|" + _endId);

			RefreshLinkGraph ();
			RefreshLineSetGroup ();
			RefreshSectionNodes ();
		}

		public static void DeleteNode (BuilderNode _node)
		{
			DestroyImmediate (_node.gameObject);
		}

		public static void OnDeleteNode (BuilderNode _node)
		{
			
			BuilderData.Data.builderNodes.RemoveAt (_node.builderNodeId - 1);
			BuilderData.Data.linkGraph.RemoveAt (_node.builderNodeId - 1);
			for (int i = _node.builderNodeId - 1; i < BuilderData.Data.linkGraph.Count; i++) {
				BuilderData.Data.linkGraph [i].RemoveAt (_node.builderNodeId - 1);
			}
			for (int i = _node.builderNodeId - 1; i < BuilderData.Data.linkGraph.Count; i++) {
				BuilderData.Data.builderNodes [i].builderNodeId--;
				BuilderData.Data.builderNodes [i].name = "Builder Node " + BuilderData.Data.builderNodes [i].builderNodeId;
			}

			RefreshLinkGraph ();
			RefreshLineSetGroup ();
		}

		public static void RefreshLinkGraph ()
		{
			for (int i = 0; i < BuilderData.Data.linkGraph.Count; i++) {
				for (int j = 0; j < BuilderData.Data.linkGraph [i].Count - 1; j++) {
					if (BuilderData.Data.linkGraph [i] [j] != 999) {
						BuilderData.Data.linkGraph [i] [j] = 
							(int)Vector3.Distance (BuilderData.Data.builderNodes [i].transform.position,
							BuilderData.Data.builderNodes [j].transform.position);
					}
				}
			}
		}

		public static void MergeIntersection ()
		{
			for (int i = 0; i < BuilderData.Data.lineSetMap.Count; i++) {
				for (int j = i + 1; j < BuilderData.Data.lineSetMap.Count; j++) {
					if (!BuilderData.Data.lineSetMap [i].startNode.Equals (BuilderData.Data.lineSetMap [j].startNode) &&
					    !BuilderData.Data.lineSetMap [i].endNode.Equals (BuilderData.Data.lineSetMap [j].endNode) &&
					    !BuilderData.Data.lineSetMap [i].startNode.Equals (BuilderData.Data.lineSetMap [j].endNode) &&
					    !BuilderData.Data.lineSetMap [i].endNode.Equals (BuilderData.Data.lineSetMap [j].startNode)) {

						Line line = DistBetweenLines (BuilderData.Data.lineSetMap [i].GetLine,
							            BuilderData.Data.lineSetMap [j].GetLine);
						if (line.Length < BuilderTool.Settings.mergeIntersectionThreshold) {
							Debug.Log (BuilderData.Data.lineSetMap [i].startNode.name + "-" + BuilderData.Data.lineSetMap [i].endNode.name
							+ " compares to " + BuilderData.Data.lineSetMap [j].startNode.name + "-" + BuilderData.Data.lineSetMap [j].endNode.name
							+ " distance = " + line.Length);
							BuilderNode newNode = AddBuilderNode (line.end);
							LinkNode (BuilderData.Data.lineSetMap [i].startNode.GetComponent<BuilderNode> ().builderNodeId,
								newNode.builderNodeId);
							LinkNode (BuilderData.Data.lineSetMap [i].endNode.GetComponent<BuilderNode> ().builderNodeId,
								newNode.builderNodeId);
							LinkNode (BuilderData.Data.lineSetMap [j].startNode.GetComponent<BuilderNode> ().builderNodeId,
								newNode.builderNodeId);
							LinkNode (BuilderData.Data.lineSetMap [j].endNode.GetComponent<BuilderNode> ().builderNodeId,
								newNode.builderNodeId);
							int p1_start = BuilderData.Data.lineSetMap [i].startNode.GetComponent<BuilderNode> ().builderNodeId;
							int p1_end = BuilderData.Data.lineSetMap [i].endNode.GetComponent<BuilderNode> ().builderNodeId;
							int p2_start = BuilderData.Data.lineSetMap [j].startNode.GetComponent<BuilderNode> ().builderNodeId;
							int p2_end = BuilderData.Data.lineSetMap [j].endNode.GetComponent<BuilderNode> ().builderNodeId;
							BreakLink (p1_start, p1_end);
							BreakLink (p2_start, p2_end);
							return;
						}
					}
				}
			}
		}

		public static void RemoveDouble ()
		{
		
			for (int i = 0; i < BuilderData.Data.builderNodes.Count; i++) {
				for (int j = i + 1; j < BuilderData.Data.builderNodes.Count; j++) {
					if (Vector3.Distance (BuilderData.Data.builderNodes [i].transform.position, BuilderData.Data.builderNodes [j].transform.position)
					    < BuilderTool.Settings.removeDoubleThreshold) {
						for (int k = 0; k < BuilderData.Data.linkGraph [j].Count - 1; k++) {
							if (BuilderData.Data.linkGraph [j] [k] != 999) {
								LinkNode (i + 1, k + 1);
							}
						}
						for (int k = j + 1; k < BuilderData.Data.builderNodes.Count; k++) {
							if (BuilderData.Data.linkGraph [k] [j] != 999) {
								LinkNode (i + 1, k + 1);
							}
						}
						DeleteNode (BuilderData.Data.builderNodes [j]);
		
						return;
					}
				}
			}
		}

		public static void RefreshLineSetGroup ()
		{
			
			BuilderData.Data.lineSetMap.Clear ();
			for (int i = 0; i < BuilderData.Data.linkGraph.Count; i++) {
				for (int j = 0; j < BuilderData.Data.linkGraph [i].Count - 1; j++) {
					if (BuilderData.Data.linkGraph [i] [j] != 999) {
						
						BuilderCore.LineSet newSet = new LineSet (BuilderData.Data.builderNodes [i].gameObject,
							                             BuilderData.Data.builderNodes [j].gameObject);
						BuilderData.Data.lineSetMap.Add (newSet);
					}

				}
			}
		}

		public static void RefreshSectionNodes ()
		{
			int count = BuilderData.Data.bezierHandles.Count;
			for (int i = 0; i < count; i++) {
				GameObject o = BuilderData.Data.bezierHandles [0].gameObject;
				BuilderData.Data.bezierHandles.RemoveAt (0);
				DestroyImmediate (o);
			}
			count = BuilderData.Data.sectionNodes.Count;
			for (int i = 0; i < count; i++) {
				GameObject o = BuilderData.Data.sectionNodes [0].gameObject;
				BuilderData.Data.sectionNodes.RemoveAt (0);
				DestroyImmediate (o);
			}

			BuilderData.Data.bezierSetMap.Clear ();

			foreach (LineSet ls in BuilderData.Data.lineSetMap) {
				float distance = Vector3.Distance (ls.startNode.transform.position, ls.endNode.transform.position);
				int segment = (int)(distance / BuilderTool.Settings.sectionLength);
				Vector3 dir = ls.endNode.transform.position - ls.startNode.transform.position;
				GameObject handle = new GameObject ("Handle " + ls.startNode.name + " <-> " + ls.endNode.name,
					                    new System.Type[]{ typeof(BezierHandle) });
				handle.transform.SetParent (BuilderData.Data.handleParent);
				BuilderData.Data.bezierHandles.Add (handle);
				BezierSegment bSeg = new BezierSegment (ls.startNode, ls.endNode, handle);
				BuilderData.Data.bezierSetMap.Add (bSeg);
				for (int i = 1; i < segment; i++) {
					GameObject newSection = new GameObject ("Section");
					newSection.transform.SetParent (BuilderData.Data.sectionParent);
					BuilderData.Data.sectionNodes.Add (newSection.AddComponent<SectionNode> ());
					bSeg.sectionNodes.Add (newSection);
					newSection.transform.position = ls.startNode.transform.position + dir.normalized * i * BuilderTool.Settings.sectionLength;
				}
			}
		}

		public static void RefreshBezierSegment ()
		{
			foreach (BezierSegment bs in BuilderData.Data.bezierSetMap) {
				for (int i = 0; i < bs.sectionNodes.Count; i++) {
					float t = (float)(i + 1) / (float)(bs.sectionNodes.Count + 1);
					float x = Mathf.Pow ((1f - t), 2f) * bs.startNode.transform.position.x +
					          2 * t * (1 - t) * bs.bezierHandle.transform.position.x +
					          Mathf.Pow (t, 2f) * bs.endNode.transform.position.x;
					float y = Mathf.Pow ((1f - t), 2f) * bs.startNode.transform.position.y +
					          2 * t * (1 - t) * bs.bezierHandle.transform.position.y +
					          Mathf.Pow (t, 2f) * bs.endNode.transform.position.y;
					float z = Mathf.Pow ((1f - t), 2f) * bs.startNode.transform.position.z +
					          2 * t * (1 - t) * bs.bezierHandle.transform.position.z +
					          Mathf.Pow (t, 2f) * bs.endNode.transform.position.z;
					bs.sectionNodes [i].transform.position = new Vector3 (x, y, z);
				}
			}
		}

		public static void Load ()
		{
			BuilderData.Data.ClearData ();

			string lfd = EditorUtility.OpenFilePanelWithFilters ("加载数据库文件", "", new string[]{ "(*.db)", "db" });
			if (lfd != "") {
				BuilderNodeDAO.DAO.Load (lfd);
				LinkInfoDAO.DAO.Load (lfd);
			} else {
				return;
			}

			foreach (BuilderNodeDAO.BuilderNodeRecord r in BuilderNodeDAO.DAO.builderNodeRecords) {
				BuilderNode newNode = BuilderCore.CreateNode (String2Vector3 (r.position));
				newNode.gameObject.transform.rotation = String2Quaternion (r.rotation);
				newNode.builderNodeId = r.id;
				switch (r.type) {
				case 1:
					newNode.builderNodeType = BuilderNodeType.Section;
					break;
				case 2:
					newNode.builderNodeType = BuilderNodeType.Intersection;
					break;
				}
			}
			foreach (LinkInfoDAO.LinkInfoRecord l in LinkInfoDAO.DAO.linkInfoRecords) {
				BuilderData.Data.linkGraph [l.fromId] [l.toId] = l.distance;
			}

			RefreshLineSetGroup ();
			RefreshSectionNodes ();
		}

		public static void Save ()
		{
			RefreshLinkGraph ();

			BuilderNodeDAO.DAO.builderNodeRecords.Clear ();
			foreach (BuilderNode n in BuilderData.Data.builderNodes) {
				int enumValue = 0;
				switch (n.builderNodeType) {
				case BuilderNodeType.Section:
					enumValue = 1;
					break;
				case BuilderNodeType.Intersection:
					enumValue = 2;
					break;
				}
				BuilderNodeDAO.DAO.builderNodeRecords.Add (new BuilderNodeDAO.BuilderNodeRecord (n.builderNodeId, Vector32String (n.transform.position), 
					Quaternion2String (n.transform.rotation), enumValue));
			}

			LinkInfoDAO.DAO.linkInfoRecords.Clear ();
			for (int i = 0; i < BuilderData.Data.linkGraph.Count; i++) {
				for (int j = 0; j < BuilderData.Data.linkGraph [i].Count - 1; j++) {
					if (BuilderData.Data.linkGraph [i] [j] != 999) {
						LinkInfoDAO.DAO.linkInfoRecords.Add (new LinkInfoDAO.LinkInfoRecord (i, j, BuilderData.Data.linkGraph [i] [j]));
					}
				}
			}

			string sfd = EditorUtility.SaveFilePanel ("保存至数据库", "", "", "db");
			if (sfd != "") {
				BuilderNodeDAO.DAO.Save (sfd);
				LinkInfoDAO.DAO.Save (sfd);
			} else {
				return;
			}
		}

		public static Vector3 String2Vector3 (string _str)
		{
			string regexStr = string.Empty; 
			regexStr = @"[(](\S+)[,](\S+)[,](\S+)[)]";
			Match mt = Regex.Match (_str, regexStr);

			return new Vector3 (float.Parse (mt.Groups [1].Value), 
				float.Parse (mt.Groups [2].Value), 
				float.Parse (mt.Groups [3].Value));
		}

		public static string Vector32String (Vector3 _v)
		{
			return "(" + _v.x.ToString () + "," + _v.y.ToString () + "," + _v.z.ToString () + ")";
		}

		public static string Quaternion2String (Quaternion _q)
		{
			return "(" + _q.x.ToString () + "," + _q.y.ToString () + "," + _q.z.ToString () + "," + _q.w.ToString () + ")";
		}

		public static Quaternion String2Quaternion (string _str)
		{
			string regexStr = string.Empty; 
			regexStr = @"[(](\S+)[,](\S+)[,](\S+)[,](\S+)[)]";
			Match mt = Regex.Match (_str, regexStr);

			return new Quaternion (float.Parse (mt.Groups [1].Value), 
				float.Parse (mt.Groups [2].Value), 
				float.Parse (mt.Groups [3].Value),
				float.Parse (mt.Groups [4].Value));
		}
	}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using TrafficSystem.IOStreamer;

namespace TrafficSystem.Base
{
	public class BaseData : MonoBehaviour
	{
		public static BaseData Data = null;

		string dbPath = string.Empty;

		//		[SerializeField] private NodeInfoDAO m_nodeInfoDao = null;
		//		[SerializeField] private LaneInfoDAO m_laneInfoDao = null;
		//		[SerializeField] private LinkInfoDAO m_intersectionLinkDao = null;
		//		[SerializeField] private SectionInfoDAO m_sectionInfoDao = null;
		//		[SerializeField] private BezierNodeInfoDAO m_bezierNodeInfoDao = null;
		//		[SerializeField] private BezierHandleInfoDAO m_bezierHandleInfoDao = null;
		//
		//		private List<GameObject> nodes, lanes, bezierHandles = new List<GameObject> ();
		//
		//		private List<List<int>> linkGraph, pathGraph = new List<List<int>> ();
		//
		//		public List<GameObject> Nodes {
		//			get{ return nodes; }
		//			set{ nodes = value; }
		//		}
		//
		//		public List<GameObject> Lanes {
		//			get{ return lanes; }
		//			set{ lanes = value; }
		//		}
		//
		//		public List<GameObject> BezierHandles {
		//			get{ return bezierHandles; }
		//			set{ bezierHandles = value; }
		//		}
		//
		//		public List<List<int>> LinkGraph {
		//			get{ return linkGraph; }
		//			set{ linkGraph = value; }
		//		}
		//
		//		public List<List<int>> PathGraph {
		//			get{ return pathGraph; }
		//			set{ pathGraph = value; }
		//		}

		void Awake ()
		{
			Data = this;
			dbPath = Application.streamingAssetsPath + "/" + "trafficsystem.db";
		}

		void Data2NodeInfoDAO ()
		{
//			foreach (GameObject t in nodes) {
//				Node nodeComp = t.GetComponent<Node> ();
//				int nodeId = nodeComp.NodeID;
//				string nodePos = "(" + t.transform.position.x + "," + t.transform.position.y + "," + t.transform.position.z + ")";
//				int parentLaneId = nodeComp.ParentLane.GetComponent<Lane> ().LaneID;
//				m_nodeInfoDao.NodeInfoRecords.Add (new NodeInfoDAO.NodeInfoRecord (nodeId, nodePos, parentLaneId));
//			}
//			m_nodeInfoDao.Save (dbPath);
		}

		void Data2LaneInfoDAO ()
		{
			
		}

		void Data2IntersectionLinkDAO ()
		{
			
		}

		void Data2BezierHandleDAO ()
		{
			
		}

		public void Save ()
		{
			StartCoroutine (SaveData2DB ());
		}

		IEnumerator SaveData2DB ()
		{
			Data2NodeInfoDAO ();
			yield return null;
		}

		public void Load ()
		{
			StartCoroutine (LoadDB2Data ());
		}

		IEnumerator LoadDB2Data ()
		{
			NodeInfoDAO2Data ();
			yield return null;
		}

		void NodeInfoDAO2Data ()
		{

		}

		void LinkInfo2Graph ()
		{

		}

		void LinkInfo2PathGraph ()
		{

		}

		void BezierHandleInfoData ()
		{
	
		}

		public static class Convert
		{
			public static Vector3 Convert2Vector3 (string _str)
			{
				string regexStr = string.Empty; 
				regexStr = @"[(](\S+)[,](\S+)[,](\S+)[)]";
				Match mt = Regex.Match (_str, regexStr);

				return new Vector3 (float.Parse (mt.Groups [1].Value), 
					float.Parse (mt.Groups [2].Value), 
					float.Parse (mt.Groups [3].Value));
			}

			public static Quaternion Convert2Quaternion (string _str)
			{
				string regexStr = string.Empty; 
				regexStr = @"[(](\S+)[,](\S+)[,](\S+)[,](\S+)[)]";
				Match mt = Regex.Match (_str, regexStr);

				return new Quaternion (float.Parse (mt.Groups [1].Value), 
					float.Parse (mt.Groups [2].Value), 
					float.Parse (mt.Groups [3].Value),
					float.Parse (mt.Groups [4].Value));
			}

			public static List<int> Convert2IntList (string _str)
			{
				string regexStr = string.Empty; 

				regexStr = @"[(](\S+)[)]";
				Match mt = Regex.Match (_str, regexStr);
				for (int i = 0; i < mt.Groups.Count; i++) {
					Debug.Log ("第" + i + "组：" + mt.Groups [i].Value);
				}

				string[] list = mt.Groups [1].Value.Split (',');
				List<int> intList = new List<int> ();
				foreach (string s in list) {
					intList.Add (int.Parse (s));
				}
				foreach (int i in intList) {
					Debug.Log (i.ToString ());
				}
				return intList;
			}
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TrafficSystem.Builder
{
	[CustomEditor (typeof(BuilderData))]
	public class BuildDataEditor : Editor
	{

		void OnEnable ()
		{
			
		}

		public override void OnInspectorGUI ()
		{
//			base.OnInspectorGUI ();
			if (GUILayout.Button ("清除数据")) {
				BuilderData.Data.ClearData ();
			}
//			if (GUILayout.Button ("Refresh Link Graph")) {
//				BuilderCore.RefreshLinkGraph ();
//			}

//			for (int i = 0; i < BuilderData.Data.linkGraph.Count; i++) {
//				GUILayout.BeginHorizontal ();
//				for (int j = 0; j < BuilderData.Data.linkGraph [i].Count; j++) {
//					GUILayout.BeginVertical (GUILayout.Width (1));
//					GUILayout.Box (BuilderData.Data.linkGraph [i] [j].ToString (), GUILayout.Width (30));
//					GUILayout.EndVertical ();
//				}
//				GUILayout.EndHorizontal ();
//			}
//			for (int i = 0; i < BuilderData.Data.lineSetMap.Count; i++) {
//				GUILayout.BeginHorizontal ();
//				GUILayout.Label (i + "th line");
//				EditorGUILayout.ObjectField (BuilderData.Data.lineSetMap [i].startNode, typeof(GameObject));
//				EditorGUILayout.ObjectField (BuilderData.Data.lineSetMap [i].endNode, typeof(GameObject));
//				GUILayout.EndHorizontal ();
//			}
		}

		[DrawGizmo (GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
		static void DrawGameObjectName (Transform transform, GizmoType gizmoType)
		{   
			if (transform.CompareTag ("EditorShown"))
				Handles.Label (transform.position, transform.gameObject.name);

		}
	}
}


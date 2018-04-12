using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TrafficSystem.Builder
{
	[CustomEditor (typeof(BuilderNode))]
	[CanEditMultipleObjects]
	public class BuilderNodeEditor : Editor
	{
		SerializedProperty builderNodeTypeProp;

		void OnEnable ()
		{
			builderNodeTypeProp = serializedObject.FindProperty ("builderNodeType");
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();
			base.OnInspectorGUI ();
			Transform[] selectedObjs = Selection.transforms;
			if (GUILayout.Button ("挤出")) {
				Selection.activeGameObject = ((BuilderNode)serializedObject.targetObject).Extrude ();
			}
			if (GUILayout.Button ("删除")) {
				BuilderCore.DeleteNode ((BuilderNode)serializedObject.targetObject);
				Selection.activeObject = BuilderData.Data.gameObject;
				return;
			}
			if (selectedObjs.Length == 2 && selectedObjs [0].GetComponent<BuilderNode> () != null
			    && selectedObjs [1].GetComponent<BuilderNode> () != null) {
				if (GUILayout.Button ("连接")) {
					BuilderCore.LinkNode (Selection.transforms [0].GetComponent<BuilderNode> ().builderNodeId,
						Selection.transforms [1].GetComponent<BuilderNode> ().builderNodeId);
				}
				if (GUILayout.Button ("断开")) {
					BuilderCore.BreakLink (Selection.transforms [0].GetComponent<BuilderNode> ().builderNodeId,
						Selection.transforms [1].GetComponent<BuilderNode> ().builderNodeId);
				}
			}
			if (GUILayout.Button ("整理冗余点和交叉口")) {
				for (int i = 0; i < BuilderData.Data.lineSetMap.Count; i++) {
					BuilderCore.MergeIntersection ();
					BuilderCore.RemoveDouble ();
				}
				BuilderCore.RefreshSectionNodes ();
			}

			serializedObject.ApplyModifiedProperties ();
		}
	}
}


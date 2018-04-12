using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TrafficSystem.Builder
{
	[CustomEditor (typeof(BuilderTool))]
	public class BuilderToolEditor : Editor
	{
		SerializedProperty mergeIntersectionThresholdProp;
		SerializedProperty removeDoubleThresholdProp;
		SerializedProperty laneWidthProp;
		SerializedProperty sectionLengthProp;

		SerializedProperty builderNodeMeshProp;

		void OnEnable ()
		{
			mergeIntersectionThresholdProp = serializedObject.FindProperty ("mergeIntersectionThreshold");
			removeDoubleThresholdProp = serializedObject.FindProperty ("removeDoubleThreshold");
			laneWidthProp = serializedObject.FindProperty ("laneWidth");
			sectionLengthProp = serializedObject.FindProperty ("sectionLength");
			builderNodeMeshProp = serializedObject.FindProperty ("builderNodeMesh");
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();

			EditorGUILayout.Slider (mergeIntersectionThresholdProp, 0.1f, 100f, new GUIContent ("交叉口焊接阈值"));
			EditorGUILayout.Slider (removeDoubleThresholdProp, 0.1f, 100f, new GUIContent ("冗余点焊接阈值"));
			EditorGUILayout.Slider (laneWidthProp, 0.1f, 100f, new GUIContent ("车道宽度"));
			EditorGUILayout.Slider (sectionLengthProp, 0.1f, 100f, new GUIContent ("段长度"));

			EditorGUILayout.ObjectField (builderNodeMeshProp, typeof(Mesh));

			if (BuilderData.Data.builderNodes.Count == 0) {
				if (GUILayout.Button ("创建根节点")) {
					Selection.activeGameObject = BuilderCore.CreateRootNode (Vector3.zero).gameObject;
				}
			}
//			if (GUILayout.Button ("刷新贝塞尔曲线")) {
//				BuilderCore.RefreshBezierSegment ();
//			}
			if (GUILayout.Button ("整理冗余点和交叉口")) {
				for (int i = 0; i < BuilderData.Data.lineSetMap.Count; i++) {
					BuilderCore.MergeIntersection ();
					BuilderCore.RemoveDouble ();
				}
				BuilderCore.RefreshSectionNodes ();
			}
			if (GUILayout.Button ("加载数据库文件")) {
				BuilderCore.Load ();
			}
			if (GUILayout.Button ("保存至数据库")) {
				BuilderCore.Save ();
			}

			serializedObject.ApplyModifiedProperties ();
		}
	}
}


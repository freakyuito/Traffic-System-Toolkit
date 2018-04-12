using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrafficSystem.IOStreamer;

namespace TrafficSystem.Builder
{
	[RequireComponent (typeof(BuilderData))]
	[RequireComponent (typeof(BuilderNodeDAO))]
	[RequireComponent (typeof(LinkInfoDAO))]
	[ExecuteInEditMode]
	public class BuilderTool : MonoBehaviour
	{
		public static BuilderTool Settings = null;

		public float mergeIntersectionThreshold = 0.2f;
		public float removeDoubleThreshold = 2.5f;
		public float laneWidth = 2.5f;
		public float sectionLength = 5f;

		public Mesh builderNodeMesh = null;

		public string databasePath = string.Empty;

		void OnEnable ()
		{
			Settings = this;
			databasePath = Application.streamingAssetsPath + "/" + "trafficsystem.db";
		}
	}
}


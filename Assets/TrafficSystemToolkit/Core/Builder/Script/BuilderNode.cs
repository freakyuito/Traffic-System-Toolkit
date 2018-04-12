using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficSystem.Builder
{
	[ExecuteInEditMode]
	public class BuilderNode : MonoBehaviour
	{
		[SerializeField] public BuilderCore.BuilderNodeType builderNodeType = BuilderCore.BuilderNodeType.Section;
		[SerializeField] public int builderNodeId;
		public GameObject leftLane, rightLane;

		void OnEnable ()
		{
			
		}

		public GameObject Extrude ()
		{
			return BuilderCore.Extrude (this);
		}

		void OnDrawGizmos ()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawMesh (BuilderTool.Settings.builderNodeMesh, transform.position);
			Gizmos.color = new Color (1f, 1f, 1f, 0.35f);
			Gizmos.DrawSphere (transform.position, BuilderTool.Settings.removeDoubleThreshold);
		}

		void OnDestroy ()
		{
			BuilderCore.OnDeleteNode (this);
		}
	}

}

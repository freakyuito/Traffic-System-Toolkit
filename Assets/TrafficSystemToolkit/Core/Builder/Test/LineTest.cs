using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrafficSystem.Builder;

public class LineTest : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		

	}

	void OnDrawGizmos ()
	{
		BuilderCore.Line line = BuilderCore.DistBetweenLines (BuilderData.Data.lineSetMap [0].GetLine, BuilderData.Data.lineSetMap [26].GetLine);
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine (line.start, line.end);
		print (line.Length);
	}
}

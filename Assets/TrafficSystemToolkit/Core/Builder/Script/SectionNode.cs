using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionNode : MonoBehaviour
{

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere (transform.position, 0.25f);
	}
}

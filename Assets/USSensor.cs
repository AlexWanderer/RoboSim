using UnityEngine;
using System.Collections;

public class USSensor : Block {
	public float maxDist = 10.0f;
	public float cooldownTime = 0.1f;

	private float lastTime;
	private float lastResult;

	void Start () {
	
	}
	
	public float Measure()
	{
		RaycastHit hit;

		Vector3 p1 = transform.position + transform.forward*0.2f;
		if (Physics.SphereCast (p1, 0.2f,transform.forward, out hit, maxDist)) {
			Debug.DrawRay (p1, transform.forward);
			//Debug.Log ("Dist: " + hit.distance.ToString ());
			return (float)hit.distance;

		} else {
			return maxDist;
		}

	}

}

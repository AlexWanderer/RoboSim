using UnityEngine;
using System.Collections;

public class HypnoGrabber : Block {
	FixedJoint joint;
	Vector3 p1;
	// Use this for initialization
	void Start () {
		Init ();
		p1 = transform.position + transform.forward * 0.3f;
	}

	public void Grab() {
		if (joint == null) {
			RaycastHit hit;
			p1 = transform.position + transform.forward * 0.3f;
			if (Physics.Raycast (p1, transform.forward, out hit, 4f)) {
				Debug.DrawRay (p1, transform.forward);
				//Debug.Log ("Dist: " + hit.distance.ToString ());
				//joint = new FixedJoint ();
				joint = gameObject.AddComponent<FixedJoint> ();
				joint.connectedBody = hit.transform.gameObject.GetComponent<Rigidbody>();
				joint.enableCollision = true;
			}
		} else {
			Destroy (joint);
		}
	}


	// Update is called once per frame
	void Update () {
		p1 = transform.position + transform.forward * 0.3f;
		Debug.DrawRay (p1, transform.forward);
	}
}

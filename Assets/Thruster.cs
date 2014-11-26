using UnityEngine;
using System.Collections;

public class Thruster : Block {
	public bool isOn;
	public float force;

	public void Thrust(float f) {
		isOn = true;
		force = f;
	}

	void FixedUpdate() {
		if (isOn) {
			this.rigidbody.AddRelativeForce (force * Vector3.forward);
		}
	}
}

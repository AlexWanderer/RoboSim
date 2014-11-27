using UnityEngine;
using System.Collections;

public class Thruster : Block {
	public bool isOn;
	public float force;

	public void SetThrust(float f) {
		//isOn = true;
		force = f;
	}
	
	public void ToggleThruster() {
		isOn = !isOn;
		//force = f;
	}

	void FixedUpdate() {
		if (isOn) {
			this.rigidbody.AddRelativeForce (force * Vector3.forward);
		}
	}
}

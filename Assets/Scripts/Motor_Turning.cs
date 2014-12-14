using UnityEngine;
using System.Collections;

public class Motor_Turning : Motor {
	public GameObject axle;
	// Use this for initialization
	void Start() {
		JointMotor motor = wheel.GetComponent<HingeJoint>().motor;
		motor.force = torque;
		wheel.GetComponent<HingeJoint>().motor = motor;
		lastAngle = wheel.transform.rotation.z;
	}
	
	
	
	public void SetTurnAngle(float angle) 
	{
		ConfigurableJoint axis = axle.GetComponent<ConfigurableJoint> ();
		SoftJointLimit lim = axis.highAngularXLimit;
		lim.limit = angle+ 0.5f;
		axis.highAngularXLimit = lim;
		lim = axis.lowAngularXLimit;
		lim.limit = angle -0.5f;
		axis.lowAngularXLimit = lim;
		ConfigurableJoint axis2 = axle.GetComponent<ConfigurableJoint> ();
		//Destroy (axis2);
		//axle.AddComponent<ConfigurableJoint> ();
		axis2 = axis;

		//axle.GetComponent<Rigidbody> ().AddRelativeTorque (new Vector3 (Mathf.Sign (angle) * 10, 0, 0));
	}
}

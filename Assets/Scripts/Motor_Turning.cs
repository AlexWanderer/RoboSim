using UnityEngine;
using System.Collections;

public class Motor_Turning : Motor {
	public GameObject axle;
	// Use this for initialization
	void Start() {
		JointMotor motor = wheel.hingeJoint.motor;
		motor.force = torque;
		wheel.hingeJoint.motor = motor;
		lastAngle = wheel.transform.rotation.z;
	}
	


	public void SetTurnAngle(float angle) 
	{
		ConfigurableJoint axis = axle.GetComponent<ConfigurableJoint> ();
		SoftJointLimit lim = axis.highAngularXLimit;
		lim.limit = angle;
		axis.highAngularXLimit = lim;
		lim = axis.lowAngularXLimit;
		lim.limit = angle;
		axis.lowAngularXLimit = lim;
		ConfigurableJoint axis2 = axle.GetComponent<ConfigurableJoint> ();
		axis2 = axis;
	}
}

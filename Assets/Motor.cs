using UnityEngine;
using System.Collections;

public class Motor : Block {
	//public string MotorID = "M1";
	public GameObject wheel;


	public void SetContSpeed(float speed)
	{
		JointMotor motor = wheel.hingeJoint.motor;
		motor.targetVelocity = speed;
		wheel.hingeJoint.motor = motor;
	}


}

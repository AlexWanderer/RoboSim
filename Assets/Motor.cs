using UnityEngine;
using System.Collections;

public class Motor : Block {
	//public string MotorID = "M1";
	public GameObject wheel;
	public float torque;
	public float maxSpeed;
	public float maxEnergyDrain;
	public MotorMode mode;
	
	private float travelAngle;
	private float lastAngle = 0f;
	private bool freeSpin;
	
	public enum MotorMode
	{
		Off,
		Continious,
		TimedRun,
		RevRun
	}
	
	void Start() {
		JointMotor motor = wheel.hingeJoint.motor;
		motor.force = torque;
		wheel.hingeJoint.motor = motor;
		lastAngle = wheel.transform.rotation.z;
	}
	
	void FixedUpdate() {
		travelAngle += GetDelta();
	}

	public void SetContSpeed(float speed)
	{
		JointMotor motor = wheel.hingeJoint.motor;
		motor.targetVelocity = speed;
		wheel.hingeJoint.motor = motor;
	}

	public void ResetOdometer() {
		travelAngle = 0f;
	}
	
	public float GetDistance() {
		return travelAngle;
	}
	
	public float GetDelta() {
	float delta;
		delta = Mathf.DeltaAngle(wheel.transform.rotation.z,lastAngle);
	lastAngle = wheel.transform.rotation.z;
	return delta;
	}
	
	public void RunForSeconds(float speed, float time) 
	{
		mode = MotorMode.TimedRun;
		
	
	}
	
	public void RunForRevolutions(float speed, float revs) 
	{
		float degrees = revs * 360f;
		mode = MotorMode.RevRun;
	
	}
	
	public void SwitchFreeSpin() 
	{
		
	
	}

}

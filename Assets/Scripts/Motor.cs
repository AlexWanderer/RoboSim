using UnityEngine;
using System.Collections;

public class Motor : Block {
	//public string MotorID = "M1";
	public GameObject wheel;
	public float torque;
	public float maxSpeed;
	public float maxEnergyDrain;
	public MotorMode mode;
	
	public float travelAngle;
	public float lastAngle = 0f;
	public bool freeSpin = true;

	public bool dbgLogAngle = false;

	private float AngleA;
	private float AngleB;



	public enum MotorMode
	{
		Off,
		Continious,
		TimedRun,
		RevRun
	}
	
	void Start() {
		JointMotor motor = wheel.GetComponent<HingeJoint>().motor;
		motor.force = torque;
		wheel.GetComponent<HingeJoint>().motor = motor;
		lastAngle = wheel.transform.rotation.eulerAngles.z;
		Init ();
	}
	
	void FixedUpdate() {
		travelAngle += GetDelta();
		if (dbgLogAngle) {
			Debug.Log(travelAngle);
				}
		//Debug.Log (wheel.GetComponent<Rigidbody> ().IsSleeping ());
		if (mode == MotorMode.RevRun) {
					if(travelAngle> AngleB) { //Воу, это не работает при отрицательных углах! исправить!
						//SendUnlockSequenceToProcessor() -  разблокируем процессор, прродолжая выполнение программы
						mode= MotorMode.Off;
					}
				// ТОДО: добавить защиту от полной блокировки программы путем добавления тайм-аута, который принудительно продолжает выполнение программы.
				}
	}

	public void SetContSpeed(float speed)
	{
		JointMotor motor = wheel.GetComponent<HingeJoint>().motor;
		motor.targetVelocity = speed;
		wheel.GetComponent<HingeJoint>().motor = motor;
		wheel.GetComponent<Rigidbody> ().WakeUp ();
	}

	public void ResetOdometer() {
		travelAngle = 0f;
	}
	
	public float GetDistance() {
		return travelAngle;
	}
	
	public float GetDelta() {
		float delta;
		delta = Mathf.DeltaAngle(wheel.transform.rotation.eulerAngles.z,lastAngle);
		lastAngle = wheel.transform.rotation.eulerAngles.z;
		return delta;
	}
	
	public void RunForSeconds(float speed, float time) 
	{
		mode = MotorMode.TimedRun;
		
	
	}
	
	public void RunForRevolutions(float speed, float revs) 
	{	
		AngleA = travelAngle;
		float degrees = revs * 360f;
		AngleB = AngleA + degrees;
		mode = MotorMode.RevRun;
	
	}
	
	public void SwitchFreeSpin() 
	{
		JointMotor motor = wheel.GetComponent<HingeJoint>().motor;
		freeSpin = !freeSpin;
		motor.freeSpin = freeSpin;
		wheel.GetComponent<HingeJoint>().motor = motor;
	
	}



}

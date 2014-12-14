using UnityEngine;
using System.Collections;

public class Servo : Block {
	public GameObject Az;
	public GameObject Vert;

	public float VRotCoef= 1f;
	public float HRotCoef =1f;

	public float maxAngSpd = 200f;

	//private float a;
	//private float b;
	//private float c;
	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate() {
		//a += 2f;
		//c += 0.1f;
		//SetAz (a);
		//b = Mathf.Sin (c);
		//SetVert (b*20);

		}
	
	public void SetAz(float angle) {
		float delta = Mathf.DeltaAngle(Az.transform.localRotation.eulerAngles.z,angle);
		Debug.Log (delta);
		Az.transform.localRotation *= Quaternion.Euler(new Vector3(0f,0f,delta));
		}

	public void SetVert(float angle) {
		float delta = Mathf.DeltaAngle(Vert.transform.localRotation.eulerAngles.y,angle);
		Vert.transform.localRotation *= Quaternion.Euler(new Vector3(0f,delta,0f));
	}
}

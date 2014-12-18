using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Editor Camera")]
public class EditorCamera : MonoBehaviour {
	
	public Transform target;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	public float distanceMin = .5f;
	public float distanceMax = 15f;

	public float moveSpd = 0.5f;

	float x = 0.0f;
	float y = 0.0f;
	
	// Use this for initialization
	void Start () {
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		

	}
	
	void LateUpdate () {
		if (Input.GetMouseButton(1)) {
			if(Input.GetKey(KeyCode.LeftShift)) {
				Vector3 pos = target.position;
				pos += target.transform.TransformDirection(new Vector3(Input.GetAxis("Mouse X") * moveSpd,Input.GetAxis("Mouse Y") * moveSpd,0));
				//pos.x += Input.GetAxis("Mouse X") * moveSpd;
				//pos.y += Input.GetAxis("Mouse Y") * moveSpd;
				target.position = pos;
				//Vector3 position = rotation * negDistance + target.position;		


			} else{

				x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
				y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
				y = ClampAngle(y, yMinLimit, yMaxLimit);
		
			}
		}

		Vector3 p = target.position;
		p += target.transform.TransformDirection (Vector3.forward * 3f * Input.GetAxis("Mouse ScrollWheel"));
		target.position = p;

		Quaternion rotation = Quaternion.Euler(y, x, 0);
		RaycastHit hit;
		//if (Physics.Linecast (target.position, transform.position, out hit)) {
		//	distance -=  hit.distance;
		//}
		Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
		Vector3 position = rotation * negDistance + target.position;		
		transform.rotation = rotation;
		transform.position = position;
		target.rotation = Quaternion.LookRotation(target.position - transform.position);

	}
	
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
	
	
}
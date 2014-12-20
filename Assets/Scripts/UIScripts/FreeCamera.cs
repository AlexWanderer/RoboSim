using UnityEngine;


[AddComponentMenu("Camera/Smooth Mouse Look ")]
public class FreeCamera : MonoBehaviour
{ 
	public enum CamMode
		{
		free,
		target

		}

	public CamMode mode = CamMode.free;
	public GameObject guiObject;
	public bool Control = true;
	private GUI_Main gui; 

	Vector2 _mouseAbsolute;
	Vector2 _smoothMouse;

	public Vector2 clampInDegrees = new Vector2(360, 180);
	public bool lockCursor;
	public Vector2 sensitivity = new Vector2(2, 2);
	public Vector2 smoothing = new Vector2(3, 3);
	public Vector2 targetDirection;
	public Vector2 targetCharacterDirection;


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




	// Assign this if there's a parent object controlling motion, such as a Character Controller.
	// Yaw rotation will affect this object instead of the camera if set.
	public GameObject characterBody;

	void Start()
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;



		gui = guiObject.GetComponent<GUI_Main> ();
		// Set target direction to the camera's initial orientation.
		targetDirection = transform.localRotation.eulerAngles;

		// Set target direction for the character body to its inital state.
		if (characterBody) targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
	}

	void OnGUI() {


	}

	void LateUpdate()
	{ 

		if (Global.SelectedRobot) {
						target = Global.SelectedRobot.transform;
				}
		if (gui.mode == GUI_Main.GUIMode.Game) {
			if(Input.GetKeyDown("c")) {
				mode = CamMode.free;
			}

			if(Input.GetKeyDown("v")) {
				if(target) {
					mode = CamMode.target;} else {Debug.Log ("No target selected"); }
			}

			if(mode == CamMode.free) {

				if (Input.GetMouseButton(1)){

					var targetOrientation = Quaternion.Euler (targetDirection);
					var targetCharacterOrientation = Quaternion.Euler (targetCharacterDirection);

					// Get raw mouse input for a cleaner reading on more sensitive mice.
					var mouseDelta = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));

					// Scale input against the sensitivity setting and multiply that against the smoothing value.
					mouseDelta = Vector2.Scale (mouseDelta, new Vector2 (sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

					// Interpolate mouse movement over time to apply smoothing delta.
					_smoothMouse.x = Mathf.Lerp (_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
					_smoothMouse.y = Mathf.Lerp (_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

					// Find the absolute mouse movement value from point zero.
					_mouseAbsolute += _smoothMouse;

					// Clamp and apply the local x value first, so as not to be affected by world transforms.
					if (clampInDegrees.x < 360)
					_mouseAbsolute.x = Mathf.Clamp (_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);
						
					var xRotation = Quaternion.AngleAxis (-_mouseAbsolute.y, targetOrientation * Vector3.right);
					transform.localRotation = xRotation;
						
					// Then clamp and apply the global y value.
					if (clampInDegrees.y < 360)
					_mouseAbsolute.y = Mathf.Clamp (_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

					transform.localRotation *= targetOrientation;

					// If there's a character body that acts as a parent to the camera
					if (characterBody) {
					var yRotation = Quaternion.AngleAxis (_mouseAbsolute.x, characterBody.transform.up);
					characterBody.transform.localRotation = yRotation;
					characterBody.transform.localRotation *= targetCharacterOrientation;
					} else {
					var yRotation = Quaternion.AngleAxis (_mouseAbsolute.x, transform.InverseTransformDirection (Vector3.up));
					transform.localRotation *= yRotation;
					}
				}
			
				if(Input.GetKey("w")) {
					transform.Translate(0f,0f,.1f);
				}
			
				if(Input.GetKey("s")) {
					transform.Translate(0f,0f,-.1f);
				}

				if(Input.GetKey("a")) {
					transform.Translate(-.1f,0f,0f);
				}

				if(Input.GetKey("d")) {
					transform.Translate(.1f,0f,0f);
				}

			}
			if((mode == CamMode.target)&&(target)) {
				target  = Global.SelectedRobot.transform;
				if (Input.GetMouseButton(1)) {
					x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
					y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
					y = EditorCamera.ClampAngle(y, yMinLimit, yMaxLimit); //Не код, а каша, епта

				}


				Vector3 p = transform.position;
				distance += 3f * Input.GetAxis("Mouse ScrollWheel");
				if (distance < distanceMin) {distance=distanceMin ; }
				if (distance > distanceMax) {distance=distanceMax ; }

				Quaternion rotation = Quaternion.Euler(y, x, 0);
				RaycastHit hit;

				Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
				Vector3 position = rotation * negDistance + target.position;		
				transform.rotation = rotation;
				transform.position = position;

			}



		}
	}
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditorUI : MonoBehaviour {


	public enum EditorMode
		{
		firstPart,
		move,
		rotate,
		place,
		scale

		}
	public GameObject moveGizmo;
	public GameObject rotGizmo;
	public GameObject sclGizmo;
	public LayerMask gizmoMask = -1;

	public string robotName;

	public InputField nameEdit;

	public EditorMode mode = EditorMode.firstPart;

	public GameObject realRoot;
	private bool firstPart = true;

	//private GameObject LastBlock;
	private GameObject SelectedBlock;

	private bool hasActivePart = false;

	public EditorCamera edCam;
	private GameObject cam;

	private Quaternion rotation = Quaternion.Euler (0f, 0f, 0f);

	private bool placeFlag;
	private int axis;

	void Start () {
		cam = edCam.gameObject;
		moveGizmo = (GameObject) Instantiate (moveGizmo);

		moveGizmo.SetActive (false);
		rotGizmo = (GameObject) Instantiate (rotGizmo);
		rotGizmo.SetActive (false);
	}
	

	void Update () {

				if ((hasActivePart) && (SelectedBlock) && (mode == EditorMode.place)) { // Режим установки блоков
						RaycastHit hit;
						Ray ray = cam.GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);
						if (Physics.Raycast (ray, out hit, 30f)) {
								//Debug.Log (hit.point);
								if (hit.rigidbody) {
										SelectedBlock.transform.position = hit.point;
										SelectedBlock.transform.rotation = Quaternion.LookRotation (hit.normal) * rotation;
										if (Input.GetKeyDown ("w")) {
												rotation *= Quaternion.Euler (new Vector3 (90, 0, 0));
										}
					
										if (Input.GetKeyDown ("s")) {
												rotation *= Quaternion.Euler (new Vector3 (-90, 0, 0));
										}
					
										if (Input.GetKeyDown ("a")) {
												rotation *= Quaternion.Euler (new Vector3 (0, 0, 90));
										}
					
										if (Input.GetKeyDown ("d")) {
												rotation *= Quaternion.Euler (new Vector3 (0, 0, -90));
										}
					
										if (Input.GetKeyDown ("q")) {
												rotation *= Quaternion.Euler (new Vector3 (0, 90, 0));
										}
					
										if (Input.GetKeyDown ("e")) {
												rotation *= Quaternion.Euler (new Vector3 (0, -90, 0));
										}
										if (Input.GetMouseButtonDown (0) && (hit.transform.gameObject.GetComponent<Block> ())) { //Тут поменял, не уверен, что будет работать на всем
												hasActivePart = false;
												ActivateBlock (SelectedBlock);
												SelectedBlock.transform.parent = hit.transform;
												if (SelectedBlock.GetComponent<FixedJoint> ()) {
														Destroy (SelectedBlock.GetComponent<FixedJoint> ()); // Если уже к чему-то привязаны, то убираем.
												}
												var joint = SelectedBlock.AddComponent<FixedJoint> (); //Связываем объекты (проверить, можно ли их двигать после этого). Связывание нужно для того, чтобы записать в сохранение наличие связи между объектами
												joint.connectedBody = hit.rigidbody;
												//joint.enableCollision = true;
												SelectedBlock = null;
					
										}
								}
						}
				} else if ((mode == EditorMode.place) && (hasActivePart == false)) {
						if (Input.GetMouseButtonDown (0)) {
							RaycastHit hit;
							Ray ray = cam.GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);
							if (Physics.Raycast (ray, out hit, 30f)) {
								if (hit.rigidbody) {
									if (hit.transform.gameObject.GetComponent<Block> ()) {
										if (!hit.transform.gameObject.GetComponent<Block> ().GetRoot ()) { //корневую деталь ни к чему прикрепить нельзя, поэтому мы ее не двигаем
											hasActivePart = true;
											SelectedBlock = hit.transform.gameObject;
											DeactivateBlock (SelectedBlock);
										}	
									}
								}
							}
						}
				} else if (mode != EditorMode.place) {
						placeFlag = false;
						if(SelectedBlock) {
							ActivateBlock(SelectedBlock);
						}
						if (Input.GetMouseButtonDown (0)) {
							RaycastHit hit;
							Ray ray = cam.GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);
							if (Physics.Raycast (ray, out hit, 30f)) {
								if (hit.rigidbody) {
									if (hit.transform.gameObject.GetComponent<Block> ()) {
											hasActivePart = true;
											SelectedBlock = hit.transform.gameObject;
											DeactivateBlock (SelectedBlock);		
									}
								}
							}
						}
					}

				if ((mode == EditorMode.move) && (hasActivePart == true)) {
						moveGizmo.SetActive (true);


						moveGizmo.transform.position = SelectedBlock.transform.position;
						RaycastHit hit;
						Ray ray = cam.GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);
						if (Physics.Raycast (ray, out hit, 30f,gizmoMask)&&(!Input.GetMouseButton(0))) {
							switch(hit.transform.transform.gameObject.name)
							{
								case "XAxis":
									axis=1;
									Debug.Log ("XAxis");
									break;	
								case "YAxis":
									axis=2;
									Debug.Log ("YAxis");
									break;
								case "ZAxis":
									axis=3;
									Debug.Log ("ZAxis");
									break;
								default:
									axis = 0;
									break;
							}

						} 

					if(Input.GetMouseButton(0)){
						switch (axis)
						{
						case 1:
							SelectedBlock.transform.position += new Vector3(-(Input.GetAxis("Mouse X")+Input.GetAxis("Mouse Y"))*.1f,0,0);
						break;
						case 2:
							SelectedBlock.transform.position += new Vector3(0,Input.GetAxis("Mouse Y")*.2f,0);
						break;
						case 3:
							SelectedBlock.transform.position += new Vector3(0,0,-(Input.GetAxis("Mouse X")+Input.GetAxis("Mouse Y"))*.1f);	
						break;

							
						}
					}
						
				} else {
					moveGizmo.SetActive(false);
				}

				if ((mode == EditorMode.rotate) && (hasActivePart == true)) {
					rotGizmo.SetActive (true);
					rotGizmo.transform.position = SelectedBlock.transform.position;
					
					RaycastHit hit;
					Ray ray = cam.GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit, 30f,gizmoMask)&&(!Input.GetMouseButton(0))) {
						switch(hit.transform.transform.gameObject.name)
						{
							case "X":
							axis=1;
							Debug.Log ("X");
							break;	
							case "Y":
							axis=2;
							Debug.Log ("Y");
							break;
							case "Z":
							axis=3;
							Debug.Log ("Z");
							break;
							default:
							axis = 0;
							break;
						}
				
					} 
			
				if(Input.GetMouseButton(0)){
					switch (axis)
					{
					case 3:
						SelectedBlock.transform.rotation *= Quaternion.Euler (new Vector3((Input.GetAxis("Mouse X")+Input.GetAxis("Mouse Y"))*.8f,0,0));
					break;
					case 2:
						SelectedBlock.transform.rotation *= Quaternion.Euler (new Vector3(0,-(Input.GetAxis("Mouse X")+Input.GetAxis("Mouse Y"))*.8f,0));
					break;
					case 1:
						SelectedBlock.transform.rotation *= Quaternion.Euler (new Vector3(0,0,-(Input.GetAxis("Mouse X")+Input.GetAxis("Mouse Y"))*.8f));	
					break;	
					}
				}
			} else {
				rotGizmo.SetActive(false);
				}

				
	}

	public void SetName(string n) {
		robotName = n;
	}

	public void SetName() {
		robotName = nameEdit.text;
	}

	public void DeactivateBlock(GameObject block) { // Отключаем всю физику на блоке

		Collider[] cols;
		cols = block.GetComponents<Collider> ();
		foreach (Collider col in cols) {
			col.enabled = false;
		}

		block.gameObject.GetComponent<Rigidbody>().isKinematic = true;
		foreach (Transform child in block.transform) {
			if(child.gameObject.GetComponent<Collider>()) {
				child.gameObject.GetComponent<Collider>().enabled = false;
			}
			if (child.gameObject.GetComponent<Rigidbody>()) {
				child.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			}
		}

	}

	public void ActivateBlock(GameObject block) { // Включаем обратно физику, кроме кинематика (его в редакторе вообще лучше не убирать)
		
		Collider[] cols;
		cols = block.GetComponents<Collider> ();
		foreach (Collider col in cols) {
			col.enabled = true;
		}

		foreach (Transform child in block.transform) {
			if(child.gameObject.GetComponent<Collider>()) {
				child.gameObject.GetComponent<Collider>().enabled = true;
			}
		}
		
	}

	public void SetMoveMode() 
	{		mode = EditorMode.move;	}

	public void SetPlaceMode() 
	{		mode = EditorMode.place; SelectedBlock = null; hasActivePart = false;	}

	public void SetRotateMode() 
	{		mode = EditorMode.rotate;	}

	public void SetScaleMode() 
	{		mode = EditorMode.scale;	}

	public void SetMode(EditorMode m) 
	{
		if (m == EditorMode.place) {
			SelectedBlock = null; hasActivePart = false;
		}
		mode = m;
	}


	public void RemoveSelection() 
	{
		if ((hasActivePart)&&(placeFlag))
		{
			placeFlag = false;
			hasActivePart = false;
			Destroy(SelectedBlock);
		}
		//Debug.Log ("Click Remove Selection");
	}

	public void SaveRobot() {
		if (realRoot) {
						Debug.Log("saving...");
						GetComponent<XMLLoader> ().SaveToXml (realRoot, robotName);
				}

	}

	public void LoadRobot() {
		Debug.Log("Loading...");
		NewRobot ();
		realRoot = GetComponent<XMLLoader> ().LoadFromXML(new Vector3(0,3,0),robotName);
		firstPart = false;
		mode = EditorMode.place;
	
	}



	public void NewRobot() 
	{
		firstPart = true;
		if (realRoot) {
			foreach (Transform child in realRoot.transform) {
				Destroy(child.gameObject);
			}
			Destroy (realRoot);
		}

	}

	public void LoadPart(string partName) 
	{
		if ((hasActivePart)&&(placeFlag))
		{
			placeFlag = false;
			Destroy(SelectedBlock); //Если мы уже таскаем блок и кликаем по спауну еще одного, старый удаляется
			hasActivePart = false;
		}


		GameObject part = (GameObject) Resources.Load (partName);
		GameObject block = (GameObject) Instantiate(part,new Vector3(0,2,0),part.transform.rotation);
		block.name.Replace ("(clone)","");

		if (mode == EditorMode.firstPart) {
			mode = EditorMode.place;
			block.GetComponent<Rigidbody> ().isKinematic = true;
			realRoot = new GameObject ();
			realRoot.name = "RobotRoot";
			realRoot.transform.position = block.transform.position;
			hasActivePart = false;
			edCam.target.transform.position = block.transform.position;
			block.GetComponent<Block>().SetRoot();
		} else {
					mode = EditorMode.place;
					placeFlag = true;
					SelectedBlock = block;
					hasActivePart = true;
					DeactivateBlock(block);
				}
		block.transform.parent = realRoot.transform;

	}
}

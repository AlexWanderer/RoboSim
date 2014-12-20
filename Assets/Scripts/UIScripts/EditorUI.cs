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

	public EditorMode mode = EditorMode.firstPart;

	public GameObject realRoot;
	private bool firstPart = true;

	//private GameObject LastBlock;
	private GameObject SelectedBlock;

	private bool hasActivePart = false;

	public EditorCamera edCam;
	private GameObject cam;

	private Quaternion rotation = Quaternion.Euler (0f, 0f, 0f);

	void Start () {
		cam = edCam.gameObject;
		moveGizmo = (GameObject) Instantiate (moveGizmo);
		moveGizmo.SetActive (false);
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
												SelectedBlock = null;
					
										}
								}
						}
				} else if ((mode != EditorMode.firstPart) && (hasActivePart == false)) {
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
				}

				if ((mode == EditorMode.move) && (hasActivePart == true)) {
						moveGizmo.SetActive (true);
						moveGizmo.transform.position = SelectedBlock.transform.position;

						//А остальное доделаем завтра. Сегодня уже пора спать.
				} else {
					moveGizmo.SetActive(false);
				}
	}



	public void DeactivateBlock(GameObject block) { // Отключаем всю физику на блоке

		Collider[] cols;
		cols = block.GetComponents<Collider> ();
		foreach (Collider col in cols) {
			col.enabled = false;
		}

		block.gameObject.GetComponent<Rigidbody>().isKinematic = true;
		foreach (Transform child in block.transform) {
			child.gameObject.GetComponent<Collider>().enabled = false;
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
			child.gameObject.GetComponent<Collider>().enabled = true;
		}
		
	}

	public void SetMoveMode() 
	{		mode = EditorMode.move;	}

	public void SetPlaceMode() 
	{		mode = EditorMode.place;	}

	public void SetRotateMode() 
	{		mode = EditorMode.rotate;	}

	public void SetScaleMode() 
	{		mode = EditorMode.scale;	}

	public void SetMode(EditorMode m) 
	{
		mode = m;
	}


	public void RemoveSelection() 
	{
		if (hasActivePart)
		{
			hasActivePart = false;
			Destroy(SelectedBlock);
		}
		//Debug.Log ("Click Remove Selection");
	}

	public void NewRobot() 
	{
		firstPart = true;
		foreach (Transform child in realRoot.transform) {
			Destroy(child.gameObject);
		}
		Destroy (realRoot);

	}

	public void LoadPart(string partName) 
	{
		if (hasActivePart) 
		{
			Destroy(SelectedBlock); //Если мы уже таскаем блок и кликаем по спауну еще одного, старый удаляется
			hasActivePart = false;
		}

		GameObject part = (GameObject) Resources.Load (partName);
		GameObject block = (GameObject) Instantiate(part,new Vector3(0,2,0),Quaternion.identity);
		block.name.Replace ("(clone)","");

		//LastBlock = block;

		if (mode == EditorMode.firstPart) {
			mode = EditorMode.place;
			block.GetComponent<Rigidbody> ().isKinematic = true;
			realRoot = new GameObject ();
			realRoot.name = "RobotRoot";
			hasActivePart = false;
			edCam.target.transform.position = block.transform.position;
			block.GetComponent<Block>().SetRoot();
		} else {
					mode = EditorMode.place;
					SelectedBlock = block;
					hasActivePart = true;
					DeactivateBlock(block);
				}

		block.transform.parent = realRoot.transform;

		
	}
}

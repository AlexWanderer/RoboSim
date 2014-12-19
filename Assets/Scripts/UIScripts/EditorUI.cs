using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditorUI : MonoBehaviour {
	public GameObject realRoot;
	private bool firstPart = true;

	private GameObject LastBlock;
	private GameObject SelectedBlock;

	private bool hasActivePart = false;

	public EditorCamera edCam;
	private GameObject cam;

	void Start () {
		cam = edCam.gameObject;
	}
	

	void Update () {
		if ((hasActivePart)&&(SelectedBlock)) 
		{

			Vector3 direction = cam.transform.TransformDirection (Vector3.forward);
			RaycastHit hit;
			Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 30f)) {
				Debug.Log (hit.point);
				if(hit.rigidbody) 
				{
					SelectedBlock.transform.position = hit.point;
					SelectedBlock.transform.rotation = Quaternion.LookRotation (hit.normal) * Quaternion.Euler (90, 0, 0);
					if(Input.GetButtonDown("w") {
						SelectedBlock.transform.rotation  *= Quaternion.Euler(New Vector3(90,0,0));
					}
					
					if(Input.GetButtonDown("s") {
						SelectedBlock.transform.rotation  *= Quaternion.Euler(New Vector3(-90,0,0));
					}
					
					if(Input.GetButtonDown("a") {
						SelectedBlock.transform.rotation  *= Quaternion.Euler(New Vector3(0,0,90));
					}
					
					if(Input.GetButtonDown("d") {
						SelectedBlock.transform.rotation  *= Quaternion.Euler(New Vector3(0,0,-90));
					}
					
					if(Input.GetButtonDown("q") {
						SelectedBlock.transform.rotation  *= Quaternion.Euler(New Vector3(0,90,0));
					}
					
					if(Input.GetButtonDown("e") {
						SelectedBlock.transform.rotation  *= Quaternion.Euler(New Vector3(0,-90,0));
					}
					if(Input.GetMouseButtonDown(0)&&(hit.gameObject.GetComponent<Block>())) { //Тут поменял, не уверен, что будет работать на всем
						hasActivePart = false;
						SelectedBlock = null;
						SelectedBlock.GetComponent<Collider>().enabled = true;
						//SelectedBlock.GetComponent<Rigidbody>()
						var  joint = SelectedBlock.AddComponent<FixedJoint>(); //Связываем объекты (проверить, можно ли их двигать после этого). Связывание нужно для того, чтобы записать в сохранение наличие связи между объектами
						joint.connectedBody = hit.rigidbody;
						
					}
				}
			}
		}
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
		block.GetComponent<Rigidbody> ().isKinematic = true;
		LastBlock = block;

		if (firstPart) {
			firstPart = false;
			realRoot = new GameObject ();
			realRoot.name = "RobotRoot";
			hasActivePart = false;
			edCam.target.transform.position = block.transform.position;
		} else {
					SelectedBlock = block;
					hasActivePart = true;
					SelectedBlock.GetComponent<Collider>().enabled = false;
				}

		block.transform.parent = realRoot.transform;

		
	}
}

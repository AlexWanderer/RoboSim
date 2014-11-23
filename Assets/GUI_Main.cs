using UnityEngine;
using System.Collections;

public class GUI_Main : MonoBehaviour {
	public GameObject main;
	public processor proc;

	void Start() {
		proc = main.GetComponent<processor> ();
	}


	void OnGUI () {
		GUI.DrawTexture (new Rect (0, 0, 128, 128), proc.Scr);


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

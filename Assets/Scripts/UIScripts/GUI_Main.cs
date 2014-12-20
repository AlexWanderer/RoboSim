using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUI_Main : MonoBehaviour {
	public GameObject main;
	public processor proc;
	public Canvas canvas;
	public Toggle pauseMode;

	public bool showGraphicOutput = false;
	public bool showConsoleWindow = false;

	public InputField  runStr;
	public Button reloadButton;

	public enum GUIMode
	{
		codeEditor,
		Menu,
		Game

	}
	public GUIMode mode;
	public GameObject selectedRobot;

	void Start() {
		//proc = main.GetComponent<processor> ();
		//selectedRobot = main;
		//runStr.image.enabled = true;
	}


	void OnGUI () {
		if (showGraphicOutput == true) {
						GUI.DrawTexture (new Rect (0, 0, 128, 128), proc.Scr);
				}

	}


	

	void Update () {
		if (mode == GUIMode.Game) { //обрабатываем нажатия клавиш в игровом режиме
			if (Input.GetKey ("tab")) {
					SetMenuMode ();

				}
			if (Input.GetKey ("f2")) {
					SetEditorMode ();
				
				}
		} else if (Input.GetKey ("f12")) {
			SetGameMode();	
			}
	}

	public void SetMenuMode() {
				mode = GUIMode.Menu;
		runStr.enabled = true;
	}

	public void SetEditorMode() {
		mode = GUIMode.codeEditor;
		runStr.enabled = true;
	}

	public void SetGameMode() {
		mode = GUIMode.Game;
		runStr.enabled = false;
		
	}

	public void UISetRobot(GameObject m) {
		main = m;
		proc = main.GetComponent<processor> ();
		selectedRobot = main;
		Global.SelectedRobot = m;
		}

	public void LoadLuaScript(string path) {
		if (proc) {
						if (proc.isRunning) {
								proc.LUA_Stop ();
						}
						if (proc.LUA_LoadAndRun (path) == false) {
								runStr.image.color = Color.red;
								//return false;
						} else {
								runStr.image.color = Color.white;
								//return true;
						}

				}// else
						//return false;

	}

	public void LoadLuaScript() {
		if (proc) {
			if(proc.isRunning) {
				proc.LUA_Stop();
				
			}
			proc.LUA_LoadAndRun(runStr.text);
			//return true;
		}
		//return false;
	}

	public void PauseToggle() {
		if (proc) {
						if (pauseMode.isOn) {
								proc.LUA_Pause ();
						} else {
								proc.LUA_Resume ();
						}
				}
		}

	public void GraphOutToggle() {
		showGraphicOutput = !showGraphicOutput;
	}

	public void ToggleGamePause() 
	{
		if (Global.GetPauseState ()) 
		{Global.SetPause (false);
				} else {
			Global.SetPause(true);}


	}


}

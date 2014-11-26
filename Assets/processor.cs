using UnityEngine;
using System.Collections;
using System;
using UniLua;

public class processor : MonoBehaviour {

	public GameObject[] Motors;
	public GameObject[] Sensors;

	public Texture2D Scr;
	private Color drawCol;

	
	public string scriptPath;
	public bool isRunning;
	
	private ILuaState _lua; // через этот объект будет производится работа с Lua
	private ThreadStatus _status; // объект для работы с конкретным скриптом
	private ILuaState _temp_state;
	private LuaState _state;

	private bool waitFlag = false;
	private float waitStartTime;
	private float waitTime;


	void Awake () {
	
	}

	void Start () {
		Scr = new Texture2D (128, 128, TextureFormat.RGB24, false);
		
		LUA_Init();
		//LUA_LoadAndRun(scriptPath);
	
	}
	

	void Update () {
	
		if (Input.GetKeyDown ("p")) {
			if (_temp_state.GetTop() > 0) _lua.Resume(null, 0);
			_lua.Resume(null, 0);
		}
		
		if (Input.GetKeyDown ("r")) { //Пусть пока запуск будет по нажатию кнопки
			LUA_LoadAndRun(scriptPath);
		}

		if (waitFlag) {

			CheckWait ();

		}
	}
	
	private bool LUA_Init() {
		_lua = LuaAPI.NewState();			
		_lua.L_OpenLibs();
		_lua.L_RequireF("io", ProcLibs.OpenIOLib, true);
		_lua.L_RequireF("graph", this.GetComponent<ProcLibs>().OpenGraphicsLib, true);
		var _thread = _lua.NewThread();
		return true;
	}
	
	private bool LUA_LoadAndRun(string path) {
		_status = _lua.L_LoadFile (path);
		if(_status == ThreadStatus.LUA_OK) {
			_lua.Resume(null, 0);
			isRunning = true;
			return true;
		} else {
			Debug.Log("Failed to run lua script.");
			Debug.Log (_status);
			isRunning = false;
			return false;
		}
	}
	
	private bool LUA_Stop() {
	
	isRunning = false;
	return true;
	}

	private void CheckWait()
	{
		if((waitStartTime + waitTime)<=Time.time)
		{
				if (_temp_state.GetTop() > 0) _lua.Resume(null, 0);

			if (_lua.Status == ThreadStatus.LUA_OK) {
				waitFlag = false;
			} 
		}
	}

}

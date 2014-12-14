using UnityEngine;
using System.Collections;
using System;
using UniLua;

public class processor : MonoBehaviour {

	public GameObject[] Motors;
	public GameObject[] Sensors;

	public string[] ConsoleOut;
	
	public BlockManager manager;

	public Texture2D Scr;
	private Color drawCol;

	
	public string scriptPath;
	public bool isRunning;

	//кишки наружу, ага
	public ILuaState _lua; // через этот объект будет производится работа с Lua
	public ThreadStatus _status; // объект для работы с конкретным скриптом
	public ILuaState _temp_state;
	public LuaState _state;
	public LuaState _thread;

	public bool waitFlag = false;
	public float waitStartTime;
	public float waitTime;


	void Awake () {
	
	}

	void Start () {
		Scr = new Texture2D (128, 128, TextureFormat.RGB24, false);
		manager = GetComponent<BlockManager> ();
		LUA_Init();
		//LUA_LoadAndRun(scriptPath);
	
	}
	

	void Update () {
	
		//if (Input.GetKeyDown ("p")) {
		//	if (_temp_state.GetTop() > 0) _lua.Resume(null, 0);
		//	_lua.Resume(null, 0);
		//}
		
		//if (Input.GetKeyDown ("r")) { //Пусть пока запуск будет по нажатию кнопки
		//	LUA_LoadAndRun(scriptPath);
		//}

		if (waitFlag) {

			CheckWait ();

		}
	}
	
	private bool LUA_Init() {
		_lua = LuaAPI.NewState();			
		_lua.L_OpenLibs();
		_lua.L_RequireF("io", this.GetComponent<ProcLibs>().OpenIOLib, true);
		_lua.L_RequireF("graph", this.GetComponent<ProcLibs>().OpenGraphicsLib, true);
		_lua.L_RequireF("sys", this.GetComponent<ProcLibs>().OpenSysLib, true);
		_thread = _lua.NewThread();

		return true;
	}
	
	public bool LUA_LoadAndRun(string path) {
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
	
	public bool LUA_Stop() {
		//_lua.YieldK (_lua.GetTop(), 0, null); //стопим поток
		LUA_Init ();
		//	_thread.

		isRunning = false;
		return true;
	}

	public bool LUA_Pause() {
		if (isRunning) // Второй раз паузить уже не будем, хватит.
		{
			if (_lua.Status == ThreadStatus.LUA_YIELD) { //итак уже в йелде, чо жопу мучать.
				waitFlag = false;
				return true;
			}
		_lua.YieldK (_lua.GetTop (), 0, null); //стопим поток
		isRunning = false;
		return true;
		}
		return false;
	}

	public bool LUA_Resume() {
		if (_temp_state.GetTop() > 0) _lua.Resume(null, 0);
		Debug.Log (_lua.Status);
		isRunning = true;
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

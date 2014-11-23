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

	private ILuaState _lua; // через этот объект будет производится работа с Lua
	private ThreadStatus _status; // объект для работы с конкретным скриптом
	private ILuaState _temp_state;
	private LuaState _state;

	private bool waitFlag = false;
	private float waitStartTime;
	private float waitTime;

	private int pauseCnt = 0;

	void Awake () {

		_lua = LuaAPI.NewState();			
		_lua.L_OpenLibs();
		_lua.L_RequireF("io", OpenLib, true);
		var _thread = _lua.NewThread();
		_status = _lua.L_LoadFile (scriptPath);
		Debug.Log (_status);
		//_lua.L_DoFile (scriptPath);
		_lua.Resume(null, 0);
		//_temp_state.Resume (null, 0);
	}

	void Start () {
		Scr = new Texture2D (128, 128, TextureFormat.RGB24, false);

	}
	

	void Update () {
	
		if (Input.GetKeyDown ("a")) {
			if (_temp_state.GetTop() > 0) _lua.Resume(null, 0);
			_lua.Resume(null, 0);
		}

		if (waitFlag) {

			CheckWait ();

		}
	}

	private int OpenLib(ILuaState lua)
	{
		var define = new NameFuncPair[] // структура, описывающая все доступные методы (интерфейс Lua -> C#)
		{
			new NameFuncPair("trace", L_Trace),
			new NameFuncPair("wait", L_Wait),
			new NameFuncPair("pause", L_Pause),
			new NameFuncPair("getTime", L_GetTime),
			new NameFuncPair("getDeltaTime", L_GetDeltaTime),
			new NameFuncPair("getBrightness", L_GetBrightness),
			new NameFuncPair("getXPos", L_GetXPos),
			new NameFuncPair("getYPos", L_GetYPos),
			new NameFuncPair("getZPos", L_GetZPos),
			new NameFuncPair("getPitch", L_GetPitch),
			new NameFuncPair("getYaw", L_GetYaw),
			new NameFuncPair("getRoll", L_GetRoll),
			new NameFuncPair("move", L_Move),
			new NameFuncPair("drawPixel", L_SetPixel),
			new NameFuncPair("drawColor", L_SetColor),
			new NameFuncPair("drawDot", L_SetPixelCol),
			new NameFuncPair("redraw", L_Redraw),
		};

		lua.L_NewLib(define);
		return 1;
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


	private int L_Trace(ILuaState s)
	{
		Debug.Log("Lua trace: " + s.L_CheckString(1)+"Time:"+ Time.time.ToString()); // читаем первый параметр
		return 1; // так надо
	}

	private int L_Wait(ILuaState s)
	{

		waitTime =(float) s.L_CheckNumber (1);
		waitFlag = true;
		waitStartTime = Time.time;

		_temp_state = null;
		_temp_state = s;

		return s.YieldK (s.GetTop(), 0, null);
	}

	private int L_Pause(ILuaState s)
	{
		_temp_state = null;
		_temp_state = s;
		return s.YieldK (s.GetTop(), 0, null);

	}

	public int L_SetPixelCol(ILuaState s)
	{
		int x = (int) s.L_CheckNumber (1);
		int y = (int) s.L_CheckNumber (2);
		float R = (float) s.L_CheckNumber (3);
		float G = (float) s.L_CheckNumber (4);
		float B = (float) s.L_CheckNumber (5);
		Scr.SetPixel (x, y, new Color (R, G, B));
		return 1;
	}

	public int L_SetPixel(ILuaState s)
	{
		int x = (int) s.L_CheckNumber (1);
		int y = (int) s.L_CheckNumber (2);
		Scr.SetPixel (x, y, drawCol);
		Scr.Apply ();
		return 1;
	}

	public int L_SetColor(ILuaState s)
	{
		float R = (float) s.L_CheckNumber (1);
		float G = (float) s.L_CheckNumber (2);
		float B = (float) s.L_CheckNumber (3);
		drawCol = new Color (R, G, B);
		return 1;
	}

	public int L_Redraw(ILuaState s)
	{
		Scr.Apply ();
		return 1;
	}

	public int L_GetBrightness(ILuaState s)
	{
		int index = (int)s.L_CheckNumber(1);
		float br = Sensors [index-1].GetComponent<ColSensor> ().GetBrightness ();
		s.PushNumber ((double)br);
		return 1;
	}


	public int L_GetTime(ILuaState s)
	{
		s.PushNumber ((double)Time.time);
		return 1;
	}

	private int L_Move(ILuaState s)
	{
		var index = s.L_CheckNumber(1);
		var dir = s.L_CheckNumber(2);
		var speed = s.L_CheckNumber(3);

		JointMotor motor = Motors[(int)index-1].hingeJoint.motor;
		motor.targetVelocity = (float)speed*(float)dir;
		Motors[(int)index - 1].hingeJoint.motor = motor;

		return 1; // так надо
	}

	private int L_GetDeltaTime(ILuaState s)
	{

		s.PushNumber(Time.deltaTime);
		return 1; // так надо
	}

	private int L_GetXPos(ILuaState s)
	{

		s.PushNumber(gameObject.transform.position.x);
		return 1; // так надо
	}

	private int L_GetYPos(ILuaState s)
	{

		s.PushNumber(gameObject.transform.position.y);
		return 1; // так надо
	}

	private int L_GetZPos(ILuaState s)
	{

		s.PushNumber(gameObject.transform.position.z);
		return 1; // так надо
	}

	private int L_GetPitch(ILuaState s)
	{

		s.PushNumber(gameObject.transform.rotation.eulerAngles.x);
		return 1; // так надо
	}

	private int L_GetYaw(ILuaState s)
	{

		s.PushNumber(gameObject.transform.rotation.eulerAngles.y);
		return 1; // так надо
	}

	private int L_GetRoll(ILuaState s)
	{

		s.PushNumber(gameObject.transform.rotation.eulerAngles.z);
		return 1; // так надо
	}

}

using System;
using System.Collections;
using UnityEngine;
using UniLua;

public class LuaScriptController : MonoBehaviour {
	public	string		LuaScriptFile = "framework/main.lua";

	private ILuaState 	Lua;
	private ILuaState _temp_state;

	private bool runFlag = true;

	private int			AwakeRef;
	private int			StartRef;
	private int			UpdateRef;
	private int			LateUpdateRef;
	private int			FixedUpdateRef;

	void Awake() {
		Debug.Log("LuaScriptController Awake");

		if( Lua == null )
		{
			Lua = LuaAPI.NewState();
			Lua.L_OpenLibs();
			Lua.L_RequireF ("mylib", OpenLib, true);


			var status = Lua.L_LoadFile( LuaScriptFile );
			if( status != ThreadStatus.LUA_OK )
			{
				throw new Exception( Lua.ToString(-1) );
			}

			if( ! Lua.IsTable(-1) )
			{
				throw new Exception(
					"framework main's return value is not a table" );
			}

			AwakeRef 		= StoreMethod( "awake" );
			StartRef 		= StoreMethod( "start" );
			UpdateRef 		= StoreMethod( "update" );
			LateUpdateRef 	= StoreMethod( "late_update" );
			FixedUpdateRef 	= StoreMethod( "fixed_update" );

			Lua.Pop(1);
			Debug.Log("Lua Init Done");
		}

		CallMethod( AwakeRef );
	}

// AssetBundle testx.unity3d is built for StandaloneWindows
#if UNITY_STANDALONE
	IEnumerator Start() {
		CallMethod( StartRef );

		// -- sample code for loading binary Asset Bundles --------------------
		String s = "file:///"+Application.streamingAssetsPath+"/testx.unity3d";
		WWW www = new WWW(s);
		yield return www;
		if(www.assetBundle.mainAsset != null) {
			TextAsset cc = (TextAsset)www.assetBundle.mainAsset;
			var status = Lua.L_LoadBytes(cc.bytes, "test");
			if( status != ThreadStatus.LUA_OK )
			{
				throw new Exception( Lua.ToString(-1) );
			}
			status = Lua.PCall( 0, 0, 0);
			if( status != ThreadStatus.LUA_OK )
			{
				throw new Exception( Lua.ToString(-1) );
			}
			Debug.Log("---- call done ----");
		}
	}
#else
	void Start() {
		CallMethod( StartRef );
	}
#endif

	void Update() {
		if (runFlag) {
			CallMethod (UpdateRef);

		} else {
			if (Input.GetKeyDown ("a")) {
				_temp_state.GetTop (); 
				Lua.Resume (_temp_state, 1);
				//Debug.Log ("pressed");
				runFlag = true;
			}
		}

	}

	void LateUpdate() {
		if (runFlag) {
			CallMethod (LateUpdateRef);
		}
	}

	void FixedUpdate() {
		if (runFlag) {
			CallMethod (FixedUpdateRef);

		}
	}

	private int StoreMethod( string name )
	{
		Lua.GetField( -1, name );
		if( !Lua.IsFunction( -1 ) )
		{
			throw new Exception( string.Format(
				"method {0} not found!", name ) );
		}
		return Lua.L_Ref( LuaDef.LUA_REGISTRYINDEX );
	}

	private void CallMethod( int funcRef )
	{
		Lua.RawGetI( LuaDef.LUA_REGISTRYINDEX, funcRef );

		// insert `traceback' function
		var b = Lua.GetTop();
		Lua.PushCSharpFunction( Traceback );
		Lua.Insert(b);

		var status = Lua.PCall( 0, 0, b );
		if( status != ThreadStatus.LUA_OK )
		{
			Debug.LogError( Lua.ToString(-1) );
		}

		// remove `traceback' function
		Lua.Remove(b);
	}

	private static int Traceback(ILuaState lua) {
		var msg = lua.ToString(1);
		if(msg != null) {
			lua.L_Traceback(lua, msg, 1);
		}
		// is there an error object?
		else if(!lua.IsNoneOrNil(1)) {
			// try its `tostring' metamethod
			if(!lua.L_CallMeta(1, "__tostring")) {
				lua.PushString("(no error message)");
			}
		}
		return 1;
	}

	private int OpenLib(ILuaState lua)
	{
		var define = new NameFuncPair[] {
			new NameFuncPair ("trace", L_Trace),
			new NameFuncPair ("pause", L_Pause),
		};

		lua.L_NewLib (define);
		return 1;
	}




	private int L_Pause(ILuaState s)
	{
		runFlag = false;
		_temp_state = s;
		return s.YieldK (s.GetTop (), 0, null);
	}

	private int L_Trace(ILuaState s)
	{
		Debug.Log ("Lua trace: " + s.L_CheckString (1));
		return 1;
	}





}


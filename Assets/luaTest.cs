using UnityEngine;
using System.Collections;
using UniLua;
using System;

public class luaTest : MonoBehaviour {
	public	string		LuaScriptFile = "LuaScripts/main.lua";

	private ILuaState _lua; // через этот объект будет производится работа с Lua
	private ThreadStatus _status; // объект для работы с конкретным скриптом
	private ThreadStatus _thread;
	private ILuaState _temp_state;
	private int mainRef;

	private bool _pauseFlag = false;


	void Awake () {

		if (_lua == null) {
			_lua = LuaAPI.NewState ();	 // создаем

			_lua.L_OpenLibs ();
			_lua.L_RequireF ("mylib", OpenLib, true);
			_status = _lua.L_DoFile (LuaScriptFile);

			//	throw new Exception( _lua.ToString(-1) );

			if (_status != ThreadStatus.LUA_OK) {
					Debug.LogError ("Error parsing lua code");
			}

			if( ! _lua.IsTable(-1) )
			{
						throw new Exception("framework main's return value is not a table" );
			}


			//	_lua.L_LoadFile (LuaScriptFile);

			mainRef = StoreMethod ("main");
			//	_lua.Resume (null, 0);
			//	_lua.Pop (1);

		}

	}
	

	void Update () {
		if (_pauseFlag == false) {
			//	Debug.Log (_pauseFlag);
			CallMethod (mainRef);
		}
		if (Input.GetKeyDown ("a")) {
			_temp_state.GetTop ();
			_lua.Resume (_temp_state, 0);
			_pauseFlag = false;
			if (_temp_state.GetTop () > 0) {

				//Debug.Log ("yaya");
			}
			//_lua.Re
		}

	}


	private int pauseTest(ILuaState s)
	{
		// здесь добавляем нужные callback'и и т.п.
		_pauseFlag = true;
		Debug.Log (_pauseFlag);
		_temp_state = s; // сохраняем ILuaState в приватный член класса
		return s.YieldK(s.GetTop(), 0, null); // указываем Lua, что оно должно отдать управление шарпам
	}
	 


	private int OpenLib(ILuaState lua)
	{
		var define = new NameFuncPair[] // структура, описывающая все доступные методы (интерфейс Lua -> C#)
		{
			new NameFuncPair("trace", L_Trace),
			new NameFuncPair("pause", pauseTest),
		};

		lua.L_NewLib(define);
		return 1;
	}

	private int L_Trace(ILuaState s)
	{
		Debug.Log("Lua trace: " + s.L_CheckString(1)); // читаем первый параметр
		return 1; // так надо
	}




	private int StoreMethod( string name )
	{
		_lua.GetField( -1, name );
		if( !_lua.IsFunction( -1 ) )
		{
			throw new Exception( string.Format(
				"method {0} not found!", name ) );
		}
		return _lua.L_Ref( LuaDef.LUA_REGISTRYINDEX );
	}

	private void CallMethod( int funcRef )
	{
		_lua.RawGetI( LuaDef.LUA_REGISTRYINDEX, funcRef );

		// insert `traceback' function
		var b = _lua.GetTop();
		_lua.PushCSharpFunction( Traceback );
		_lua.Insert(b);

		var status = _lua.PCall( 0, 0, b );
		if( status != ThreadStatus.LUA_OK )
		{
			Debug.LogError( _lua.ToString(-1) );
		}

		// remove `traceback' function
		_lua.Remove(b);
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







}

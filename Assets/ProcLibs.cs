using UnityEngine;
using System.Collections;
using UniLua;

public class ProcLibs : MonoBehaviour {

	public int L_GetTime(ILuaState s)
	{
		s.PushNumber ((double)Time.time);
		return 1;
	}

	



}

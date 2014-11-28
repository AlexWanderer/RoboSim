using UnityEngine;
using System.Collections;
using UniLua;

public class ProcLibs : MonoBehaviour {
	Texture2D Scr;
	processor proc;
	Color drawCol;
	BlockManager manager;


	void Start() {
		Scr = this.GetComponent<processor>().Scr;
		proc = this.GetComponent<processor> ();
		manager = GetComponent<BlockManager> ();
	}
	
	public int OpenSysLib(ILuaState lua)
	{
		var define = new NameFuncPair[] // структура, описывающая все доступные методы (интерфейс Lua -> C#)
		{
			new NameFuncPair("trace", L_Trace),
			new NameFuncPair("wait", L_Wait),
			new NameFuncPair("pause", L_Pause),
			new NameFuncPair("getTime", L_GetTime),
			new NameFuncPair("getDeltaTime", L_GetDeltaTime),
		
		};
		lua.L_NewLib(define);
		return 1;
		
	}
	
	public int OpenIOLib(ILuaState lua)
	{
		var define = new NameFuncPair[] // структура, описывающая все доступные методы (интерфейс Lua -> C#)
		{
			new NameFuncPair("getBrightness", L_GetBrightness),
			new NameFuncPair("getXPos", L_GetXPos),
			new NameFuncPair("getYPos", L_GetYPos),
			new NameFuncPair("getZPos", L_GetZPos),
			new NameFuncPair("getPitch", L_GetPitch),
			new NameFuncPair("getYaw", L_GetYaw),
			new NameFuncPair("getRoll", L_GetRoll),
			new NameFuncPair("move", L_Move),
			new NameFuncPair("motorSpeed", L_MotorSpeed),
			new NameFuncPair("getCSBr", L_CSBrightness),
			new NameFuncPair("getUSDist", L_USDistance),
			new NameFuncPair("setThrust", L_SetThrust),
			new NameFuncPair("toggleThruster", L_TgThrust),
			new NameFuncPair("toggleThruster", L_TgThrust),
			new NameFuncPair("setLight", L_SetLight),
			new NameFuncPair("setLightColor", L_SetLightColor),
			new NameFuncPair("setLightIntensity", L_SetLightIntensity),
			new NameFuncPair("toggleGrabber", L_ToggleGrabber),
		};

		lua.L_NewLib(define);
		return 1;
	}
	
	public int OpenGraphicsLib(ILuaState lua)
	{
		var define = new NameFuncPair[] // структура, описывающая все доступные методы (интерфейс Lua -> C#)
		{
			new NameFuncPair("drawPixel", L_SetPixel),
			new NameFuncPair("drawColor", L_SetColor),
			new NameFuncPair("drawDot", L_SetPixelCol),
			new NameFuncPair("redraw", L_Redraw), 
			new NameFuncPair("clrscr", L_ClearScreen),
			new NameFuncPair("drawLine", L_DrawLine),
			new NameFuncPair("drawRect", L_DrawRect),
		};

		lua.L_NewLib(define);
		return 1;
	}
	
	

	private int L_Trace(ILuaState s)
	{
		Debug.Log("Lua trace: " + s.L_CheckString(1)); // читаем первый параметр
		return 1; // так надо
	}

	private int L_Wait(ILuaState s)
	{

		proc.waitTime =(float) s.L_CheckNumber (1);
		proc.waitFlag = true;
		proc.waitStartTime = Time.time;

		proc._temp_state = null;
		proc._temp_state = s;

		return s.YieldK (s.GetTop(), 0, null);
	}

	private int L_Pause(ILuaState s)
	{
		proc._temp_state = null;
		proc._temp_state = s;
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
	
	public int L_DrawRect(ILuaState s) //Получаем левую нижнюю точку прямоугольника и рисуем его
	{
		int x = (int) s.L_CheckNumber(1);
		int y = (int)s.L_CheckNumber(2);
		int width = (int)s.L_CheckNumber(3);
		int height = (int)s.L_CheckNumber(4);
		//int fill = (int)s.L_CheckNumber (5);

		for(int c = y;c< (y + height);c++){ //рисуем вертикальные линии
				Scr.SetPixel (x, c, drawCol);
				Scr.SetPixel (x + width, c, drawCol);
			//	if (fill > 0) {

			//}
			}
		
		for(int c = x; c< (x + width);c++){
				Scr.SetPixel (c, y, drawCol);
				Scr.SetPixel (c, y + height, drawCol);
			}

		Scr.Apply ();
		return 1;
	}

	public int L_DrawLine(ILuaState s)
	{
		int x0 = (int) s.L_CheckNumber(1);
		int y0 = (int)s.L_CheckNumber(2);
		int x1 = (int)s.L_CheckNumber(3);
		int y1 = (int)s.L_CheckNumber(4);
		
		//Обработаем частные случаи (вертикальная / горизонтальная линии, диагонали под 45 градусов)
		//int c = 0;
		//рисуем вертикальные
		if(x1==x0) {
		int d = y1 - y0;
			if(d>=0) {
				for(int c = y1;c< y0;c++){
				Scr.SetPixel (x0, c, drawCol);
			}
			} else {
				for(int c = y0;c< y1;c++){
					Scr.SetPixel (x0, c, drawCol);
				}
			}
			Scr.Apply ();
			return 1;
		}

		
		//Рисуем горизонтальные
		if(y1==y0) {
		int d = x1 - x0;
			if(d>0) {
			for(int c = x1;c< x0;c++){
				Scr.SetPixel (x0, c, drawCol);
			}
			} else {
				for(int c = x0;c< x1;c++){
					Scr.SetPixel (c, y0, drawCol);
				}
			}

			Scr.Apply ();
			return 1;
		}
		

		
		//Рисуем диагонали
		//if( (x0 - x1) == (y0 - y1)) 
		//int del = y0 - y1;
		//int s = 
		

		//Изменения координат
        int dx = (x1 > x0) ? (x1 - x0) : (x0 - x1);
        int dy = (y1 > y0) ? (y1 - y0) : (y0 - y1);
        //Направление приращения
        int sx = (x1 >= x0) ? (1) : (-1);
        int sy = (y1 >= y0) ? (1) : (-1);
		
		
		
		
		
        if (dy < dx)
        {
            int d = (dy << 1) - dx;
            int d1 = dy << 1;
            int d2 = (dy - dx) << 1;
			Scr.SetPixel (x0, y0, drawCol);
            int x = x0 + sx;
            int y = y0;
			for (int i = 1; i <= dx; i++) {
				if (d > 0) {
					d += d2;
					y += sy;
				} else {
					d += d1;
				}
					
				Scr.SetPixel (x, y, drawCol);
				x++;
			}
                
        }
            else
            {
                int d = (dx << 1) - dy;
                int d1 = dx << 1;
                int d2 = (dx - dy) << 1;
				Scr.SetPixel (x0, y0, drawCol);
                int x = x0;
                int y = y0 + sy;
                for (int i = 1; i <= dy; i++)
                {
                    if (d > 0)
                    {
                        d += d2;
                        x += sx;
                    }
                    else
					d += d1;
					
                    Scr.SetPixel (x, y, drawCol);
                    y++;
					
                }
            }

	
	Scr.Apply ();
	return 1;
	}
	
	public int L_ClearScreen(ILuaState s)
	{
	for (int x = 0; x < 128; x++)
	{
		for (int y = 0; y < 128; y++)
		{
		Scr.SetPixel (x, y, drawCol);
		}
	
	}
		
	Scr.Apply();
	return 1;
	}
	
	
	
	public int L_GetBrightness(ILuaState s)
	{
		int index = (int)s.L_CheckNumber(1);
		float br = 0f;
		//Sensors [index - 1].SendMessage ("GetBrightness");
		ColSensor Col = proc.Sensors [index - 1].GetComponent<ColSensor> ();
		if (!Col) {
			br = proc.Sensors [index - 1].GetComponent<CamSensor> ().GetBrightness ();

		} else {
			br = Col.GetBrightness ();

		}
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

		JointMotor motor = proc.Motors[(int)index-1].hingeJoint.motor;
		motor.targetVelocity = (float)speed*(float)dir;
		proc.Motors[(int)index - 1].hingeJoint.motor = motor;

		return 1; // так надо
	}

	private int L_GetDeltaTime(ILuaState s)
	{

		s.PushNumber(Time.deltaTime);
		return 1; 
	}

	private int L_GetXPos(ILuaState s)
	{

		s.PushNumber(gameObject.transform.position.x);
		return 1; 
	}

	private int L_GetYPos(ILuaState s)
	{

		s.PushNumber(gameObject.transform.position.y);
		return 1; 
	}

	private int L_GetZPos(ILuaState s)
	{

		s.PushNumber(gameObject.transform.position.z);
		return 1; 
	}

	private int L_GetPitch(ILuaState s)
	{

		s.PushNumber(gameObject.transform.rotation.eulerAngles.x);
		return 1; 
	}

	private int L_GetYaw(ILuaState s)
	{

		s.PushNumber(gameObject.transform.rotation.eulerAngles.y);
		return 1; 
	}

	private int L_GetRoll(ILuaState s)
	{

		s.PushNumber(gameObject.transform.rotation.eulerAngles.z);
		return 1; 
	}

	private int L_MotorSpeed(ILuaState s)
	{	
		var ID = s.L_CheckString (1);
		var dir = s.L_CheckNumber(2);
		var speed = s.L_CheckNumber(3);
		//Debug.Log ("ROWROW");
		GameObject motor = manager.GetBlockByIDAndType (ID, BlockInfo.BlockType.Motor);

		if (motor != null) {
			motor.GetComponent<Motor> ().SetContSpeed ((float)speed * (float)dir);
		}

		return 1;
	}

	private int L_SetLightColor(ILuaState s)
	{	
		var ID = s.L_CheckString (1);
		float r = (float) s.L_CheckNumber(2);
		float g = (float) s.L_CheckNumber(3);
		float b = (float) s.L_CheckNumber(4);
		//Debug.Log ("ROWROW");
		GameObject motor = manager.GetBlockByIDAndType (ID, BlockInfo.BlockType.Light);

		if (motor != null) {
			motor.GetComponent<LightBlock> ().SetColor(new Color(r,g,b));
		}

		return 1;
	}

	private int L_SetLightIntensity(ILuaState s)
	{	
		var ID = s.L_CheckString (1);
		float i = (float) s.L_CheckNumber(2);

		GameObject motor = manager.GetBlockByIDAndType (ID, BlockInfo.BlockType.Light);

		if (motor != null) {
			motor.GetComponent<LightBlock> ().SetIntensity(i);
		}

		return 1;
	}

	private int L_SetLight(ILuaState s)
	{	
		var ID = s.L_CheckString (1);
		float f = (float) s.L_CheckNumber(2);

		GameObject motor = manager.GetBlockByIDAndType (ID, BlockInfo.BlockType.Light);

		if (motor != null) {
			if (f < 0.7) {
				motor.GetComponent<LightBlock> ().SetLight (false);
			} else {
				motor.GetComponent<LightBlock> ().SetLight (true);
			}
		}

		return 1;
	}
	

	private int L_CSBrightness(ILuaState s) 
	{
		string id = s.L_CheckString(1);
		float br = 0f;
		//Sensors [index - 1].SendMessage ("GetBrightness");
		GameObject cs = manager.GetBlockByIDAndType (id, BlockInfo.BlockType.ColorSensor);

		if (cs != null) {
			br = cs.GetComponent<Color_Sensor>().GetBrightness();
		}
		s.PushNumber ((double)br);

		return 1;
	}

	private int L_USDistance(ILuaState s) 
	{
		//Debug.Log ("Call");
		string id = s.L_CheckString(1);
		float dist = 0f;
		//Sensors [index - 1].SendMessage ("GetBrightness");
		GameObject us = manager.GetBlockByIDAndType (id, BlockInfo.BlockType.USSensor);

		if (us != null) {
			dist = us.GetComponent<USSensor>().Measure();
			s.PushNumber ((double)dist);
			return 1;
		}

		s.PushNumber ((double)dist);
		return 1;
	}

	private int L_ToggleGrabber(ILuaState s) 
	{
		string id = s.L_CheckString(1);
		GameObject gr = manager.GetBlockByIDAndType (id, BlockInfo.BlockType.Grabber);

		if (gr != null) {
			gr.GetComponent<HypnoGrabber>().Grab();
		}

		return 1;
	}


	private int L_SetThrust(ILuaState s) 
	{
		string id = s.L_CheckString(1);
		float thrust =(float) s.L_CheckNumber (2);

		GameObject thr = manager.GetBlockByIDAndType (id, BlockInfo.BlockType.Thruster);

		if (thr != null) {
			thr.GetComponent<Thruster>().SetThrust(thrust);
		}

		return 1;
	}


	
	private int L_TgThrust(ILuaState s) 
	{
		string id = s.L_CheckString(1);
		//float thrust =(float) s.L_CheckNumber (2);

		GameObject thr = manager.GetBlockByIDAndType (id, BlockInfo.BlockType.Thruster);

		if (thr != null) {
			thr.GetComponent<Thruster>().ToggleThruster();
		}

		return 1;
	}

}

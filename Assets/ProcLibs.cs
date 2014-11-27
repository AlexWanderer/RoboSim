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
	
	public int OpenIOLib(ILuaState lua)
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
			new NameFuncPair("motorSpeed", L_MotorSpeed),
			new NameFuncPair("getCSBr", L_CSBrightness),
			new NameFuncPair("SetThrust", L_SetThrust),
			new NameFuncPair("ToggleThruster", L_TgThrust),
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
		};

		lua.L_NewLib(define);
		return 1;
	}
	
	

	private int L_Trace(ILuaState s)
	{
		Debug.Log("Lua trace: " + s.L_CheckString(1)+"Time:"+ Time.time.ToString()); // читаем первый параметр
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
			for(int c = y1;y< y0;c++){
				Scr.SetPixel (x0, c, drawCol);
			}
			} else {
				for(int c = y0;y< y1;c++){
					Scr.SetPixel (x0, c, drawCol);
				}
			}
		}
		Scr.Apply ();
		return 1;
		
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
		}
		
		Scr.Apply ();
		return 1;
		
		//Рисуем диагонали
		//if( (x0 - x1) == (y0 - y1)) 
		//int del = y0 - y1;
		//int s = 
		
		
		//
		
		
		
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
            PutPixel(g, clr, x0, y0, 255);
            int x = x0 + sx;
            int y = y0;
            for (int i = 1; i <= dx; i++)
            {
                if (d > 0)
                {
                    d += d2;
                    y += sy;
                }
                else
                    d += d1;
					
					Scr.SetPixel (x, y, drawCol);
                    x++;
                
            }
            else
            {
                int d = (dx << 1) - dy;
                int d1 = dx << 1;
                int d2 = (dx - dy) << 1;
                PutPixel(g, clr, x0, y0, 255);
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
		}
	
	Scr.Apply ();
	return 1;
	}
	
	public int L_ClearScreen(ILuaState s)
	{
	//Scr.SetPixels( Надо дописать, автокомплита нет.
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
		float thrust =(float) s.L_CheckNumber (2);

		GameObject thr = manager.GetBlockByIDAndType (id, BlockInfo.BlockType.Thruster);

		if (thr != null) {
			thr.GetComponent<Thruster>().ToggleThruster();
		}

		return 1;
	}

}

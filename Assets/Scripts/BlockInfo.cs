using UnityEngine;
using System.Collections;

public class BlockInfo {
	public string BlockID = "null";
	public string BlockName = "null block";
	public GameObject BlockObj;
	public BlockType Type;
	public enum BlockType
	{
		Structure,
		Main,
		Wheel,
		Motor,
		Battery,
		Servo,
		Thruster,
		USSensor,
		IRSensor,
		LaserSensor,
		ColorSensor,
		LightSensor,
		LaserMatrix,
		SunBattery,
		Grabber,
		Forcer,
		Light

	};


}

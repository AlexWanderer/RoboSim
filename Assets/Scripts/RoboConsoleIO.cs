using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoboConsoleIO : MonoBehaviour 
{
	public List<string> output = new List<string> ();
	public string input;
	public GameObject thisRobot;
	public processor proc;
	//public ProcIOhandler IOHandler;
	
	
	void Start() 
	{
		thisRobot = gameObject;
		proc = thisRobot.GetComponent<processor>();
		//IOHandler = thisRobot.GetComponent<ProcIOhandler>();
	}
	
	public void AddString(string str) 
	{
		output.Add(str);
	
	}
	
	public void SetInput(string str) 
	{
		output.Add(str); // ������� ������ � ������� 
		//IOhandler.SetInputString(str);
		//Processor.RemovePause(PauseType.IOPause); //���� ���� ����� �� �����, ������� � ���, ����� ��������� ��� ������� ����� �������.
		//if(UI.SelectedRobot.GetComponent<
	}
	
	public string GetSpecificString(int index) 
	{
		return output [index];
	}
	


}
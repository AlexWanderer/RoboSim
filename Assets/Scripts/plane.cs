using UnityEngine;
using System.Collections;
using System;
using UniLua;

public class plane : MonoBehaviour {

public float ro; // ��������� �������, ����� ������ �������� �� ������, �� ��� �����
public float Cx0;  // ����. �������� ������������� ��� �������������� �������������� ������ � ������� �����
public float wingArea; // ������� �����
public float wingspan; // ������ �����
public float fuseHeight;
public float fuseLength;
public float vStabArea;

public float Cy; // ����. ��������� ����. (��� �������, ������� � ������� animation curve)

public GameObject aircraft;
private float vel;
private float AoA;
private float slip; // ���������� (����)
private float lambda; // �������������� ��������� �����
private float Cx;
private float Z;  // ������� ���� (�����)
private float Zf; // ������� ����, ����������� �� �������
private float ZvS; // ������� ����, ����������� �� ����
private float CZf; // ����. ������������� �������� ��� ����������
private float CvS;
//private float 
public AnimationCurve CyCurve;

private float dynPress; // ���������� �����

public float liftForce;
public float dragForce;

void Start() 
{
	lambda = (wingspan * wingspan)/wingArea; // ������� �������������� ���������
	GetAngles(); // ������, ��� ����� �������� ���� ������.
}

void FixedUpdate() 
{
	CalcForces();
		//Debug.DrawRay (aircraft.transform.position, aircraft.transform.TransformDirection( new Vector3 (1, 0, 0)));
		Debug.DrawRay (aircraft.transform.position, aircraft.transform.TransformDirection( new Vector3 (0, 0, 1)));
}

float CalculateCy(float AoA) 
{
	float cy = CyCurve.Evaluate(AoA); 
	return cy;
}

void GetAngles() // �������� ���� ���������� � ���� ����� ��������
{
	var v = aircraft.GetComponent<Rigidbody> ().velocity;
		v = aircraft.transform.InverseTransformDirection(v);
		AoA = Vector2.Angle (new Vector2 (v.x, v.z), new Vector2 (aircraft.transform.TransformDirection (new Vector3 (1, 0, 0)).x, aircraft.transform.TransformDirection (new Vector3 (1, 0, 0)).z));
		Debug.DrawRay (aircraft.transform.position,v);
		Debug.Log (v.z);
}

void CalcForces()
{
	GetAngles();
	vel = aircraft.GetComponent<Rigidbody>().velocity.magnitude;
	Cy = CalculateCy(AoA);
	liftForce = 0.5f * Cy * ro * vel * vel *wingArea; //������� ��� ���������� ��������� ����
	dynPress = 0.5f * vel * vel * ro;
	Cx = Cx0 + Cy/(1.4f * lambda);
	dragForce = Cx * dynPress * wingArea; // ���� �������� �������������
	
	CZf =(float) (0.28f * 0.8 * fuseHeight * fuseLength * slip)/wingArea;
	Zf = CZf * dynPress * wingArea; // � ������� ����� ������� �� �����������?
	CvS = 1.64f * slip;
	ZvS = CvS * vStabArea * dynPress;
	Z = ZvS + Zf;

}

}
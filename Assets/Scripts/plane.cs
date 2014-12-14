using UnityEngine;
using System.Collections;
using System;
using UniLua;

public class plane : MonoBehaviour {

public float ro; // Плотность воздуха, можно задать функцией от высоты, но это потом
public float Cx0;  // Коэф. лобового сопротивления при установившемся горизонтальном полете с нулевым альфа
public float wingArea; // Площадь крыла
public float wingspan; // размах крыла
public float fuseHeight;
public float fuseLength;
public float vStabArea;

public float Cy; // Коэф. подъемной силы. (Это функция, сделать с помощью animation curve)

public GameObject aircraft;
private float vel;
private float AoA;
private float slip; // Скольжение (угол)
private float lambda; // геометрическое удлинение крыла
private float Cx;
private float Z;  // Боковая сила (сумма)
private float Zf; // Боковая сила, действующая на фюзеляж
private float ZvS; // Боковая сила, действующая на киль
private float CZf; // Коэф. Сопротивления фюзеляжа при скольжении
private float CvS;
//private float 
public AnimationCurve CyCurve;

private float dynPress; // Скоростной напор

public float liftForce;
public float dragForce;

void Start() 
{
	lambda = (wingspan * wingspan)/wingArea; // Считаем геометрическое удлинение
	GetAngles(); // Спорно, все равно вероятно нули выйдут.
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

void GetAngles() // Получаем угол скольжения и угол атаки самолета
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
	liftForce = 0.5f * Cy * ro * vel * vel *wingArea; //формула для вычисления подъемной силы
	dynPress = 0.5f * vel * vel * ro;
	Cx = Cx0 + Cy/(1.4f * lambda);
	dragForce = Cx * dynPress * wingArea; // сила лобового сопротивления
	
	CZf =(float) (0.28f * 0.8 * fuseHeight * fuseLength * slip)/wingArea;
	Zf = CZf * dynPress * wingArea; // А площадь крыла случаем не сокращается?
	CvS = 1.64f * slip;
	ZvS = CvS * vStabArea * dynPress;
	Z = ZvS + Zf;

}

}
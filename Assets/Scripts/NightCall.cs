using UnityEngine;
using System.Collections;

public class NightCall : MonoBehaviour {
	public GameObject Sun;
	public GameObject EzdunSpawn;
	public GameObject Ezdun;
	public GameObject EzdunPref;
	private float timer;
	private float maxTime = 12f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer += Time.fixedDeltaTime;
		if (timer > maxTime) 
		{
			Ezdun.GetComponent<Rigidbody>().position =EzdunSpawn.transform.position;
			Destroy(Ezdun);
			Invoke("SpawnEzdun",0.1f);
			timer = 0f;
		
		}
		//Quaternion sAngle = Sun.transform.rotation;
		//sAngle.eulerAngles = new Vector3(sAngle.eulerAngles.x + 0.1f,-30,6);
		Sun.transform.rotation = Quaternion.Euler(new Vector3(Time.time * 10f,0f,30));
		//Sun.transform.rotation = sAngle;
	}

	void SpawnEzdun() {
		Ezdun = (GameObject) Instantiate(EzdunPref,EzdunSpawn.transform.position,Quaternion.Euler(-90,180,0));
	}
}

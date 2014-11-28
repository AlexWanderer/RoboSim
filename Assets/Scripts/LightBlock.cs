using UnityEngine;
using System.Collections;

public class LightBlock : Block {
	public Light light;
	// Use this for initialization
	void Start () {
		
	}
	
	public void SetColor(Color col) {
		light.color = col;
	}

	public void SetLight(bool a) {
		light.enabled = a;
	}

	public void SetIntensity(float a) {
		light.intensity = a;
	}

}

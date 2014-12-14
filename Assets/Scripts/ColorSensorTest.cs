using UnityEngine;
using System.Collections;

public class ColorSensorTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Input.GetMouseButtonDown (0)) {
			if (Physics.Raycast (ray, out hit, 100)) {

				Texture2D TextureMap = (Texture2D)hit.transform.GetComponent<Renderer>().material.mainTexture;
				var pixelUV = hit.textureCoord;
				pixelUV.x *= TextureMap.width;
				pixelUV.y *= TextureMap.height;

				print ("x=" + pixelUV.x + ",y=" + pixelUV.y + " " + TextureMap.GetPixel ((int)pixelUV.x, (int)pixelUV.y));




			}

		}
	}
}

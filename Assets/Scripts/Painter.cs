using UnityEngine;
using System.Collections;

public class Painter : Block {
	public Color col = Color.black;
	Vector3 p1;
	// Use this for initialization
	void Start () {
		Init ();
		p1 = transform.position + transform.forward * 0.1f;
	}

	public void Paint() {

		RaycastHit hit;
		p1 = transform.position + transform.forward * 0.1f;
		if (Physics.Raycast (p1, transform.forward, out hit, 4f)) {
			Debug.DrawRay (p1, transform.forward);


			Texture2D TextureMap = (Texture2D)hit.transform.GetComponent<Renderer>().material.mainTexture;

			var pixelUV = hit.textureCoord;
			pixelUV.x *= TextureMap.width;
			pixelUV.y *= TextureMap.height;

			TextureMap.SetPixel ((int)pixelUV.x, (int)pixelUV.y,col);
			TextureMap.Apply ();

		}

	}
}

using UnityEngine;
using System.Collections;

public class ColSensor : MonoBehaviour {

	public float GetBrightness()
	{
		//Ray ray = 
		RaycastHit hit;
		if (Physics.Raycast(gameObject.transform.position,Vector3.down, out hit)) {
				Texture2D TextureMap = (Texture2D)hit.transform.GetComponent<Renderer>().material.mainTexture;
				var pixelUV = hit.textureCoord;
				pixelUV.x *= TextureMap.width;
				pixelUV.y *= TextureMap.height;

			//	print ("Sensor Br: " + TextureMap.GetPixel ((int)pixelUV.x, (int)pixelUV.y));
			Color px = TextureMap.GetPixel ((int)pixelUV.x, (int)pixelUV.y);
			double br = (px.r + px.g + px.b) * 0.333;
			return (float)br;


		}
		return 0;



	}

}

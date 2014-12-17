using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Global : MonoBehaviour {
	public static bool isPaused = false;
	public static List<GameObject> robotsList = new List<GameObject> ();
	public static GameObject self;

	void Start() {
		self = this.gameObject;
	}

	public static GameObject ReturnSelf() {
		return self;
		}

	public static void SetPause(bool p) {
		isPaused = p;
		if (p) {
						Time.timeScale = 0f;
			self.BroadcastMessage("LUA_Pause");
				} else {
			self.BroadcastMessage("LUA_Resume");
			Time.timeScale = 1f;
				}
	}

	public static bool GetPauseState() {
		return isPaused;
		}

	public static void RegisterNewRobot(GameObject robot) {
		robotsList.Add (robot);
	}

	public static void UnRegRobot(GameObject robot) {
		robotsList.Remove (robot);
	}

}

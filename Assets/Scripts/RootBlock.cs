//Компонент корневого блока робота. Несет в себе всякую важную и служебную инфу, его уничтожение уничтожает робота.
//Находится не на корневом объекте, а на основном блоке (обычно на том, где процессор, но не обязательно)

using UnityEngine;
using System.Collections;

public class RootBlock : MonoBehaviour {
	public string name = "New Robot";

	private GameObject realRoot;
	void Start () {
		realRoot = gameObject.transform.parent.gameObject;
		Global.RegisterNewRobot (this.gameObject);
		gameObject.GetComponent<Block> ().SetRoot ();
	}
	
	public void SetName(string n) {
		name = n;
		}
	//Нам также нужны функции, временно отключающие физику у всех объектов робота, чтобы его таскать. Но проблема в том, что часть блоков должна находиться всегда
	// с отключенной физикой, например датчик на поворотной установке.
	public void DisablePhysics() {
			foreach (Transform child in realRoot.transform) {
				if (child.GetComponent<Rigidbody>()) {
				child.GetComponent<Rigidbody>().isKinematic = true;
				}
			}
		}
	public void EnablePhysics() {
			foreach (Transform child in realRoot.transform) {
				if (child.GetComponent<Rigidbody>()) {
					child.GetComponent<Rigidbody>().isKinematic = false;
				}
			}
		}

	void OnDestroy() {
		Global.UnRegRobot (gameObject);
		//realRoot.BroadcastMessage ("Selfdestruct",SendMessageOptions.DontRequireReceiver);
		}
}

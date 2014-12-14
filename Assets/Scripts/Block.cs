using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	public string BlockID = "null";
	public string BlockName = "null block";
	public GameObject BlockObj;
	public BlockInfo.BlockType Type;
	
	public float BlockHealth = 100f;
	public float BlockCrashTol = 10f;
	public float BlockEnergyDrain = 0.0f;
	
	private GameObject master; // ссылка на главный объект
	private BlockManager manager;

	private Color? blockCol; // Цвет объекта (используется при подсветке при выделении)
	private bool mouseOver;
	private bool showInfo;

	private Rect infoRect;
	private Vector3 infoWindowPos; // Точка на детали, где мы кликнули по окошку

	private int infoRectH = 150;
	private int infoRectW = 250;

	private string IdToEdit;// = BlockID;


	void Start () {

		BlockObj = this.gameObject;
		IdToEdit = BlockID;
	}

	public void RegisterBlock(GameObject owner)
	{
		master = owner;
		manager = master.GetComponent<BlockManager> ();
		manager.RegisterNew (BlockID, BlockName, Type, this.gameObject);
	}

	public void ChangeID(string newID)
	{
		//manager = master.GetComponent<BlockManager> ();
		manager.RemoveBlock (this.gameObject); //ссылка на игровой объект однозначно определяет блок, больше ничего не нужно.
		BlockID = newID;
		manager.RegisterNew (BlockID, BlockName, Type, this.gameObject);
	
	}

	void OnMouseOver() {
		mouseOver = true;
		Debug.Log ("Mouse Hovering Over " + BlockID + " block");
		if (blockCol == null) {

			blockCol = (Color?) gameObject.GetComponent<Renderer> ().material.color;

				}

		Color col = Color.green;
		gameObject.GetComponent<Renderer> ().material.color = col;

		if (Input.GetMouseButtonUp(1)) {
			showInfo = !showInfo;
			if (showInfo) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if(Physics.Raycast(ray,out hit,50f))
				{
					infoWindowPos = hit.point;
					Vector3 p = Camera.main.WorldToScreenPoint(infoWindowPos);
					infoRect = new Rect(p.x,Screen.height - p.y,infoRectW,infoRectH);
				}
				//infoRect = new Rect(Input.mousePosition.x,Input.mousePosition.y
			}
		}
		

	}

	void OnMouseExit() {
		gameObject.GetComponent<Renderer> ().material.color = (Color) blockCol;
		blockCol = null;
		mouseOver = false;
	}

	void OnGUI() 
	{

			if (mouseOver) {
				GUI.Label (new Rect (Input.mousePosition.x + 20,Screen.height - Input.mousePosition.y, 150, 30), "Block ID: " + BlockID);
				if(BlockObj != null) {
					manager.SetSelectedBlock(BlockObj);
				}
			}

		if (showInfo) 
		{	
			Vector3 p = Camera.main.WorldToScreenPoint(infoWindowPos);
			infoRect = new Rect(p.x,Screen.height - p.y,infoRectW,infoRectH);
			infoRect = GUI.Window(0,infoRect,ShowInfoRect,"Block Properties: " + BlockName);
		}
	}

	void ShowInfoRect(int windowID) 
	{
		IdToEdit = GUI.TextField (new Rect (10, 30, 100, 20), IdToEdit, 16);
		if (GUI.Button (new Rect (150, 30, 40, 20), "Set")) 
		{
			ChangeID(IdToEdit);
		}
	}



}

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

	void Start () {
		BlockObj = this.gameObject;
	}

	public void RegisterBlock(GameObject owner)
	{
		owner.GetComponent<BlockManager> ().RegisterNew (BlockID, BlockName, Type, this.gameObject);
		master = owner;
	}

	public void ChangeID(string newID)
	{
	master.GetComponent<BlockManager> ().RemoveBlock (this.gameObject); //ссылка на игровой объект однозначно определяет блок, больше ничего не нужно.
	BlockID = newID;
	
	master.GetComponent<BlockManager> ().RegisterNew (BlockID, BlockName, Type, this.gameObject);
	
	}
}

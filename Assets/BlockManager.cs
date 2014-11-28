using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class BlockManager : MonoBehaviour {

	public List<BlockInfo> blockList = new List<BlockInfo> ();


	// Use this for initialization
	void Start () {
		foreach (Transform child in transform.parent) {
			child.SendMessage ("RegisterBlock", this.gameObject);
		}
	}

	public void RegisterNew(string ID, string name, BlockInfo.BlockType type,GameObject obj) {
		BlockInfo block = new BlockInfo();
		block.BlockID = ID;
		block.BlockObj = obj;
		block.BlockName = name;
		block.Type = type;
		Debug.Log (ID);
		blockList.Add (block);
	}


	public GameObject GetBlockByIDAndType(string ID, BlockInfo.BlockType type) {
		foreach(BlockInfo bl in blockList) {
			if (bl.BlockID == ID) {
				if (bl.Type == type) {
					return bl.BlockObj;
				}

			}
		}
		return null;
	}

	public GameObject GetBlockByID(string ID) {
		foreach(BlockInfo bl in blockList) {
			if (bl.BlockID == ID) {
					return bl.BlockObj;
			}
		}
		return null;
	}
	
	public bool RemoveBlock(GameObject block) {
		foreach(BlockInfo bl in blockList) {
			if (bl.BlockObj == block) {
				BlockList.Remove(bl);
				return true;
			}
		}
		return false;
	}

}

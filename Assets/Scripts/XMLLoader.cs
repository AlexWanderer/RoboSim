using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class XMLLoader : MonoBehaviour {
	public string fileName;
	public string path;

	public GameObject RoboRoot;

	public List<GameObject> blocks = new List<GameObject> ();

	public XmlDocument doc;

	void Start () {
	//	SaveToXml (this.gameObject); //Проверим, работает ли
	}

	public void SaveToXml(GameObject robot, string name) { //Сюда кидаем рутовый объект робота
		RoboRoot = robot;
		XmlTextWriter save = new XmlTextWriter(name + ".xml",System.Text.Encoding.UTF8);
		save.WriteStartDocument ();
		save.WriteStartElement ("Robot");
		save.WriteAttributeString ("Name", name);
		save.WriteAttributeString ("File", "sin.lua");
		save.WriteAttributeString ("Description", "First robot save");
		save.WriteEndElement ();

		save.Close ();

		doc = new XmlDocument();
		doc.Load (name + ".xml");
	
		Transform[] blocks = RoboRoot.GetComponentsInChildren<Transform>(); //cохраняем сами блоки


		foreach (Transform child in blocks) {
			if(child.GetComponent<Block>()) {
				SavePart(child.gameObject);

			}
		}

		FixedJoint[] joints = RoboRoot.GetComponentsInChildren<FixedJoint>(); //cохраняем соединения блоков (пока только фиксированные)
		
		
		foreach (FixedJoint constraint in joints) {
			if(constraint.gameObject.GetComponent<Block>()) {
				SaveConstraint(constraint);
				Debug.Log ("saved constraint!");
			}
		}



		doc.Save(name + ".xml");

		}

	public GameObject LoadFromXML(Vector3 pos, string name) {
		GameObject root = new GameObject();
		root.transform.position = pos;
		RoboRoot = root;
		root.name = "realRoot";
		doc = new XmlDocument ();
		doc.Load (name + ".xml");

		foreach (XmlNode node in doc.DocumentElement.ChildNodes)
		{
			if (node.Name == "Part")
			{
				Debug.Log("Part Info Detected");
				GameObject prt = LoadPart(node);
				blocks.Add(prt);
				prt.transform.parent = root.transform;
			}

			if (node.Name == "Constraint")
			{
				Debug.Log("Constraint Info Detected");
				LoadConstraint(node);
			}
		}

		return root;
		}

	GameObject LoadPart(XmlNode part) {
		int uid =int.Parse( part.Attributes.GetNamedItem("uID").Value);
		bool isRt = bool.Parse(part.Attributes.GetNamedItem("isRoot").Value);
		string uname = part.Attributes.GetNamedItem("uName").Value;
		string procID = part.Attributes.GetNamedItem("procID").Value;
		string p = part.Attributes.GetNamedItem("pos").Value;
		Vector3 pos = ParseVector3 (p);
		p = part.Attributes.GetNamedItem("rot").Value;
		Quaternion rot = ParseQuaternion (p);

		GameObject block = Instantiate ((GameObject)Resources.Load (uname));
		Block cfg = block.GetComponent<Block> ();
		cfg.UID = uid;
		cfg.BlockID = procID;
		cfg.SetRoot (isRt);
		block.transform.position = pos + RoboRoot.transform.position;
		block.transform.rotation = rot;

		block.GetComponent<Rigidbody> ().isKinematic = true;

		return block;
		}

	void LoadConstraint(XmlNode constraint) {
		int id1 = int.Parse (constraint.Attributes.GetNamedItem ("id1").Value);
		int id2 = int.Parse (constraint.Attributes.GetNamedItem ("id2").Value);
		bool col = bool.Parse (constraint.Attributes.GetNamedItem ("collision").Value);

		GameObject o1 = GetByUID (id1);
		GameObject o2 = GetByUID (id2);

		FixedJoint joint = o1.AddComponent<FixedJoint> ();
		joint.connectedBody = o2.GetComponent<Rigidbody>();
		joint.enableCollision = col;

		}


	void SavePart(GameObject part) {
		Block block = part.GetComponent<Block> ();
		XmlNode element = doc.CreateElement("Part");
		doc.DocumentElement.AppendChild(element); 
		XmlAttribute uid = doc.CreateAttribute("uID"); 
		uid.Value = block.UID.ToString();
		element.Attributes.Append(uid); 

		XmlAttribute root = doc.CreateAttribute("isRoot"); 
		root.Value = block.GetRoot ().ToString ();
		element.Attributes.Append(root); 

		XmlAttribute uName = doc.CreateAttribute("uName"); 
		uName.Value = block.uName;
		element.Attributes.Append(uName); 

		XmlAttribute id = doc.CreateAttribute("procID"); 
		id.Value = block.BlockID;
		element.Attributes.Append(id); 

		Vector3 relPos = part.transform.position -  RoboRoot.transform.position;

		XmlAttribute pos = doc.CreateAttribute("pos"); 
		string val = relPos.x.ToString () + "," + relPos.y.ToString () + "," + relPos.z.ToString ();
		pos.Value = val;
		element.Attributes.Append(pos); 

		XmlAttribute rot = doc.CreateAttribute("rot"); 
		val = part.transform.rotation.x.ToString () + "," + part.transform.rotation.y.ToString () + "," + part.transform.rotation.z.ToString () + "," + part.transform.rotation.w.ToString ();
		rot.Value = val;
		element.Attributes.Append(rot); 


	}

	void SaveConstraint(FixedJoint joint) {
		GameObject obj = joint.gameObject;
		GameObject target = joint.connectedBody.gameObject;
		int id1 = obj.GetComponent<Block> ().UID;
		int id2 = target.GetComponent<Block> ().UID;


		XmlNode element = doc.CreateElement("Constraint");
		doc.DocumentElement.AppendChild(element); 

		XmlAttribute i1 = doc.CreateAttribute("id1"); 
		i1.Value = id1.ToString ();
		element.Attributes.Append(i1); 

		XmlAttribute i2 = doc.CreateAttribute("id2"); 
		i2.Value = id2.ToString ();
		element.Attributes.Append(i2); 

		XmlAttribute colFlag = doc.CreateAttribute("collision"); 
		colFlag.Value = joint.enableCollision.ToString ();
		element.Attributes.Append(colFlag); 


	}

	public GameObject GetByUID(int id) {
		foreach (GameObject obj in blocks) {
			if(obj.GetComponent<Block>().UID==id){
				return obj;
			}
		}
		return null;
		}


	public static Vector3 ParseVector3(string str) {
		Vector3 vec = new Vector3 ();
		string[] elements = str.Split (new char[] {','});
		vec.x = float.Parse (elements [0]);
		vec.y = float.Parse (elements [1]);
		vec.z = float.Parse (elements [2]);
		return vec;
	}

	public static Quaternion ParseQuaternion(string str) {
		Quaternion qua = new Quaternion ();
		string[] elements = str.Split (new char[] {','});
		qua.x = float.Parse (elements [0]);
		qua.y = float.Parse (elements [1]);
		qua.z = float.Parse (elements [2]);
		qua.w = float.Parse (elements [3]);
		return qua;
	}

}

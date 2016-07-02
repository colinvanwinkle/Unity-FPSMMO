using UnityEngine;
using System.Collections;

public class playerTable : ScriptableObject {

	Hashtable table = new Hashtable();

	public void add(int ID, string name){
		table.Add (ID, name);

	}

	public string getPlayer(int ID){
	//	table [ID];
		return null;
	}
}

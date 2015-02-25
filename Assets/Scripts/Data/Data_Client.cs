using UnityEngine;
using System.Collections;

[System.Serializable]
public class Data_Client{

	public int projectID;
	public string projectName  = "";
	public string projectInfo  = "";

	public Data_Client(int id, string name, string info){
		projectID = id;
		projectName = name;
		projectInfo = info;
	}

}

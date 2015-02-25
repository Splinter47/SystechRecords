using UnityEngine;
using System.Collections;

[System.Serializable]
public class Data_Project{

	public int projectID;
	public string projectName  = "";
	public string projectValue  = "";
	public string projectRegion  = "";
	public string projectCountry  = "";
	public string projectSector  = "";
	public string projectContract  = "";
	public string projectPara1  = "";
	public string projectPara2  = "";
	public string projectPara3  = "";
	public string projectPara4  = "";
	public string projectPara5  = "";

	public Data_Project(int id, string name, string value, string region, string country, 
	                    string sector, string contract, string para1, string para2, 
	                    string para3, string para4, string para5){
		projectID = id;
		projectName = name;
		projectValue = value;
		projectRegion = region;
		projectCountry = country;
		projectSector = sector;
		projectContract = contract;
		projectPara1 = para1;
		projectPara2 = para2;
		projectPara3 = para3;
		projectPara4 = para4;
		projectPara5 = para5;
	}

}

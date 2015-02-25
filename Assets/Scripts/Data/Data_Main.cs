//You must include these namespaces
//to use BinaryFormatter
using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;


public class Data_Main : MonoBehaviour {

	public GameObject promptObject;
	private UILabel prompt;

	private bool saved = false;
	private bool projectSizeCalculated = false;
	private bool clientSizeCalculated = false;
	private bool peopleSizeCalculated = false;
	private bool downloadStarted = false;
	
	public int dataDownloaded = 0;
	private int downloadSize;
	private int projectTableSize;
	private int clientTableSize;
	private int peopleTableSize;

	string metaDataURL = "http://www.samdavies.info/Systech/metaData.php";
	string projectDataURL = "http://www.samdavies.info/Systech/projectData.php";
	string clientDataURL = "http://www.samdavies.info/Systech/clientData.php";
	string peopleDataURL = "http://www.samdavies.info/Systech/peopleData.php";

	// List of all project data transfering between WWW and Local
	List<Data_Project> Projects = new List<Data_Project>();
	List<Data_Client> Clients = new List<Data_Client>();
	List<Data_People> People = new List<Data_People>();

	// used to save images to binary PlayerPref
	List<string> ImageURLs = new List<string>();

	void Awake(){
		// Forces a different code path in the BinaryFormatter that doesn't rely on run-time code generation (which would break on iOS).
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
	}

	void Start(){

		/// clear PP /////////
		PlayerPrefs.DeleteAll();
		//////////////////////
		prompt = promptObject.GetComponent<UILabel>();
		prompt.text = "Initialising connection to server...";

		//!!!!!!!!!!!! must be in format: Type_******** for use in current php file
		StartCoroutine(findTableSize("Type_Projects"));
		StartCoroutine(findTableSize("Type_Clients"));
		StartCoroutine(findTableSize("Type_People"));
	}
	
	void Update(){
		// stage check
		// double peopleTableSize since images need to be downloaded too
		downloadSize = (projectTableSize + clientTableSize + (2 * peopleTableSize));
		bool sizeCalculated = projectSizeCalculated && clientSizeCalculated && peopleSizeCalculated;

		// download
		if(sizeCalculated && (!downloadStarted)){
			downloadStarted = true;
			prompt.text = "Gathering Data... 0%";
			print ("sizeCalculated");
			DownloadAllData();
		}

		// save and load
		if((dataDownloaded >= downloadSize) && (sizeCalculated)  && (!saved)){
			saved = true;
			prompt.text = "Download complete.";
			print ("saving, loading, printing");
			SaveDataLocal();
			LoadLocalData();
			foreach(Data_Project p in Projects){
				print (p.projectName);
			}
			foreach(Data_Client c in Clients){
				print (c.projectName);
			}
			foreach(Data_People p in People){
				print (p.firstName);
			}
			Application.LoadLevel(2);
		}
	}
	
	void DownloadAllData(){
		for(int i = 0; i < projectTableSize; i++){
			StartCoroutine(DownloadData(i, "Type_Projects", projectDataURL));
		}
		
		for(int i = 0; i < clientTableSize; i++){
			StartCoroutine(DownloadData(i, "Type_Clients", clientDataURL));
		}

		for(int i = 0; i < peopleTableSize; i++){
			StartCoroutine(DownloadData(i, "Type_People", peopleDataURL));
		}
	}

	IEnumerator findTableSize(string type){
		// cut off the Data_ part of string
		string URL = metaDataURL + "?type=" + type.Substring(5);
		WWW tableReader = new WWW(URL) ;
		yield return tableReader;
		
		if(tableReader.error != null){
			print("Failed");
			StartCoroutine(findTableSize(type));
		}else{
			print ("found data: " + tableReader.text);
			prompt.text = "Caluculating download size... ";

			if(type.Equals("Type_Projects")){
				projectTableSize = Convert.ToInt32(tableReader.text);
				projectSizeCalculated = true;
			}
			else if (type.Equals("Type_Clients")){
				clientTableSize = Convert.ToInt32(tableReader.text);
				clientSizeCalculated = true;
			}
			else if (type.Equals("Type_People")){
				peopleTableSize = Convert.ToInt32(tableReader.text);
				peopleSizeCalculated = true;
			}
		}
	}

	IEnumerator DownloadData(int id, string type, string dataURL){
		string URL = dataURL + "?id=" + id;
		WWW projectReader = new WWW(URL);
		yield return projectReader;
		
		if(projectReader.error != null){
			print("Failed");
			StartCoroutine(DownloadData(id, type, dataURL));
		}else{
			print ("found data: " + projectReader.text);
			dataDownloaded ++;
			int percent = (dataDownloaded*100)/downloadSize ;
			prompt.text = "Gathering Data... " + percent + "%";
			AddData(id, projectReader.text, type);
		}
	}

	void AddData(int id, string stringToCut, string type){
		// *LEGACY* split the iput string into parts and add a new project
		string[] stringSeparators = {"<!>"};
		string[] data = stringToCut.Split(stringSeparators, StringSplitOptions.None);

		//choose which list to add to
		if(type.Equals("Type_Projects")){
			Projects.Add(new Data_Project(id, data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], data[10]));
		}else
		if(type.Equals("Type_Clients")){
			Clients.Add(new Data_Client(id, data[0], data[1]));
		}
		if(type.Equals("Type_People")){
			Data_People newPerson = new Data_People(id, stringToCut);
			People.Add(newPerson);
			// download Images
			string photoName = newPerson.photoSmall;
			DownloadImage("http://www.samdavies.info/Systech/PeopleImages/" + photoName + ".png");
		}
	}

	void DownloadImage(string URL){
		// new downloader per image URL
		// remember to add the file extension
		WebImageDownload downloader = gameObject.AddComponent<WebImageDownload>();
		downloader.imageUrl = URL;
		ImageURLs.Add(URL);
		downloader.FindImage(this);
	}

	void SaveDataLocal(){
		convertTo<Data_Project>("ProjectData", Projects);
		convertTo<Data_Client>("ClientData", Clients);
		convertTo<Data_People>("PeopleData", People);
		convertTo<string>("ImageURLs", ImageURLs);
	}

	void convertTo<T>(string key, List<T> list){
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream();
		b.Serialize(m, list);
		PlayerPrefs.SetString(key, Convert.ToBase64String(m.GetBuffer()));
	}

	void LoadLocalData(){
		string projects = PlayerPrefs.GetString("ProjectData");
		string clients = PlayerPrefs.GetString("ClientData");
		string people = PlayerPrefs.GetString("PeopleData");

		convertBack(projects, "Type_Projects");
		convertBack(clients, "Type_Clients");
		convertBack(people, "Type_People");
	}

	void convertBack(string data, string type){
		if(!String.IsNullOrEmpty(data)){
			//load the data from binary back into the list
			var b = new BinaryFormatter();
			var m = new MemoryStream(Convert.FromBase64String(data));

			if(type.Equals("Type_Projects")){
				Projects = b.Deserialize(m) as List<Data_Project>;
			}else
			if(type.Equals("Type_Clients")){
				Clients = b.Deserialize(m) as List<Data_Client>;
			}else
			if(type.Equals("Type_People")){
				People = b.Deserialize(m) as List<Data_People>;
			}
		}
	}

}
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ProjDisplayMain : MonoBehaviour {

	public GameObject gridObject;
	public GameObject projectPrefab;
	public UIScrollView scrollView;
	public UIPopupList regionButton;

	private UIGrid grid;

	private List<Data_Project> allProjs = new List<Data_Project>();
	private SortedList<string, Data_Project> sortedProjs = new SortedList<string, Data_Project>();
	private List<GameObject> currentProjects = new List<GameObject>();

	void Awake(){
		// Forces a different code path in the BinaryFormatter that doesn't rely on run-time code generation (which would break on iOS).
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
	}

	// Use this for initialization
	void Start () {

		///////////////////////////////////////////
		// get all the string data from Player Pref
		string projPref = PlayerPrefs.GetString("ProjectData");
		if(!String.IsNullOrEmpty(projPref)){
			//load the data from binary back into the list
			var b = new BinaryFormatter();
			var m = new MemoryStream(Convert.FromBase64String(projPref));

			allProjs = b.Deserialize(m) as List<Data_Project>;
		}

		// get all the Photo data from Player Pref
		var textureCache = WebTextureCache.InstantiateGlobal();
		textureCache.LoadCacheFromPrefs("ImageURLs");
		///////////////////////////////////////////

		addAllProj();

		// find the grid
		grid = gridObject.GetComponent<UIGrid>();

		// create new profiles
		foreach (KeyValuePair<string, Data_Project> kvp in sortedProjs){
			Data_Project profile = kvp.Value;
			// create a new Profile Thumbnail
			ProjDisplayThumnail thubmNail = new ProjDisplayThumnail(profile, gridObject, projectPrefab);
			currentProjects.Add(thubmNail.create());
		}
		grid.Reposition();
	}

	public void addAllProj(){
		//first clear all Data_Project
		sortedProjs.Clear();
		//add all Data_Project in allProjs to sortedProjs
		foreach(var proj in allProjs){
			string key = proj.projectName;
			sortedProjs.Add(key, proj);
		}
	}
	
	public void updateProfiles(){
		//recentre before
		scrollView.ResetPosition();
		// delete old profiles
		foreach (GameObject prof in currentProjects){
			Destroy(prof);
		}
		//recentre after
		scrollView.ResetPosition();
		currentProjects.Clear();
		
		// create new profiles
		foreach (KeyValuePair<string, Data_Project> kvp in sortedProjs){
			Data_Project profile = kvp.Value;
			// create a new Profile Thumbnail
			ProjDisplayThumnail thubmNail = new ProjDisplayThumnail(profile, gridObject, projectPrefab);
			currentProjects.Add(thubmNail.create());
		}
		grid.Reposition();

		//recentre after
		scrollView.ResetPosition();
	}

	/*public void resetToAll(){
		// set the "current" label to the current selection
		GameObject currentRegion = GameObject.Find("currentFilterText/currentRegion");
		currentRegion.GetComponent<UILabel>().text = "All";
	}*/
	
	//private bool firstTimeSkip = false;

	/*public void buttonFilter(){
		if(firstTimeSkip){
			addAllProj();
			string region = regionButton.value;
			// set the "current" label to the current selection
			GameObject currentRegion = GameObject.Find("currentFilterText/currentRegion");
			currentRegion.GetComponent<UILabel>().text = regionButton.value;

			if(region.Equals("UK")){ filterRegion(Data_Project.Region.UK); }
			else if(region.Equals("Middle East and Africa")){ filterRegion(Data_Project.Region.MiddleEastAndAfrica); }
			else if(region.Equals("Europe")){ filterRegion(Data_Project.Region.Europe); }
			else if(region.Equals("Canada")){ filterRegion(Data_Project.Region.Canada); }
			else if(region.Equals("Asian Pacific")){ filterRegion(Data_Project.Region.AsianPacific); }
			else if(region.Equals("Americas")){ filterRegion(Data_Project.Region.Americas); }

			updateProfiles();
		}else{
			firstTimeSkip = true;
		}

	}*/

	/*Data_Project.Region[] regionify(string input){;
		string[] stringSeparators = {","};
		string[] regionStrings = input.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
		Data_Project.Region[] regions = new Data_Project.Region[regionStrings.Length];
		
		for(int i = 0; i < regionStrings.Length; i++){
			string current = regionStrings[i].ToLower();
			
			if(current.Equals("uk")){ regions[i] = Data_Project.Region.UK; ;}
			else if(current.Equals("middleeastandafrica")){ regions[i] = Data_Project.Region.MiddleEastAndAfrica; }
			else if(current.Equals("europe")){ regions[i] = Data_Project.Region.Europe; }
			else if(current.Equals("canada")){ regions[i] = Data_Project.Region.Canada; }
			else if(current.Equals("asianpacific")){ regions[i] = Data_Project.Region.AsianPacific; }
			else if(current.Equals("americas")){ regions[i] = Data_Project.Region.Americas; }
			else{ print ("error: region not found");}
		}
		
		return regions;
	}*/

	/*public void filterRegion(Data_Project.Region filter){

		// record which keys to remove
		List<string> keysToRemove = new List<string>();

		// look up all pairs
		foreach (KeyValuePair<string, Data_Project> kvp in sortedProjs){
			string key = kvp.Key;
			Data_Project person = kvp.Value;

			// check multiple regions
			bool contains = false;
			foreach(Data_Project.Region r in person.regions){
				if(r == filter){
					contains = true;
				}
			}
			if(!contains){
				keysToRemove.Add(key);
			}
		}

		//remove keys
		foreach (string key in keysToRemove){
			sortedProjs.Remove(key);
		}
	}*/

	/*public void filterSector(Data_Project.Sector filter){

		// record which keys to remove
		List<string> keysToRemove = new List<string>();

		// look up all pairs
		foreach (KeyValuePair<string, Data_Project> kvp in sortedProjs){
			string key = kvp.Key;
			Data_Project person = kvp.Value;

			// check multiple regions
			bool contains = false;
			foreach(Data_Project.Sector r in person.sectors){
				if(r == filter){
					contains = true;
				}
			}
			if(!contains){
				keysToRemove.Add(key);
			}
		}

		//remove keys
		foreach (string key in keysToRemove){
			sortedProjs.Remove(key);
		}
	}*/

	/*public void filterService(Data_Project.Service filter){
		
		// record which keys to remove
		List<string> keysToRemove = new List<string>();
		
		// look up all pairs
		foreach (KeyValuePair<string, Data_Project> kvp in sortedProjs){
			string key = kvp.Key;
			Data_Project person = kvp.Value;
			
			// check multiple regions
			bool contains = false;
			foreach(Data_Project.Service r in person.services){
				if(r == filter){
					contains = true;
				}
			}
			if(!contains){
				keysToRemove.Add(key);
			}
		}
		
		//remove keys
		foreach (string key in keysToRemove){
			sortedProjs.Remove(key);
		}
	}*/

}

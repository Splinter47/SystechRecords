using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DisplayMain : MonoBehaviour {

	public GameObject gridObject;
	public GameObject profilePrefab;
	public UIScrollView scrollView;
	public UIPopupList regionButton;

	private GameObject profilePagePointer;
	private UIGrid grid;

	private List<Data_People> allFolk = new List<Data_People>();
	private SortedList<string, Data_People> sortedFolk = new SortedList<string, Data_People>();
	private List<GameObject> currentProfiles = new List<GameObject>();

	void Awake(){
		// Forces a different code path in the BinaryFormatter that doesn't rely on run-time code generation (which would break on iOS).
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
	}

	// Use this for initialization
	void Start () {

		// get all the string data from Player Pref
		string peoplePref = PlayerPrefs.GetString("PeopleData");
		if(!String.IsNullOrEmpty(peoplePref)){
			//load the data from binary back into the list
			var b = new BinaryFormatter();
			var m = new MemoryStream(Convert.FromBase64String(peoplePref));

			allFolk = b.Deserialize(m) as List<Data_People>;
		}

		// get all the Photo data from Player Pref
		var textureCache = WebTextureCache.InstantiateGlobal();
		textureCache.LoadCacheFromPrefs("ImageURLs");

		addAllFolk();
		profilePagePointer = GameObject.Find("ProfilePage");
		// find the gridObject
		grid = gridObject.GetComponent<UIGrid>();

		// create all profiles
		createGrid();
	}

	public void addAllFolk(){
		//first clear all Data_People
		sortedFolk.Clear();
		//add all Data_People in allFolk to sortedFolk
		foreach(var person in allFolk){
			string key = person.surname + " " + person.firstName;
			sortedFolk.Add(key, person);
		}
	}
	
	public void updateProfiles(){
		//recentre before
		scrollView.ResetPosition();
		// delete old profiles
		foreach (GameObject prof in currentProfiles){
			DestroyImmediate(prof);
		}
		//empty the currentProfiles
		currentProfiles.Clear();
		createGrid();
	}

	void createGrid(){
		// create new profiles
		scrollView.ResetPosition();
		foreach (KeyValuePair<string, Data_People> kvp in sortedFolk){
			Data_People profile = kvp.Value;
			// create a new Profile Thumbnail
			GameObject profileObject = NGUITools.AddChild(gridObject, profilePrefab);
			DisplayThumnail thubmNail = profileObject.AddComponent("DisplayThumnail") as DisplayThumnail;
			thubmNail.Constructor(profile, profilePagePointer);
			currentProfiles.Add(thubmNail.create());
		}
		grid.Reposition();
		scrollView.ResetPosition();
	}

	public void resetToAll(){
		// set the "current" label to the current selection
		GameObject currentRegion = GameObject.Find("currentFilterText/currentRegion");
		currentRegion.GetComponent<UILabel>().text = "All";
	}
	
	private bool firstTimeSkip = false;

	public void buttonFilter(){
		if(firstTimeSkip){
			addAllFolk();
			string region = regionButton.value;
			// set the "current" label to the current selection
			GameObject currentRegion = GameObject.Find("currentFilterText/currentRegion");
			currentRegion.GetComponent<UILabel>().text = regionButton.value;

			if(region.Equals("UK")){ filterRegion(Data_People.Region.UK); }
			else if(region.Equals("Middle East and Africa")){ filterRegion(Data_People.Region.MiddleEastAndAfrica); }
			else if(region.Equals("Europe")){ filterRegion(Data_People.Region.Europe); }
			else if(region.Equals("Canada")){ filterRegion(Data_People.Region.Canada); }
			else if(region.Equals("Asian Pacific")){ filterRegion(Data_People.Region.AsianPacific); }
			else if(region.Equals("Americas")){ filterRegion(Data_People.Region.Americas); }

			updateProfiles();
		}else{
			firstTimeSkip = true;
		}

	}
	
	Data_People.Region[] regionify(string input){;
		string[] stringSeparators = {","};
		string[] regionStrings = input.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
		Data_People.Region[] regions = new Data_People.Region[regionStrings.Length];
		
		for(int i = 0; i < regionStrings.Length; i++){
			string current = regionStrings[i].ToLower();
			
			if(current.Equals("uk")){ regions[i] = Data_People.Region.UK; ;}
			else if(current.Equals("middleeastandafrica")){ regions[i] = Data_People.Region.MiddleEastAndAfrica; }
			else if(current.Equals("europe")){ regions[i] = Data_People.Region.Europe; }
			else if(current.Equals("canada")){ regions[i] = Data_People.Region.Canada; }
			else if(current.Equals("asianpacific")){ regions[i] = Data_People.Region.AsianPacific; }
			else if(current.Equals("americas")){ regions[i] = Data_People.Region.Americas; }
			else{ print ("error: region not found");}
		}
		
		return regions;
	}

	public void filterRegion(Data_People.Region filter){

		// record which keys to remove
		List<string> keysToRemove = new List<string>();

		// look up all pairs
		foreach (KeyValuePair<string, Data_People> kvp in sortedFolk){
			string key = kvp.Key;
			Data_People person = kvp.Value;

			// check multiple regions
			bool contains = false;
			foreach(Data_People.Region r in person.regions){
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
			sortedFolk.Remove(key);
		}
	}

	public void filterSector(Data_People.Sector filter){

		// record which keys to remove
		List<string> keysToRemove = new List<string>();

		// look up all pairs
		foreach (KeyValuePair<string, Data_People> kvp in sortedFolk){
			string key = kvp.Key;
			Data_People person = kvp.Value;

			// check multiple regions
			bool contains = false;
			foreach(Data_People.Sector r in person.sectors){
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
			sortedFolk.Remove(key);
		}
	}

	public void filterService(Data_People.Service filter){
		
		// record which keys to remove
		List<string> keysToRemove = new List<string>();
		
		// look up all pairs
		foreach (KeyValuePair<string, Data_People> kvp in sortedFolk){
			string key = kvp.Key;
			Data_People person = kvp.Value;
			
			// check multiple regions
			bool contains = false;
			foreach(Data_People.Service r in person.services){
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
			sortedFolk.Remove(key);
		}
	}

}

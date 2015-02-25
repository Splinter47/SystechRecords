using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Data_People{

	public int id;
	public string firstName;
	public string surname;

	public string jobTitle;
	public string qualAcc;
	public string qualProf;

	public string jobDescrShort;
	public string jobDescrLong;
	public string expShort;

	public string expLong;
	public string office;
	public string dateCreated;

	public string dateModified;
	public string photoSmall;
	public string photoLarge;

	public List<Region> regions = new List<Region>();
	public List<Sector> sectors = new List<Sector>();
	public List<Service> services = new List<Service>();

	public Data_People(int newId, string stringToCut){

		string[] stringSeparators = {"<!>"};
		string[] data = stringToCut.Split(stringSeparators, StringSplitOptions.None);

		id = newId;
		firstName = data[0];
		surname = data[1];

		jobTitle = data[2];
		qualAcc = data[3];
		qualProf = data[4];
		
		jobDescrShort = data[5];
		jobDescrLong = data[6];
		expShort = data[7];
		
		expLong = data[8];
		office = data[9];
		dateCreated = data[10];
		
		dateModified = data[11];
		photoSmall = data[12];
		photoLarge = data[13];

		Region[] regionArray = regionify(data[14]);
		foreach(Region region in regionArray){
			regions.Add(region);
		}
		// fake sectors
		Data_People.Sector[] newSectors = {Data_People.Sector.Americas};
		foreach(Sector sector in newSectors){
			sectors.Add(sector);
		}
		// fake services
		Data_People.Service[] newServices = {Data_People.Service.Americas};
		foreach(Service service in newServices){
			services.Add(service);
		}
	}

	public enum Region{
		UK,
		Europe,
		MiddleEastAndAfrica,
		AsianPacific,
		Americas,
		Canada
	};
	
	public enum Sector{
		Europe,
		MiddleEastAndAfrica,
		AsianPacific,
		Americas
	};
	
	public enum Service{
		Europe,
		MiddleEastAndAfrica,
		AsianPacific,
		Americas
	};

	public static Region[] regionify(string input){;
		string[] stringSeparators = {","};
		string[] regionStrings = input.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
		Region[] regions = new Region[regionStrings.Length];
		
		for(int i = 0; i < regionStrings.Length; i++){
			string current = regionStrings[i].ToLower();
			
			if(current.Equals("uk")){ regions[i] = Region.UK; }
			else if(current.Equals("middleeastandafrica")){ regions[i] = Region.MiddleEastAndAfrica; }
			else if(current.Equals("europe")){ regions[i] = Region.Europe; }
			else if(current.Equals("canada")){ regions[i] = Region.Canada; }
			else if(current.Equals("asianpacific")){ regions[i] = Region.AsianPacific; }
			else if(current.Equals("americas")){ regions[i] = Region.Americas; }
			else{ Debug.Log("error: region not found");}
		}
		if(regions.Length<1){
			Debug.Log("error: no region found");
			Region[] regionBackup = {Region.UK};
			return regionBackup;
		}
		
		return regions;
	}
}

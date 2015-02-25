using UnityEngine;
using System.Collections;

public class DisplayThumnail: MonoBehaviour{

	private Data_People person;
	private GameObject profilePage;

	public void Constructor(Data_People profile, GameObject profilePageRef){
		profilePage = profilePageRef;
		person = profile;
	}

	public GameObject create(){


		setLabel(gameObject, "0_Name", person.firstName + " " + person.surname);
		setLabel(gameObject, "5_office", person.office);
		setLabel(gameObject, "6_ProfQual", person.qualProf);
		setLabel(gameObject, "7_AccQual", person.qualAcc);
		setLabel(gameObject, "8_job2", person.jobTitle);

		//add region text
		GameObject regionObject = gameObject.transform.FindChild("4_Regions").gameObject;
		UILabel region = regionObject.GetComponent<UILabel>();
		string regionText = regionToString(person.regions[0]);
		if(person.regions.Count>1){
			for(int i = 1; i<person.regions.Count; i++){
				regionText += ", " + regionToString(person.regions[i]);
			}
		}
		region.text = regionText;
		region.MarkAsChanged();

		// add photo
		GameObject photoObject = gameObject.transform.FindChild("2_Photo").gameObject;
		LoadImage(photoObject, person.photoSmall);

		///////////////////////add the page script//////////////////////////////////
		UIPlayTween playTweenComponent = gameObject.GetComponent<UIPlayTween>();
		playTweenComponent.tweenTarget = profilePage;
		////////////////////////////////////////////////////////////////////////////

		return gameObject;

	}

	public void setLabel(GameObject parent, string child, string text){
		GameObject nameObject = parent.transform.FindChild(child).gameObject;
		UILabel name = nameObject.GetComponent<UILabel>();
		name.text = text;
		name.MarkAsChanged();
	}

	void LoadImage(GameObject photoObject, string fileName){
		// we only want one image displayer
		if(photoObject.GetComponent<WebImageDisplayCached>() == null){
			photoObject.AddComponent<WebImageDisplayCached> ();
		}
		WebImageDisplayCached displayer = photoObject.GetComponent<WebImageDisplayCached>();
		displayer.imageUrl = "http://www.samdavies.info/Systech/PeopleImages/" + fileName + ".png";
		displayer.FindImage();
	}

	protected virtual void OnClick(){
		DisplayFullProfile pageContent = new DisplayFullProfile(person, profilePage);
		pageContent.create();
	}

	string regionToString(Data_People.Region r){
		string regionString = "";
		if(r == Data_People.Region.UK){regionString = "UK";}
		else if(r == Data_People.Region.MiddleEastAndAfrica){regionString = "Middle East and Africa";}
		else if(r == Data_People.Region.Europe){regionString = "Europe";}
		else if(r == Data_People.Region.Canada){regionString = "Canada";}
		else if(r == Data_People.Region.AsianPacific){regionString = "Asian Pacific";}
		else if(r == Data_People.Region.Americas){regionString = "Americas";}
		return regionString;
	}
}

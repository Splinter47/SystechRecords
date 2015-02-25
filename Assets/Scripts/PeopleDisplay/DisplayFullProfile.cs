using UnityEngine;
using System.Collections;

public class DisplayFullProfile: MonoBehaviour{
	
	private GameObject parentPanel;
	private Data_People person;

	public DisplayFullProfile(Data_People profile, GameObject parent){

		parentPanel = parent;
		person = profile;
	}

	public void create(){
		print ("page created");
		
		setLabel(parentPanel, 0, person.firstName + " " + person.surname);

		//setLabel(parentPanel, 3, person.jobDescrLong);
		GameObject scrollObject = parentPanel.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject;
		UIScrollView scrollView = scrollObject.GetComponent<UIScrollView>();
		GameObject labelObject = scrollObject.transform.GetChild(0).gameObject;
		UILabel job = labelObject.GetComponent<UILabel>();
		job.text = person.jobDescrLong;
		job.MarkAsChanged();
		scrollView.ResetPosition();

		setLabel(parentPanel, 5, person.office);
		setLabel(parentPanel, 6, person.qualProf);
		setLabel(parentPanel, 7, person.qualAcc);
		setLabel(parentPanel, 8, person.jobTitle);
		
		//add region text
		GameObject regionObject = parentPanel.transform.GetChild(4).gameObject;
		UILabel region = regionObject.GetComponent<UILabel>();
		string regionText = regionToString(person.regions[0]);
		if(person.regions.Count>1){
			for(int i = 1; i<person.regions.Count; i++){
				regionText += ", " + regionToString(person.regions[i]);
			}
		}
		region.text = regionText;
		region.MarkAsChanged();
		
		
		GameObject photoObject = parentPanel.transform.GetChild(2).gameObject;
		LoadImage(photoObject, person.photoSmall);

		/*setLabel(parentPanel, 0, person.firstName + " " + person.surname);
		//setLabel(parentPanel, 1, person.jobTitle);
		setLabel(parentPanel, 5, person.office);
		setLabel(parentPanel, 6, person.qualProf);
		setLabel(parentPanel, 7, person.qualAcc);
		setLabel(parentPanel, 8, person.jobTitle);

		//add region text
		GameObject regionObject = profileObject.transform.GetChild(4).gameObject;
		UILabel region = regionObject.GetComponent<UILabel>();
		string regionText = regionToString(person.regions[0]);
		if(person.regions.Count>1){
			for(int i = 1; i<person.regions.Count; i++){
				regionText += ", " + regionToString(person.regions[i]);
			}
		}
		region.text = regionText;


		GameObject photoObject = profileObject.transform.GetChild(2).gameObject;
		UISprite photo = photoObject.GetComponent<UISprite>();
		photo.spriteName = person.photoSmall;

		//create the button script
		GameObject buttonObject = profileObject.transform.GetChild(10).gameObject;
		UIPageButton button = buttonObject.GetComponent<UIPageButton>();
		button.profile = person;

		return profileObject;*/

	}

	void LoadImage(GameObject photoObject, string fileName){
		if(photoObject.GetComponent<WebImageDisplayCached>() == null){
			photoObject.AddComponent<WebImageDisplayCached> ();
		}
		WebImageDisplayCached displayer = photoObject.GetComponent<WebImageDisplayCached> ();
		displayer.imageUrl = "http://www.samdavies.info/Systech/PeopleImages/" + fileName + ".png";
		displayer.FindImage();
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
	
	public void setLabel(GameObject parent, int index, string text){
		GameObject nameObject = parent.transform.GetChild(index).gameObject;
		UILabel name = nameObject.GetComponent<UILabel>();
		name.text = text;
		name.MarkAsChanged();
	}
}

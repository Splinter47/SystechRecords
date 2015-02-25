using UnityEngine;
using System.Collections;

public class ProjDisplayThumnail{
	
	private GameObject parentPanel;
	private GameObject projectPrefab;
	private Data_Project project;

	public ProjDisplayThumnail(Data_Project profile, GameObject parent, GameObject prefab){

		parentPanel = parent;
		projectPrefab = prefab;
		project = profile;
	}

	public GameObject create(){
		// create a new Profile Thumbnail
		GameObject projectObject = NGUITools.AddChild(parentPanel, projectPrefab);
		setLabel(projectObject, "name", project.projectName);

		/*setLabel(projectObject, 0, project.firstName + " " + project.surname);
		//setLabel(projectObject, 1, project.jobTitle);
		setLabel(projectObject, 5, project.office);
		setLabel(projectObject, 6, project.qualProf);
		setLabel(projectObject, 7, project.qualAcc);
		setLabel(projectObject, 8, project.jobTitle);

		//add region text
		GameObject regionObject = projectObject.transform.GetChild(4).gameObject;
		UILabel region = regionObject.GetComponent<UILabel>();
		string regionText = regionToString(project.regions[0]);
		if(project.regions.Count>1){
			for(int i = 1; i<project.regions.Count; i++){
				regionText += ", " + regionToString(project.regions[i]);
			}
		}
		region.text = regionText;
		region.MarkAsChanged();

		// load image
		GameObject photoObject = projectObject.transform.GetChild(2).gameObject;
		LoadImage(photoObject, project.photoSmall);

		//create the button script
		UIPageButton buttonComponent = projectObject.GetComponent<UIPageButton>();
		buttonComponent.profile = project;
		UIPlayTween playComponent = projectObject.GetComponent<UIPlayTween>();
		playComponent.tweenTarget = GameObject.Find("ProfilePage");*/

		return projectObject;

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
	

	/*string regionToString(Data_Project.Region r){
		string regionString = "";
		if(r == Data_Project.Region.UK){regionString = "UK";}
		else if(r == Data_Project.Region.MiddleEastAndAfrica){regionString = "Middle East and Africa";}
		else if(r == Data_Project.Region.Europe){regionString = "Europe";}
		else if(r == Data_Project.Region.Canada){regionString = "Canada";}
		else if(r == Data_Project.Region.AsianPacific){regionString = "Asian Pacific";}
		else if(r == Data_Project.Region.Americas){regionString = "Americas";}
		return regionString;
	}*/
}

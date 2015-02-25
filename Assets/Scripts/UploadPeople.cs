using UnityEngine;
using System.Collections;

public class UploadPeople : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/////// ALL PROFILE DATA //////////
		/*TextFileLoader txtLoader = new TextFileLoader();
		
		for(int i = 0; i < 39; i++){
			
			string[] data = txtLoader.Load("" + i);
			
			StartCoroutine(Upload(i, data[0], data[1], data[2], data[3], data[4], 
			                         "TBA", data[5], "TBA", "TBA", data[6], "TBA", 
			                         "TBA", data[7], "TBA", data[8], 
			                         "TBA", "TBA"));
		}*/

	}
	
	IEnumerator Upload(int data1, string data2, string data3, string data4, string data5, string data6, string data7, string data8, string data9,
	                   string data10, string data11, string data12, string data13, string data14, string data15, string data16, string data17,
	                   string data18){

		string URL = "http://www.samdavies.info/Systech/peopleUpload.php";
		WWWForm fields = new WWWForm();
		fields.AddField("id", data1);
		fields.AddField("firstName", data2);
		fields.AddField("surname", data3);
		fields.AddField("jobTitle", data4);
		fields.AddField("qualAcc", data5);
		fields.AddField("qualProf", data6);
		fields.AddField("jobDescrShort", data7);
		fields.AddField("jobDescrLong", data8);
		fields.AddField("expShort", data9);
		fields.AddField("expLong", data10);
		fields.AddField("office", data11);
		fields.AddField("dateCreated", data12);
		fields.AddField("dateModified", data13);
		fields.AddField("photoSmall", data14);
		fields.AddField("photoLarge", data15);

		fields.AddField("regions", data16);
		fields.AddField("sectors", data17);
		fields.AddField("services", data18);

		WWW upload = new WWW(URL, fields) ;
		yield return upload;
		
		if(upload.error != null){
			print("Failed");
		}else{
			print("Upload Query: " + upload.text);
		}
	}
}

using UnityEngine;
using System.Collections;
using System;

public class ImageSaver : MonoBehaviour {
	
	
	/*public Texture2D test;
	
	void Start () {
		SaveTexture(test,"bar");
		test=RetriveTexture("bar"); 
	}*/
	
	public void SaveTexture(Texture2D texToSave ,string saveAs){

		Texture2D tex = texToSave;
		byte[] byteArray;

		byteArray=tex.EncodeToPNG();
		
		string temp=Convert.ToBase64String(byteArray);
		
		PlayerPrefs.SetString(saveAs,temp);      /// save it to file if u want.
		PlayerPrefs.SetInt(saveAs+"_w",tex.width);
		PlayerPrefs.SetInt(saveAs+"_h",tex.height);
	}
	
	public Texture2D RetriveTexture(string savedImageName){

		string temp=PlayerPrefs.GetString(savedImageName);
		
		int width=PlayerPrefs.GetInt(savedImageName+"_w");
		int height=PlayerPrefs.GetInt(savedImageName+"_h");
		
		byte[] byteArray= Convert.FromBase64String(temp);
		
		Texture2D tex = new Texture2D(width,height);
		
		tex.LoadImage(byteArray);
		return tex;
	}
}

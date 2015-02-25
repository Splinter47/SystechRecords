using UnityEngine;
using System.Collections;

public class WebImageDownload : MonoBehaviour{
	
	public string imageUrl = "";
	public Data_Main mainPointer;
	ImageSaver saver = new ImageSaver();

	public void FindImage(Data_Main main){
		//create a pointer so we can incriment download counter
		mainPointer = main;

		var textureCache = WebTextureCache.InstantiateGlobal();
		StartCoroutine (textureCache.GetTexture (imageUrl, LoadImage));
	}
	
	public void LoadImage(Texture2D texture){
		mainPointer.dataDownloaded +=1;
		saver.SaveTexture(texture , imageUrl);
	}
}
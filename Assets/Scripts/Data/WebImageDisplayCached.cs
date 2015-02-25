using UnityEngine;
using System.Collections;

public class WebImageDisplayCached : MonoBehaviour{

	public UITexture sprite;
	public string imageUrl = "";

	public void FindImage(){
		if (sprite == null) {
			sprite = GetComponent<UITexture>();
			if (sprite == null) {
				return;
			}
		}
		var textureCache = WebTextureCache.InstantiateGlobal();
		StartCoroutine (textureCache.GetTexture (imageUrl, LoadImage));
	}

	
	public void LoadImage(Texture2D texture){
		print("image found");
		if (texture != null) {
			Shader shader = Shader.Find ("Unlit/Transparent Colored");
			if (shader != null) {
				sprite.shader = shader;
				sprite.mainTexture = texture;
			}
		}
	}
}
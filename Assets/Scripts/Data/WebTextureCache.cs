using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class WebTextureCache : MonoBehaviour
{
	private Dictionary<string, Texture2D> imageCache = new Dictionary<string, Texture2D> ();
	private Dictionary<string, WWW> requestCache = new Dictionary<string, WWW> ();
	private static WebTextureCache instance = null;
	
	/// <summary>
	/// Instantiates a global instance of this object in the scene
	/// </summary>
	/// <param name='name'>What to name the new global object </param>
	public static WebTextureCache InstantiateGlobal (string name = "WebTextureCache")
	{
		// A perminent gameobject which isn't destroyed between scenes
		if (instance == null) {
			var gameobject = new GameObject (name);
			DontDestroyOnLoad(gameobject);
			gameobject.AddComponent<WebTextureCache> ();
			instance = gameobject.GetComponent<WebTextureCache> ();
		}
		
		return instance;
	}
	
	public IEnumerator GetTexture (string url, Action<Texture2D> callback)
	{
		if (!this.imageCache.ContainsKey (url)) {
			int retryTimes = 10; // Number of time to retry if we get a web error
			WWW request;
			do {
				--retryTimes;
				if (!this.requestCache.ContainsKey (url)) {
					// Create a new web request and cache is so any additional
					// calls with the same url share the same request.
					this.requestCache [url] = new WWW (url);
				}
				
				request = this.requestCache [url];
				yield return request;
				
				// Remove this request from the cache if it is the first to finish
				if (this.requestCache.ContainsKey (url)&& this.requestCache [url] == request) {
					this.requestCache.Remove (url);
				}
			} while(request.error != null && retryTimes >= 0);
			
			// If there are no errors add this is the first to finish,
			// then add the texture to the texture cache.
			if (request.error == null && !this.imageCache.ContainsKey (url)) {
				this.imageCache [url] = request.texture;
			}
		}
		
		if (callback != null) {
			// By the time we get here there is either a valid image in the cache
			// or we were not able to get the requested image.
			Texture2D texture = null;
			this.imageCache.TryGetValue(url, out texture);
			callback (texture);
		}
	}

	public void LoadCacheFromPrefs(string playerPrefKeys){
		string keys = PlayerPrefs.GetString(playerPrefKeys);
		List<string> images = new List<string>();

		if(!String.IsNullOrEmpty(keys)){
			//load the data from binary back into the list
			var b = new BinaryFormatter();
			var m = new MemoryStream(Convert.FromBase64String(keys));
			images = b.Deserialize(m) as List<string>;
		}

		Dictionary<string, Texture2D> tempImageCache = new Dictionary<string, Texture2D> ();
		ImageSaver saver = new ImageSaver();

		foreach(string name in images){
			Texture2D image = saver.RetriveTexture(name);
			tempImageCache.Add(name, image);
		}
		
		//replace image cache
		imageCache = tempImageCache;

	}
}

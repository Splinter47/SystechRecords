using UnityEngine;
using System.Collections;

public class loginWindow : MonoBehaviour {

	public GameObject promptObject;
	private UILabel prompt;

	string loginURL = "http://www.doogatti.com/login.php";

	private string userName = "";
	private string password = "";

	/*void OnGUI(){
		GUI.skin.window.fontSize = GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = Screen.width/60;

		GUI.Window(0, new Rect(Screen.width/4, Screen.height/4, (Screen.width/2)-40, (Screen.height/2)-100),
		           LoginWindow, "Systech Login System");
	}

	void LoginWindow(int windowID){
		GUI.Label(new Rect(Screen.width/7, Screen.height/19, Screen.width/8, Screen.height/8), "-----Username-----");
		userName = GUI.TextField(new Rect(Screen.width/50, Screen.height/12, Screen.width/3, Screen.height/26), userName);
		GUI.Label(new Rect(Screen.width/7, Screen.height/8, Screen.width/8, Screen.height/8), "-----Password-----");
		password = GUI.TextField(new Rect(Screen.width/50, Screen.height/6, Screen.width/3, Screen.height/26), password);


		if(GUI.Button(new Rect(Screen.width/8, Screen.height/4-10, Screen.width/6, Screen.height/15), "Login")){
			StartCoroutine(handleLogin(userName, password));
		}

		GUI.Label(new Rect(Screen.width/19, Screen.height/3-10, Screen.width/4, Screen.height/8), prompt);
	}*/

	void Start(){
		prompt = promptObject.GetComponent<UILabel>();
		InvokeRepeating("checkConnection", 0, 20.0F);
	}

	public void UsernameUpdate()
	{
		userName = UIInput.current.value;
	}

	public void PasswordUpdate()
	{
		password = UIInput.current.value;
	}

	public void checkConnection(){
		StartCoroutine(testConnection());
	}

	public void tryLogin(){
		StartCoroutine(handleLogin(userName, password));
	}

	IEnumerator handleLogin(string user, string pass){
		prompt.text = "Checking username and password";
		string URL = loginURL + "?username=" + user + "&password=" + pass;
		WWW loginReader = new WWW(URL);
		yield return loginReader;

		if(loginReader.error != null){
			prompt.text = "Could not locate page";
			attemptOfflineLogin(user, pass);
		}else{
			if(loginReader.text == "right"){
				saveLogin(user, pass);
				prompt.text = "Log in successful.";
				Application.LoadLevel(1);
			}else{
				removePossibleLogins(user, pass);
				prompt.text = "invalid username/password";
			}
		}
	}

	IEnumerator testConnection(){
		prompt.text = "Checking connection to Systech server...";
		// send a wrong login just to test the connection
		string URL = loginURL + "?username=" + "empty" + "&password=" + "empty";
		WWW loginReader = new WWW(URL);
		yield return loginReader;
		
		if(loginReader.error != null){
			prompt.text = "Failed to connect to server";
		}else{
			if(loginReader.text == "right"){
				prompt.text = "Log in successful.";
				Application.LoadLevel(1);
			}else{
				prompt.text = "Connected to server";
			}
		}
	}

	private void saveLogin(string user, string pass){
		PlayerPrefs.SetInt(user, 3);
		PlayerPrefs.SetString(user + "Password", pass);
	}

	private void removePossibleLogins(string user, string pass){
		PlayerPrefs.SetInt(user, 0);
	}

	private void attemptOfflineLogin(string user, string pass){
		int remainingLogins = PlayerPrefs.GetInt(user);
		string userPassword = PlayerPrefs.GetString(user + "Password");

		prompt.text += "... attempting offline login";
		print (user + " has " + remainingLogins + " logins remaining.");
		if(remainingLogins > 0){
			if(userPassword.Equals(pass)){
				prompt.text = "Offline login sucessful: " + (remainingLogins-1) + " remaining";
				print ("Offline login sucessful: " + (remainingLogins-1) + " remaining");
				PlayerPrefs.SetInt(user, (remainingLogins-1));
				Application.LoadLevel(1);
			}else{
				prompt.text = "invalid username/password (offline)";
			}
		}else{
			prompt.text = "invalid username/password (offline)";
		}

	}

}

using UnityEngine;
using System.Collections;

public class autoGridSort : MonoBehaviour {
	
	private UIGrid grid; 
	
	void Start () {
		grid = gameObject.GetComponent<UIGrid>();
		grid.Reposition();
	}
}

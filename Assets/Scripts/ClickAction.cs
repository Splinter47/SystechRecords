using UnityEngine;
using System.Collections.Generic;
using System;

public class ClickAction : MonoBehaviour {

	public GameObject parentPage;
	public Data_People profile;
	public Func<int> methodToExecute;

	public ClickAction(Func<int> methodPointer){
		methodToExecute = methodPointer;
	}

	protected virtual void OnClick(){
		methodToExecute();
	}

}

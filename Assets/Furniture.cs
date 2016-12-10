using UnityEngine;
using System.Collections;

public class Furniture : MonoBehaviour {
	public string name;
	public string verb;
	public string fnVerb;

	void MouseOver() { 
		GameController.GetInstance().SetCurrentTarget(this);
	}
	
	void MouseOut() { 
		GameController.GetInstance().SetCurrentTarget(null);
	}
}

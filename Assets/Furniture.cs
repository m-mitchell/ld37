using UnityEngine;
using System.Collections;

public class Furniture : MonoBehaviour {
	public string name;
	public string verb;
	public string fnVerb;

	void Click() { 
		GameController instance = GameController.GetInstance();
		instance.PlaySound(instance.sfxClick);
		instance.SetCurrentTarget(this);
	}

	void MouseOver() { 
		GameController.GetInstance().SetHoverTarget(this);
	}

	void MouseOut() { 
		GameController.GetInstance().SetHoverTarget(null);
	}
}

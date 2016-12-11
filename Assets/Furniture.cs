using UnityEngine;
using System.Collections;

public class Furniture : MonoBehaviour {
	public string name;
	public string verb;
	public string fnVerb;

	private SpriteOutline outline;

	void Start(){
		outline = GetComponent<SpriteOutline>();
		HideOutline();
	}

	public void SetOutlineColor(Color c){
		outline.enabled = true;
		outline.color = c;
	}

	public void HideOutline(){
		outline.enabled = false;
	}

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

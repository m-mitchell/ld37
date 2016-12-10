using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject nextPanel;
	public Text nextNoun;
	public Text nextVerb;

	private Furniture currentTarget = null;
	private static GameController instance;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	public static GameController GetInstance(){
		return instance;
	}

	// Update is called once per frame
	void Update () {
	}

	public void SetCurrentTarget(Furniture f){
		currentTarget = f;
		if(currentTarget==null){
			nextNoun.text = "";
			nextVerb.text = "";
			nextPanel.SetActive(false);
		} else {
			nextNoun.text = currentTarget.name+" ";
			nextVerb.text = currentTarget.verb;
			nextPanel.SetActive(true);
		}
	}
}

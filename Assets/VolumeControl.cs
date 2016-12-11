using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VolumeControl : MonoBehaviour {
	public AudioSource musicSource;
	public Sprite volumeOnSprite;
	public Sprite volumeOffSprite;

	private bool volumeOn = true;
	private float volume;


	// Use this for initialization
	void Start () {
		volume = musicSource.volume;
	}
	
	public void Toggle(){
		if(volumeOn){
			volumeOn = false;
			GetComponent<Image>().sprite = volumeOffSprite;
			musicSource.volume = 0f;
		} else {
			volumeOn = true;
			GetComponent<Image>().sprite = volumeOnSprite;
			musicSource.volume = volume;
		}
	}
}

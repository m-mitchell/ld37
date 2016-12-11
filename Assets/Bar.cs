using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bar : MonoBehaviour {
	public int maxContentWidth;

	private float value = 0.5f;
	private Transform contents;

	// Use this for initialization
	void Start () {
		contents = transform.Find("Fill");
		SetValue(value);
	}
	
	public void SetValue(float v){
		value = v;
		contents.GetComponent<LayoutElement>().preferredWidth = v*maxContentWidth;
	}

	public void ChangeFill(Sprite fillSprite){
		contents.GetComponent<Image>().sprite = fillSprite;
	}
}

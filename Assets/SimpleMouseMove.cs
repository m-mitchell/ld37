using UnityEngine;
using System.Collections;

public class SimpleMouseMove : MonoBehaviour {
	private GameObject prevObj = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		GameObject hit = null;
		double topSV = Mathf.NegativeInfinity;
		RaycastHit2D[] hits = Physics2D.LinecastAll(pos, pos);
		for(int i=0; i<hits.Length; i++){
			SpriteRenderer rend = hits[i].collider.gameObject.GetComponent<SpriteRenderer>();
			if(rend.sortingOrder > topSV){
				hit = rend.gameObject;
				topSV = rend.sortingOrder;
			}
		}

		if(prevObj != null && prevObj != hit){
			prevObj.SendMessage("MouseOut", null, SendMessageOptions.DontRequireReceiver);
			prevObj = null;
		}
		if(hit!=null){
			hit.SendMessage("MouseOver", null, SendMessageOptions.DontRequireReceiver);
			prevObj = hit;
		}
	}
}

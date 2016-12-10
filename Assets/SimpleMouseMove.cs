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
		RaycastHit2D hit = new RaycastHit2D();
		RaycastHit2D[] hits = Physics2D.LinecastAll(pos, pos);
		if(hits.Length > 0){
			hit = hits[hits.Length-1];
		}
		if(prevObj != null && (hit.collider==null || prevObj!=hit.collider.gameObject)){
			prevObj.SendMessage("MouseOut", null, SendMessageOptions.DontRequireReceiver);
			prevObj = null;
		}
		if(hit.collider!=null){
			hit.collider.gameObject.SendMessage("MouseOver", null, SendMessageOptions.DontRequireReceiver);
			prevObj = hit.collider.gameObject;
		}
	}
}

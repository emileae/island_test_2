using UnityEngine;
using System.Collections;

public class Sea : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		GameObject go = col.gameObject;
		if (go.tag == "Player") {
			Debug.Log ("Player hit the surface of the sea...");
			Player playerScript = go.GetComponent<Player> ();
			playerScript.inSea = true;
		}
	}
	void OnTriggerExit(Collider col){
		GameObject go = col.gameObject;
		if (go.tag == "Player" && go.transform.position.y >= transform.position.y) {
			Debug.Log ("Player hit the surface of the sea...");
			Player playerScript = go.GetComponent<Player> ();
			playerScript.inSea = false;
		}
	}

}

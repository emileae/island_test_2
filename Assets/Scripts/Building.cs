using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col)
	{
		GameObject go = col.gameObject;
		if (go.tag == "Player") {
			Debug.Log("Player entered a building.... or tree");
			Player playerScript = go.GetComponent<Player> ();
			playerScript.nearBuilding = true;
		}
	}
	void OnTriggerExit (Collider col)
	{
		GameObject go = col.gameObject;
		if (go.tag == "Player") {
			Debug.Log("Player exited a building.... or tree");
			Player playerScript = go.GetComponent<Player> ();
			playerScript.nearBuilding = false;
		}
	}

}

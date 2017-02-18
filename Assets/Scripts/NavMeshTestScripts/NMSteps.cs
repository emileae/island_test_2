using UnityEngine;
using System.Collections;

public class NMSteps : MonoBehaviour {

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
			NMPlayer playerScript = go.GetComponent<NMPlayer>();
			playerScript.EnterSteps();
		}
	}
	void OnTriggerExit (Collider col)
	{
		GameObject go = col.gameObject;
		if (go.tag == "Player") {
			NMPlayer playerScript = go.GetComponent<NMPlayer>();
			playerScript.ExitSteps();
		}
	}

}

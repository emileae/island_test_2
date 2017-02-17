using UnityEngine;
using System.Collections;

public class NMCamera : MonoBehaviour {

	public Transform player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(player);
		transform.Translate(Vector3.right * Time.deltaTime);
	}
}

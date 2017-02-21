using UnityEngine;
using System.Collections;

public class NMCamera : MonoBehaviour {

	public Transform player;
	public Transform island;
	public float distance = -1000.0f;
	public bool orbitY = false;
	public float yHeight = 40;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
//		transform.LookAt (player);
////		transform.Translate(Vector3.right * Time.deltaTime);
////		transform.position = new Vector3 (player.position.x, transform.position.y, transform.position.z);
//
//		if (orbitY) {
//			transform.RotateAround(player.position, Vector3.up, Time.deltaTime);
//		}

		transform.LookAt (player);
		transform.position = (island.position - player.position).normalized * distance;

		transform.position = new Vector3 (transform.position.x, player.position.y, transform.position.z);


	}
}

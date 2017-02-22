using UnityEngine;
using System.Collections;
using System.Collections.Specialized;

public class NMCamera : MonoBehaviour {

	public Transform player;
	public Transform island;
	public float distance = 100.0f;
	public bool orbitY = false;
	public float yHeight = 20;

	// smoothing
	public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
//		transform.LookAt (player);
////		transform.Translate(Vector3.right * Time.deltaTime);
////		transform.position = new Vector3 (player.position.x, transform.position.y, transform.position.z);
//
//		if (orbitY) {
//			transform.RotateAround(player.position, Vector3.up, Time.deltaTime);
//		}

		transform.LookAt (player);
		Vector3 slopeVector = (player.position - island.position).normalized * distance;

		Vector3 targetPosition = new Vector3 (slopeVector.x, player.position.y + yHeight, slopeVector.z);

		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);


	}
}

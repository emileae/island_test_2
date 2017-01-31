using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	private CharacterController controller;

	public Blackboard blackboard;

	// moving
	private float gravity = -30.0f;
	public float speed = 0.5f;
	private Vector3 direction = Vector3.zero;

	public bool facingRight = true;// bool to get player's ehading

	public bool movingLeft = false;
	public bool movingRight = false;
	public bool movingDown = false;
	public bool movingUp = false;

	// waypoints

	public GameObject currentWaypoint = null;
	public GameObject nextWaypoint = null;
	public GameObject previousWaypoint = null;

	public int currentPlatform = 0;
	public List<GameObject> waypoints = new List<GameObject>();
	public int startingWaypointIndex = 0;

	// changing 'platforms'
	private bool changePlatform = false;

	// Ascending / Descending
	public bool onStairs = false;
	public bool climbingStairs = false;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();

		// get waypoints from blackboard
		if (blackboard == null) {
			blackboard = GameObject.Find ("Blackboard").GetComponent<Blackboard>();
		}

		waypoints = blackboard.platformWaypoints [currentPlatform];

		currentWaypoint = waypoints [startingWaypointIndex];

		if (startingWaypointIndex == 0) {
			previousWaypoint = null;
		}else if (startingWaypointIndex == waypoints.Count - 1){
			nextWaypoint = null;
		}else{
			Waypoint waypointScript = blackboard.platformWaypointScripts [currentPlatform] [startingWaypointIndex];
			previousWaypoint = waypointScript.previousWaypoint;
			nextWaypoint = waypointScript.nextWaypoint;
		}

		transform.position = new Vector3 (waypoints[startingWaypointIndex].transform.position.x, transform.position.y, waypoints[startingWaypointIndex].transform.position.z);

	}
	
	// Update is called once per frame
	void Update ()
	{

		float inputH = Input.GetAxisRaw ("Horizontal");
		float inputV = Input.GetAxisRaw ("Vertical");

		if (inputH > 0) {
			movingLeft = false;
			movingRight = true;
			facingRight = true;
		} else if (inputH < 0) {
			movingLeft = true;
			movingRight = false;
			facingRight = false;
		} else {
			movingLeft = false;
			movingRight = false;
			// no facingRight here since it has to keep its current heading
		}

		if (inputV > 0) {
			movingDown = false;
			movingUp = true;
			facingRight = true;
		} else if (inputV < 0) {
			movingDown = true;
			movingUp = false;
			facingRight = false;
		} else {
			movingDown = false;
			movingUp = false;
		}

		if (onStairs && inputV > 0) {
			climbingStairs = true;
			MoveUpPlatform ();
		}
//		if (onStairs && inputV < 0) {
//			climbingStairs = true;
//			MoveDownPlatform ();
//		}

		NavigateWaypoints ();

		if (climbingStairs) {
			controller.Move (direction * inputV * speed + Vector3.up * gravity * Time.deltaTime + Vector3.up * Time.deltaTime);
		} else {
			controller.Move (direction * inputH * speed + Vector3.up * gravity * Time.deltaTime);
		}

	}

	void NavigateWaypoints ()
	{
		if (facingRight) {
			if (nextWaypoint != null) {
				direction = (nextWaypoint.transform.position - transform.position).normalized;
			} else {
				direction = Vector3.right;
			}
		} else {
			if (previousWaypoint != null) {
				direction = (transform.position - previousWaypoint.transform.position).normalized;
			} else {
				direction = Vector3.right;
			}
		}

	}

	void MoveUpPlatform(){
		onStairs = false;
		currentPlatform += 1;
		nextWaypoint = blackboard.platformWaypoints [currentPlatform] [0];
		direction = (nextWaypoint.transform.position - transform.position).normalized;
	}

}

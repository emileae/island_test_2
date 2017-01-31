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
	public bool firstWaypoint = false;
	public bool lastWaypoint = false;
	public GameObject currentWaypoint = null;
	// horizontal waypoints
	public GameObject nextWaypoint = null;
	public GameObject previousWaypoint = null;
	// vertical waypoints
	public GameObject previousVerticalWaypoint = null;
	public GameObject nextVerticalWaypoint = null;

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

		if (onStairs && inputH != 0) {
			inputH = 0;
		}

		if (!onStairs && inputV != 0) {
			if (!nextVerticalWaypoint && !previousVerticalWaypoint) {
				inputV = 0;
			}
			// for cases of horizontal waypoint at top or bottom of stairs
			// bottom of stairs
			if (nextVerticalWaypoint && inputV < 0) {
				inputV = 0;
			}
			// top of stairs
			if (previousVerticalWaypoint && inputV > 0) {
				inputV = 0;
			}
		}

		

		if (inputH != 0) {
			NavigateWaypoints ();
			controller.Move (direction * inputH * speed + Vector3.up * gravity * Time.deltaTime);
		} else if (inputV != 0) {
			NavigateVerticalWaypoints ();
			controller.Move (direction * inputV * speed + Vector3.up * gravity * Time.deltaTime);
		} else {
			controller.Move (Vector3.up * gravity * Time.deltaTime);
		}

//		NavigateWaypoints ();

//		if (climbingStairs) {
//			controller.Move (direction * inputV * speed + Vector3.up * gravity * Time.deltaTime + Vector3.up * Time.deltaTime);
//		} else {
//			controller.Move (direction * inputH * speed + Vector3.up * gravity * Time.deltaTime);
//		}

	}

	void NavigateWaypoints (){
		if (facingRight) {
			if (!lastWaypoint) {
				direction = (nextWaypoint.transform.position - transform.position).normalized;
			} else {
//				previousWaypoint = currentWaypoint;
				direction = Vector3.right;
			}
		} else {
			if (!firstWaypoint) {
				direction = (transform.position - previousWaypoint.transform.position).normalized;
			} else {
//				nextWaypoint = currentWaypoint;
				direction = Vector3.right;
			}
		}

		Debug.Log ("Direction.... " + direction);

	}

	void NavigateVerticalWaypoints(){
		if (facingRight) {
			if (nextVerticalWaypoint != null) {
				direction = (nextVerticalWaypoint.transform.position - transform.position).normalized;
			} else {
				direction = Vector3.right;
			}
		} else {
			if (previousVerticalWaypoint != null) {
				direction = (transform.position - previousVerticalWaypoint.transform.position).normalized;
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

	void MoveDownPlatform(){
		currentPlatform -= 1;
		SetWaypoints(currentPlatform);//blackboard.platformWaypoints [currentPlatform] [0];
		direction = (nextWaypoint.transform.position - transform.position).normalized;
		Debug.Log("Should be on lower platform");
		climbingStairs = false;
	}

	// find the closest waypoint on a given platform that is an entry to steps
	void SetWaypoints (int platformIndex)
	{
		List<GameObject> potentialWaypoints = new List<GameObject> ();
		for (int i = 0; i < blackboard.platformWaypointScripts [platformIndex].Count; i++) {
			if (blackboard.platformWaypointScripts [platformIndex] [i].steps) {
				potentialWaypoints.Add (blackboard.platformWaypoints [platformIndex] [i]);
			}
		}

		float smallestDistance = Mathf.Infinity;
		int smallestDistanceIndex = 0;
		for (int j = 0; j < potentialWaypoints.Count; j++) {
			float distance = (transform.position - potentialWaypoints [j].transform.position).sqrMagnitude;
			if (distance < smallestDistance) {
				smallestDistance = distance;
				smallestDistanceIndex = j;
			}
		}

		nextWaypoint = potentialWaypoints[smallestDistanceIndex];

	}

}

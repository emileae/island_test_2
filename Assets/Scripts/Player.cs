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

	public bool facingRight = false;// bool to get player's ehading

	public bool movingLeft = false;
	public bool movingRight = false;
	public bool movingDown = false;
	public bool movingUp = false;

	// waypoints
	public float waypointTolerance = 1.0f;
	public bool atWaypoint = false;
	public int currentPlatform = 0;
	public List<GameObject> waypoints = new List<GameObject>();
	public int currentWaypointIndex = 0;
	public int currentSector = 0;
	private bool reachedEndWaypoint = false;
	private bool reachedStartWaypoint = false;

	// changing 'platforms'
	private bool changePlatform = false;

	// Ascending / Descending
	public bool ascending = false;
	public bool descending = false;
	public bool canAscend = false;
	public bool canDescend = false;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();

		// get waypoints from blackboard
		if (blackboard == null) {
			blackboard = GameObject.Find ("Blackboard").GetComponent<Blackboard>();
		}

		waypoints = blackboard.platformWaypoints [currentPlatform];

		transform.position = new Vector3 (waypoints[currentWaypointIndex].transform.position.x, transform.position.y, waypoints[currentWaypointIndex].transform.position.z);

	}
	
	// Update is called once per frame
	void Update () {

		float inputH = Input.GetAxisRaw ("Horizontal");
		float inputV = Input.GetAxisRaw ("Vertical");

		if (inputH > 0) {
			movingLeft = false;
			movingRight = true;
			facingRight = true;
		}else if (inputH < 0){
			movingLeft = true;
			movingRight = false;
			facingRight = false;
		}else{
			movingLeft = false;
			movingRight = false;
			// no facingRight here since it has to keep its current heading
		}

		if (inputV > 0) {
			movingDown = false;
			movingUp = true;
		}else if (inputV < 0){
			movingDown = true;
			movingUp = false;
		}else{
			movingDown = false;
			movingUp = false;
		}

//		FindDirection ();

		GetDirection();

		controller.Move (direction * inputH * speed + Vector3.up * gravity*Time.deltaTime);

	}

	void GetDirection ()
	{
//		Debug.Log ("currentWaypointIndex: " + currentWaypointIndex);
//		Debug.Log ("currentSector: " + currentSector);

		int targetIndex = 0;

		if (movingLeft || movingRight) {
			if (movingRight) {
				targetIndex = currentSector + 1;
				direction = (waypoints [targetIndex].transform.position - transform.position).normalized;
			} else if (movingLeft) {
				targetIndex = currentWaypointIndex;
				direction = (transform.position - waypoints [targetIndex].transform.position).normalized;
			}
		}

		float distanceFromWaypoint = (transform.position - waypoints [targetIndex].transform.position).sqrMagnitude;

//		Debug.Log ("Distance form Waypoint: " + distanceFromWaypoint);

		if (distanceFromWaypoint < waypointTolerance) {
			if (facingRight) {
				Debug.Log ("Reached targetIndex: " + targetIndex);
			} else {
				Debug.Log ("Leaving index: " + targetIndex);
			}
		}
		if (distanceFromWaypoint > waypointTolerance) {
			Debug.Log("Leaving currentSector/Index: " + currentSector);
		}

	}

	void FindDirection ()
	{

		// direction irrespective of whether player is moving

		if (currentWaypointIndex <= waypoints.Count - 1) {
			Vector3 staticDirection = waypoints [currentWaypointIndex + 1].transform.position - transform.position;
			if (staticDirection.sqrMagnitude <= waypointTolerance) {
				Debug.Log ("At a waypoint");
			} else {
				Debug.Log ("Not at a waypoint");
			}
		}

		if (movingRight && currentWaypointIndex >= 0 && currentWaypointIndex < waypoints.Count-1) {
			Vector3 unNormalizedDirection = waypoints [currentWaypointIndex + 1].transform.position - transform.position;
			if (reachedStartWaypoint) {
				unNormalizedDirection = waypoints [currentWaypointIndex].transform.position - transform.position;
			}

			// Entering the waypoint
			if (unNormalizedDirection.sqrMagnitude <= waypointTolerance) {

				if (currentWaypointIndex < waypoints.Count - 1 && !reachedStartWaypoint) {
					currentWaypointIndex += 1;
				}

				if (currentWaypointIndex == 0 && reachedStartWaypoint) {
					reachedStartWaypoint = false;
				}
					
				if (currentWaypointIndex == waypoints.Count - 1) {
					reachedEndWaypoint = true;
				}

			}

			if (unNormalizedDirection.sqrMagnitude < waypointTolerance) {
//				Debug.Log ("moving right and at waypoint....." + currentWaypointIndex);
				atWaypoint = true;
				if (blackboard.platformWaypointScripts [currentPlatform] [currentWaypointIndex].isLadder) {
					canAscend = true;
				}
			}

			direction = unNormalizedDirection.normalized;

			// Leaving the waypoint
			if (unNormalizedDirection.sqrMagnitude > waypointTolerance) {
				atWaypoint = false;
				if (canAscend) {
//					Debug.Log ("Can no longer climb");
					canAscend = false;
				}
			}

		}else if (movingLeft && currentWaypointIndex >= 0 && currentWaypointIndex <= waypoints.Count-1){
			Vector3 unNormalizedDirection =  transform.position - waypoints[currentWaypointIndex].transform.position;

			if (unNormalizedDirection.sqrMagnitude < waypointTolerance) {
				atWaypoint = true;
				if (blackboard.platformWaypointScripts [currentPlatform] [currentWaypointIndex].isLadder) {
//					Debug.Log ("Can climb from here..." + currentWaypointIndex);
					canAscend = true;
				}
			}

			// Entering the waypoint
			if (unNormalizedDirection.sqrMagnitude <= waypointTolerance) {
				if (currentWaypointIndex == 0) {
					reachedStartWaypoint = true;
				}

				if (currentWaypointIndex > 0) {
					currentWaypointIndex -= 1;
				}
			}
			if (reachedEndWaypoint) {
				reachedEndWaypoint = false;
			}
			direction = unNormalizedDirection.normalized;

			// Leaving the waypoint
			if (unNormalizedDirection.sqrMagnitude > waypointTolerance) {
				atWaypoint = false;
				if (canAscend) {
//					Debug.Log ("Can no longer climb");
					canAscend = false;
				}
			}

		}

		if (reachedEndWaypoint) {
//			Debug.Log ("Reached End waypoint");
			direction = Vector3.right;
		}
		if (reachedStartWaypoint) {
//			Debug.Log ("Reached Start waypoint");
			// this vector is adjusted for direction (made negative) by 'inputH'
			direction = Vector3.right;
		}

	}

//	void CheckWaypoint(){
//		Debug.Log ("Check Waypoint: " + currentWaypointIndex + " on platform: " + currentPlatform);
//		if (blackboard.platformWaypointScripts [currentPlatform] [currentWaypointIndex].isLadder) {
//			Debug.Log ("Can climb from here..." + currentWaypointIndex);
////			canAscend = true;
//		}
//	}



}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using System.Net.Configuration;
using UnityEditorInternal.VersionControl;

public class NMPlayer : MonoBehaviour {

	private NMBlackboard blackboard;

	NavMeshAgent agent;

	public List<GameObject> waypoints = new List<GameObject>();
	public List<GameObject> waypoints1 = new List<GameObject>();
	public List<GameObject> waypoints2 = new List<GameObject>();
	public int numPlatforms = 3;
	public List<List<GameObject>> platformWaypoints = new List<List<GameObject>>();

	// platform heights will be set either procedurally or manually, 
	// either way they dont need to be calculated at runtime by player controller
	//public int numPlatforms = 0;
	public List<float> platformHeights = new List<float>();

	public int destinationPlatform;
	public int currentPlatform = 0;
	public bool goingToNewPlatform = false;
	public bool goingUp = false;
	public bool goingDown = false;
	public bool onSteps = false;

	public int currentWaypointIndex = 0;

	public int previousWaypoint = 0;
	public int nextWaypoint = 1;

	// Use this for initialization
	void Start ()
	{
		agent = GetComponent<NavMeshAgent> ();

		platformWaypoints.Add(waypoints);
		platformWaypoints.Add(waypoints1);
		platformWaypoints.Add(waypoints2);

//		for (int i = 0; i < numPlatforms; i++) {
//			platformWaypoints.Add (new List<GameObject> ());
//		}
//		GameObject[] waypoints = GameObject.FindGameObjectsWithTag ("Waypoint");

//		Debug.Log ("waypoints.Length" + waypoints.Length);

//		for (int i = 0; i < waypoints.Length; i++) {
//			NMWaypoint wpScript = waypoints[i].GetComponent<NMWaypoint>();
//			platformWaypoints[wpScript.waypointPlatform].Add(waypoints[i]);
//		}

		if (blackboard == null) {
			blackboard = GameObject.Find("Blackboard").GetComponent<NMBlackboard>();
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		/// Calling NPCs
		bool inputCall = Input.GetButton ("Fire3");
		if (inputCall) {
			blackboard.CallNPCs (transform.position);
		}



		/// Movements
		float inputH = Input.GetAxisRaw ("Horizontal");
		float inputV = Input.GetAxisRaw ("Vertical");

		// both vertical on horizontal movement
		if (inputV != 0 && inputH != 0) {
			if (onSteps) {
				// on steps prioritize vertical movement
				inputH = 0;
			} else {
				// off steps prioritize horizontal movement
				inputV = 0;
			}
		}

		if (inputV != 0 || inputH != 0) {

			// horizontal input
			if (inputH != 0) {
				if (!onSteps) {
					if (inputH > 0) {
						Debug.Log ("nextWaypoint: " + nextWaypoint);
						agent.SetDestination (platformWaypoints [currentPlatform] [nextWaypoint].transform.position);
					} else if (inputH < 0) {
						agent.SetDestination (platformWaypoints [currentPlatform] [previousWaypoint].transform.position);
					}
					// if not on steps and press horizontal button then reset step movement
					if (goingUp || goingDown) {
						// now need to make sure we've got the correct platform
//						if (goingUp) {
//							currentPlatform -= 1;
//						} else if (goingDown) {
//							currentPlatform += 1;
//						}
//						goingToNewPlatform = false;
						goingUp = false;
						goingDown = false;
					}
				}
			}

			// vertical input
			if (inputV != 0) {

				if (inputV > 0) {

					if (onSteps) {
						if (goingUp) {
							goingUp = true;
							goingDown = false;
						} else if (goingDown) {
							destinationPlatform = currentPlatform;
							currentPlatform = destinationPlatform - 1;
							goingUp = true;
							goingDown = false;
						}
					} else {
						Debug.Log ("Increased destination platform...");
						destinationPlatform = currentPlatform + 1;
						goingUp = true;
						goingDown = false;
					}

					if (destinationPlatform <= (platformWaypoints.Count - 1)) {
						agent.SetDestination (platformWaypoints [destinationPlatform] [0].transform.position);
					}
					else {
						inputV = 0;
					}

				} else if (inputV < 0) {

					if (onSteps) {
						if (goingDown) {
//							Debug.Log ("pressing down and so should go down");
							goingUp = false;
							goingDown = true;
//						destinationPlatform = currentPlatform - 1;
						} else if (goingUp) {
//							Debug.Log ("pressing down but should be going up... reconsider");
							destinationPlatform = currentPlatform;
							currentPlatform = destinationPlatform + 1;
							goingUp = false;
							goingDown = true;
						}
					} else {
//						if (currentPlatform > 0) {
						destinationPlatform = currentPlatform - 1;
//						}
						goingUp = false;
						goingDown = true;
					}

					if (destinationPlatform >= 0) {
						agent.SetDestination (platformWaypoints [destinationPlatform] [0].transform.position);
					} else {
						inputV = 0;
					}

				} else {
					goingUp = false;
					goingDown = false;
				}

			}

		}

		// No input... stop movement
		if (inputH == 0 && inputV == 0) {
			agent.ResetPath ();// no movement
		}


	}

	// TODO
	// allow player to cancel going to the stairs, maybe with the horizontal keys?
	// edge case where player is on the steps and then presses horizontal keys?
	// maybe try to use the onSteps bool to control when horizontal keys have an effect?

	public void HitWaypoint (GameObject wp)
	{
		int currentWaypointIndex = platformWaypoints[currentPlatform].IndexOf (wp);

		Debug.Log("currentWaypointIndex: " + currentWaypointIndex);

		NMWaypoint wpScript = wp.GetComponent<NMWaypoint> ();

		if (wpScript.waypointPlatform == currentPlatform) {

			if (currentWaypointIndex > 0) {
				previousWaypoint = currentWaypointIndex - 1;
			}
			if (currentWaypointIndex < platformWaypoints [currentPlatform].Count - 1) {
				nextWaypoint = currentWaypointIndex + 1;
			}
		} else {
			Debug.Log ("Keep going to the upper platform..... maybe if Player presses horizontal button they stay on the platform???");
		}

	}

//	public void LeftWaypoint(GameObject wp){
//		int currentWaypointIndex = platformWaypoints[currentPlatform].IndexOf (wp);
//
//		NMWaypoint wpScript = wp.GetComponent<NMWaypoint> ();
//
//	}

	public void EnterSteps ()
	{
		onSteps = true;
	}
	public void ExitSteps ()
	{
		onSteps = false;
		goingToNewPlatform = false;
		currentPlatform = destinationPlatform;

		// reset platform horizontal waypoints
		currentWaypointIndex = 0;
		previousWaypoint = 0;
		nextWaypoint = 0;
	}
}

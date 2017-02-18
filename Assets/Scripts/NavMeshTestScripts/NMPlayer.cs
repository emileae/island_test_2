using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;

public class NMPlayer : MonoBehaviour {

	NavMeshAgent agent;

	public List<GameObject> waypoints = new List<GameObject>();
	public List<GameObject> waypoints1 = new List<GameObject>();
	public List<List<GameObject>> platformWaypoints = new List<List<GameObject>>();

	public int currentPlatform = 0;
	public bool goingToNewPlatform = false;
	public bool goingUp = false;
	public bool goingDown = false;
	public bool onSteps = false;

	public bool arrivedAtWaypoint = false;
	public int currentWaypointIndex = 0;

	public int previousWaypoint = 0;
	public int nextWaypoint = 1;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();

		platformWaypoints.Add (waypoints);
		platformWaypoints.Add (waypoints1);
	}
	
	// Update is called once per frame
	void Update ()
	{

		float inputH = Input.GetAxisRaw ("Horizontal");
		float inputV = Input.GetAxisRaw ("Vertical");

		if (inputH != 0 && !onSteps) {
			// cancel steps and keep moving along this platform
			Debug.Log ("Cancel going to the new platform and keep moving along the current platform");
			if (goingToNewPlatform) {
				if (goingUp) {
					currentPlatform -= 1;
				} else if (goingDown) {
					currentPlatform += 1;
				}
			}
			goingToNewPlatform = false;
		}

		if (!goingToNewPlatform) {
			if (inputH > 0) {
				agent.SetDestination (platformWaypoints [currentPlatform] [nextWaypoint].transform.position);
			} else if (inputH < 0) {
				agent.SetDestination (platformWaypoints [currentPlatform] [previousWaypoint].transform.position);
			} else {
				agent.ResetPath ();
			}
		} else {
			if (inputV > 0) {
				if (goingUp) {
					agent.SetDestination (platformWaypoints [currentPlatform] [0].transform.position);
				} else if (goingDown) {
					agent.SetDestination (platformWaypoints [currentPlatform + 1] [0].transform.position);
				}
			} else if (inputV < 0) {
				if (goingUp) {
					agent.SetDestination (platformWaypoints [currentPlatform - 1] [0].transform.position);
				} else if (goingDown) {
					agent.SetDestination (platformWaypoints [currentPlatform] [0].transform.position);
				}
			} else {
				agent.ResetPath ();
			}
		}

		if (inputV > 0 && !goingToNewPlatform) {
			// if there are more platforms
			if (platformWaypoints.Count > currentPlatform + 1) {
				currentPlatform += 1;
				goingUp = true;
				goingDown = false;
				goingToNewPlatform = true;
			}
		}
		if (inputV < 0 && !goingToNewPlatform) {
			// if thee are move platforms
			if (currentPlatform - 1 >= 0) {
				currentPlatform -= 1;
				goingUp = false;
				goingDown = true;
				goingToNewPlatform = true;
			}
		}

		// check normal
		if (Input.GetButton ("Fire3")) {
			CheckSlope();
		}

	}


	// this will check if player is moving between platforms
	// can assume that platforms will be flat
	// stairs between platforms may have flat areas as well, may be slopws or stairs
	void CheckSlope ()
	{
		NavMeshHit hit;
        
		// Check all areas one length unit ahead.
		if (!agent.SamplePathPosition (NavMesh.AllAreas, 0.1F, out hit)) {
			Debug.Log ("position: " + hit.position.y);
		}
	}

	// TODO
	// allow player to cancel going to the stairs, maybe with the horizontal keys?
	// edge case where player is on the steps and then presses horizontal keys?
	// maybe try to use the onSteps bool to control when horizontal keys have an effect?

	public void HitWaypoint(GameObject wp){
		int currentWaypointIndex = platformWaypoints[currentPlatform].IndexOf (wp);

		NMWaypoint wpScript = wp.GetComponent<NMWaypoint> ();

		if (wpScript.stepGate) {
			onSteps = true;
		}
		if (wpScript.waypointPlatform == currentPlatform) {
			// hit a waypoint so must be on a platform
			if (goingToNewPlatform) {
				goingToNewPlatform = false;
				goingUp = false;
				goingDown = false;
			}

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

	public void LeftWaypoint(GameObject wp){
		int currentWaypointIndex = platformWaypoints[currentPlatform].IndexOf (wp);

		NMWaypoint wpScript = wp.GetComponent<NMWaypoint> ();

		if (wpScript.stepGate && currentPlatform == wpScript.waypointPlatform) {
			onSteps = false;
		}

	}
}

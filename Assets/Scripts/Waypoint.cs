using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

	public Blackboard blackboard;

	public bool steps = false;

	public bool firstWaypoint = false;
	public bool lastWaypoint = false;

	public int waypointIndex;
	public int platformIndex;

	// horizontal previous and next
	public GameObject previousWaypoint;
	public GameObject nextWaypoint;

	// vertical previous and next
	public GameObject previousVerticalWaypoint;
	public GameObject nextVerticalWaypoint;

	// Use this for initialization
	void Start ()
	{
		if (blackboard == null) {
			blackboard = GameObject.Find ("Blackboard").GetComponent<Blackboard> ();
		}

		waypointIndex = blackboard.platformWaypoints [platformIndex].IndexOf (gameObject);

		int prevIndex = waypointIndex - 1;
		int nextIndex = waypointIndex + 1;

		if (waypointIndex == 0) {
			firstWaypoint = true;
			lastWaypoint = false;
		}else if (waypointIndex == blackboard.platformWaypoints [platformIndex].Count - 1) {
			firstWaypoint = false;
			lastWaypoint = true;
		}

		if (steps) {
			if (prevIndex >= 0) {
				previousVerticalWaypoint = blackboard.platformWaypoints [platformIndex] [prevIndex];
			}

			if (nextIndex <= blackboard.platformWaypoints [platformIndex].Count - 1) {
				nextVerticalWaypoint = blackboard.platformWaypoints [platformIndex] [nextIndex];
			}
		} else {
			if (prevIndex >= 0) {
				previousWaypoint = blackboard.platformWaypoints [platformIndex] [prevIndex];
			}

			if (nextIndex <= blackboard.platformWaypoints [platformIndex].Count - 1) {
				nextWaypoint = blackboard.platformWaypoints [platformIndex] [nextIndex];
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col)
	{
		GameObject go = col.gameObject;
		if (go.tag == "Player") {
			Player playerScript = go.GetComponent<Player> ();

			playerScript.firstWaypoint = firstWaypoint;
			playerScript.lastWaypoint = lastWaypoint;

			playerScript.currentWaypoint = gameObject;

			playerScript.previousWaypoint = previousWaypoint;
			playerScript.nextWaypoint = nextWaypoint;

			playerScript.previousVerticalWaypoint = previousVerticalWaypoint;
			playerScript.nextVerticalWaypoint = nextVerticalWaypoint;

			if (steps) {
				playerScript.onStairs = true;
			} else {
				playerScript.onStairs = false;
			}

		}
	}

	void OnTriggerExit(Collider col){
		GameObject go = col.gameObject;
//		if (go.tag == "Player") {
//			Player playerScript = go.GetComponent<Player> ();
//		}
	}

}

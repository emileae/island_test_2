using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

	public Blackboard blackboard;

	public bool steps = false;

	public int waypointIndex;
	public int platformIndex;
	public GameObject previousWaypoint;
	public GameObject nextWaypoint;

	// Use this for initialization
	void Start () {
		if (blackboard == null) {
			blackboard = GameObject.Find ("Blackboard").GetComponent<Blackboard>();
		}

		waypointIndex = blackboard.platformWaypoints [platformIndex].IndexOf (gameObject);

		int prevIndex = waypointIndex - 1;
		int nextIndex = waypointIndex + 1;

		if (prevIndex >= 0) {
			previousWaypoint = blackboard.platformWaypoints [platformIndex] [prevIndex];
		}

		if (nextIndex <= blackboard.platformWaypoints [platformIndex].Count - 1) {
			nextWaypoint = blackboard.platformWaypoints [platformIndex] [nextIndex];
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		GameObject go = col.gameObject;
		if (go.tag == "Player") {
			Player playerScript = go.GetComponent<Player> ();
			playerScript.currentWaypoint = gameObject;

			playerScript.previousWaypoint = previousWaypoint;
			playerScript.nextWaypoint = nextWaypoint;

			if (steps) {
				playerScript.onStairs = true;
			}

		}
	}

	void OnTriggerExit(Collider col){
		GameObject go = col.gameObject;
		if (go.tag == "Player") {
			Player playerScript = go.GetComponent<Player> ();

			if (steps) {
				playerScript.onStairs = false;
			}
		}
	}

}

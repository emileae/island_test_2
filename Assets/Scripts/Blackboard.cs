using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blackboard : MonoBehaviour {

	public List<GameObject> npcs = new List<GameObject> ();

	public List<GameObject> waypoints0 = new List<GameObject> ();
	public List<GameObject> waypoints1 = new List<GameObject> ();
	public List<GameObject> waypoints2 = new List<GameObject> ();
	public List<List<GameObject>> platformWaypoints = new List<List<GameObject>>();
	public List<List<Waypoint>> platformWaypointScripts = new List<List<Waypoint>>();

	// Use this for initialization
	void Awake () {

		platformWaypoints.Add(waypoints0);
		platformWaypoints.Add(waypoints1);
		platformWaypoints.Add(waypoints2);

		for (int i = 0; i < platformWaypoints.Count; i++) {
			List<Waypoint> waypointScriptList = new List<Waypoint> ();
			for (int j = 0; j < platformWaypoints [i].Count; j++) {
				Waypoint waypointScript = platformWaypoints [i] [j].GetComponent<Waypoint> ();
//				if (j > 0) {
//					waypointScript.previousWaypoint = platformWaypoints [i][j - 1];
//				} else {
//					waypointScript.previousWaypoint = null;
//				}
//				if (j < platformWaypoints [i].Count - 1) {
//					waypointScript.nextWaypoint = platformWaypoints [i][j + 1];
//				} else {
//					waypointScript.nextWaypoint = null;
//				}
				waypointScriptList.Add (waypointScript);
			}
			platformWaypointScripts.Add (waypointScriptList);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

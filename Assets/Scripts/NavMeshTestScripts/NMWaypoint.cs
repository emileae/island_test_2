using UnityEngine;
using System.Collections;

public class NMWaypoint : MonoBehaviour {

	public int waypointPlatform = 0;

	public int orderOnPlatform = 0;

	void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		GameObject go = col.gameObject;
		if (go.tag == "Player") {
//			Debug.Log ("Player triggered waypoint");
			go.GetComponent<NMPlayer>().HitWaypoint (gameObject);
		}
	}

//	void OnTriggerExit(Collider col){
//		GameObject go = col.gameObject;
//		if (go.tag == "Player") {
//			Debug.Log ("Player triggered waypoint");
//			go.GetComponent<NMPlayer>().LeftWaypoint (gameObject);
//		}
//	}

}

using UnityEngine;
using System.Collections;

public class NMNPC : MonoBehaviour {

	NavMeshAgent agent;
	public bool goToTarget = false;
	public Vector3 targetPosition;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (goToTarget) {
			agent.SetDestination(targetPosition);
		}
	}

	public void GoToDestination(Vector3 destination){
		targetPosition = destination;
		goToTarget = true;
	}
}

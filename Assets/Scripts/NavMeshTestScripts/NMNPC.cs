using UnityEngine;
using System.Collections;

public class NMNPC : MonoBehaviour {

	NavMeshAgent agent;
	public bool goToTarget = false;
	public GameObject target;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (goToTarget) {
			agent.SetDestination(target.transform.position);
		}
	}
}

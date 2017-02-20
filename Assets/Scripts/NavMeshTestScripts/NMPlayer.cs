using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;

public class NMPlayer : MonoBehaviour {

	NavMeshAgent agent;

	public List<GameObject> waypoints = new List<GameObject>();
	public List<GameObject> waypoints1 = new List<GameObject>();
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

		// both vertical on horizontal movement
		if (inputV != 0 && inputH != 0) {
			Debug.Log ("V && H");
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
				Debug.Log ("H");
				if (!onSteps) {
					if (inputH > 0) {
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
				Debug.Log ("V");

				// double back had some kind of fix where you had to reset the current platform

				if (inputV > 0) {

					if (onSteps) {
						if (goingUp) {
							Debug.Log ("pressing up and so should go up");
							goingUp = true;
							goingDown = false;
//						destinationPlatform = currentPlatform + 1;
						} else if (goingDown) {
							Debug.Log ("pressing up but should be going down... reconsider");
							destinationPlatform = currentPlatform;
							currentPlatform = destinationPlatform - 1;
							goingUp = true;
							goingDown = false;
						}
					} else {
						destinationPlatform = currentPlatform + 1;
						goingUp = true;
						goingDown = false;
					}


//					goingUp = true;
//					goingDown = false;
//					goingToNewPlatform = true;

					if (destinationPlatform <= (platformWaypoints.Count - 1)) {
						agent.SetDestination (platformWaypoints [destinationPlatform] [0].transform.position);
					}

				} else if (inputV < 0) {
//					destinationPlatform = currentPlatform - 1;
//					goingUp = false;
//					goingDown = true;
//					goingToNewPlatform = true;

					if (onSteps) {
						if (goingDown) {
							Debug.Log ("pressing down and so should go down");
							goingUp = false;
							goingDown = true;
//						destinationPlatform = currentPlatform - 1;
						} else if (goingUp) {
							Debug.Log ("pressing down but should be going up... reconsider");
							destinationPlatform = currentPlatform;
							currentPlatform = destinationPlatform + 1;
							goingUp = false;
							goingDown = true;
						}
					} else {
						destinationPlatform = currentPlatform - 1;
						goingUp = false;
						goingDown = true;
					}

					if (destinationPlatform >= 0) {
						agent.SetDestination (platformWaypoints [destinationPlatform] [0].transform.position);
					}

				} else {
					goingUp = false;
					goingDown = false;
				}

//				if (inputV > 0) {
//					if (currentPlatform < (platformWaypoints.Count - 1)) {
//
//						Debug.Log ("Can move up");
//
//						if (!goingToNewPlatform) {
//							currentPlatform += 1;
//							goingUp = true;
//							goingDown = false;
//							agent.SetDestination (platformWaypoints [currentPlatform] [0].transform.position);
//							goingToNewPlatform = true;
//						}
//
////						if (!goingToNewPlatform) {
////							currentPlatform += 1;
////							goingUp = true;
////							goingDown = false;
////						} else if (goingToNewPlatform) {
////							if (goingDown) {
////								currentPlatform += 1;
////								goingUp = true;
////								goingDown = false;
////							} else if (goingUp) {
////								Debug.Log ("moving in correct vertical location... up");
////							}
////						}
////
////						// go to waypoint
////						agent.SetDestination (platformWaypoints [currentPlatform] [0].transform.position);
////						goingToNewPlatform = true;
////
////						// reset current platform
////						if (currentPlatform > (platformWaypoints.Count - 1)) {
////							currentPlatform = platformWaypoints.Count - 1;
////							goingToNewPlatform = false;
////							agent.ResetPath ();// no movement
////						}
//
//					}
//
//				} else if (inputV < 0) {
//					if (currentPlatform > 0) {
//
//						Debug.Log("Can move down");
//
//						if (!goingToNewPlatform) {
//							currentPlatform -= 1;
//							goingUp = false;
//							goingDown = true;
//							agent.SetDestination (platformWaypoints [currentPlatform] [0].transform.position);
//							goingToNewPlatform = true;
//						}
//
////						if (!goingToNewPlatform) {
////							Debug.Log ("platform -= 1...a");
////							currentPlatform -= 1;
////							goingUp = false;
////							goingDown = true;
////						} else if (goingToNewPlatform) {
////							if (goingUp) {
////								Debug.Log ("platform -= 1...b");
////								currentPlatform -= 1;
////								goingUp = false;
////								goingDown = true;
////							} else if (goingDown) {
////								Debug.Log ("moving in correct vertical location... down");
////							}
////						}
////
////						// go to waypoint
////						agent.SetDestination (platformWaypoints [currentPlatform] [0].transform.position);
////						goingToNewPlatform = true;
////
////						// reset current platform
////						if (currentPlatform < 0) {
////							currentPlatform = 0;
////							goingToNewPlatform = false;
////							agent.ResetPath ();// no movement
////						}
//
//					}
//
//				}
			}

		}

		// No input... stop movement
		if (inputH == 0 && inputV == 0) {
			agent.ResetPath ();// no movement
		}

//		if (!onSteps) {
//			if (inputH != 0) {
//				// cancel steps and keep moving along this platform
//				Debug.Log ("Cancel going to the new platform and keep moving along the current platform");
//				if (goingToNewPlatform) {
//					if (goingUp) {
//						currentPlatform -= 1;
//					} else if (goingDown) {
//						currentPlatform += 1;
//					}
//				}
//				goingToNewPlatform = false;
//			}
//		}
//
//		if (!goingToNewPlatform) {
//			if (inputH > 0) {
//				agent.SetDestination (platformWaypoints [currentPlatform] [nextWaypoint].transform.position);
//			} else if (inputH < 0) {
//				agent.SetDestination (platformWaypoints [currentPlatform] [previousWaypoint].transform.position);
//			} else {
//				agent.ResetPath ();
//			}
//
//			if (inputV > 0) {
//				// if there are more platforms
//				if (platformWaypoints.Count > currentPlatform + 1) {
//					currentPlatform += 1;
//					goingUp = true;
//					goingDown = false;
//					goingToNewPlatform = true;
//				}
//			} else if (inputV < 0) {
//				// if thee are move platforms
//				if (currentPlatform - 1 >= 0) {
//					currentPlatform -= 1;
//					goingUp = false;
//					goingDown = true;
//					goingToNewPlatform = true;
//				}
//			}
//
//		} else if (goingToNewPlatform) {
//			if (inputV > 0) {
//				if (goingUp) {
//					agent.SetDestination (platformWaypoints [currentPlatform] [0].transform.position);
//				} else if (goingDown) {
//					agent.SetDestination (platformWaypoints [currentPlatform + 1] [0].transform.position);
//				}
//			} else if (inputV < 0) {
//				if (goingUp) {
//					agent.SetDestination (platformWaypoints [currentPlatform - 1] [0].transform.position);
//				} else if (goingDown) {
//					agent.SetDestination (platformWaypoints [currentPlatform] [0].transform.position);
//				}
//			} else {
//				agent.ResetPath ();
//			}
//		}


	}

	// TODO
	// allow player to cancel going to the stairs, maybe with the horizontal keys?
	// edge case where player is on the steps and then presses horizontal keys?
	// maybe try to use the onSteps bool to control when horizontal keys have an effect?

	public void HitWaypoint(GameObject wp){
		int currentWaypointIndex = platformWaypoints[currentPlatform].IndexOf (wp);

		NMWaypoint wpScript = wp.GetComponent<NMWaypoint> ();

//		if (wpScript.stepGate) {
//			onSteps = true;
//		}
		if (wpScript.waypointPlatform == currentPlatform) {
			// hit a waypoint so must be on a platform
//			if (goingToNewPlatform) {
//				goingToNewPlatform = false;
//				goingUp = false;
//				goingDown = false;
//			}

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
	}
}

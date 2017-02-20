using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;

public class NMBlackboard : MonoBehaviour {

	public List<GameObject> npcs = new List<GameObject>();
	public List<NMNPC> npcScripts = new List<NMNPC>();

	// Use this for initialization
	void Awake () {
		Transform npcContainer = GameObject.Find("NPCs").transform;

		foreach (Transform child in npcContainer){
			npcs.Add(child.gameObject);
			npcScripts.Add(child.gameObject.GetComponent<NMNPC>());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CallNPCs (Vector3 position)
	{
		for (int i = 0; i < npcScripts.Count; i++) {
			npcScripts[i].GoToDestination(position);
		}	
	}

}

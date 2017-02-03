using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	private Vector3 direction = -Vector3.up;
	public float speed = 0.05f;

	// Use this for initialization
	void Start () {
		speed = speed * Random.Range(0.05f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(direction*speed);
	}
}

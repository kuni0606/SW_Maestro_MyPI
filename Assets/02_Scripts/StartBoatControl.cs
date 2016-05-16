using UnityEngine;
using System.Collections;

public class StartBoatControl : MonoBehaviour {

	public float speed = 50;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.back*speed*Time.deltaTime);
	}
}

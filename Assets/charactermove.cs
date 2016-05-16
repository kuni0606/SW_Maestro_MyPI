using UnityEngine;
using System.Collections;

public class charactermove : MonoBehaviour {

	public float speed = 50.0F;
	public Animation walk;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey (KeyCode.UpArrow) == true) {
			transform.Translate (Vector3.forward * speed);
			walk.Play ();
		} else if (Input.GetKey (KeyCode.DownArrow) == true) {
			transform.Translate (-Vector3.forward * speed);
			walk.Play ();
		} else {
			walk.Stop();
		}
		
	}
}

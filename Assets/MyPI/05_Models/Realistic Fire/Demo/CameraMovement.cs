using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	Vector3 position;
	Quaternion rotation;
	// Use this for initialization
	void Start () {
		position = gameObject.transform.position;
		rotation = gameObject.transform.rotation;
	}
	public float speed = 1f;
	public Transform pivot;
	// Update is called once per frame
	void Update () {
		transform.RotateAround(pivot.transform.position,Vector3.up, speed);
	}

	void OnGUI() {
		if (GUI.Button(new Rect( Screen.width - 110, 10, 100, 30),"Reset Camera")){
			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;
		}
	}
}

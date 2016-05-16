using UnityEngine;
using System.Collections;

public class CharaterManager : MonoBehaviour {
	
	public bool administrator;
	public Transform character;
	public Transform headX;
	public Transform headY;

	private const float MOVE_SPEED = 0.2f;
	private const float ROTATION_SPEED = 1f;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {	
		// Character Move
		Vector3 vec = new Vector3 (0f, 0f, Input.GetAxis ("Vertical"));
		vec *= MOVE_SPEED;
		character.Translate (vec);

		// Character Rotate
		vec = new Vector3 (0f, Input.GetAxis ("Horizontal"), 0f);
		vec *= ROTATION_SPEED;
		character.Rotate(vec);

		// Head Rotate
		if (Input.GetMouseButton (1)) {
			//vec = new Vector3(0f, Input.GetAxis ("Mouse X"), 0f);
			//vec *= ROTATION_SPEED;
			//headX.Rotate(vec);

			vec = new Vector3(-Input.GetAxis("Mouse Y"), 0f, 0f);
			headY.Rotate(vec);
		}
	}
}

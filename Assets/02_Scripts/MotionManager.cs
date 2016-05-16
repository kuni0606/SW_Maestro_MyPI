using UnityEngine;
using System.Collections;

public class MotionManager : MonoBehaviour {

	public bool useHMD;

	public BlockManager blockManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (useHMD) {
		} else {
			MouseControl();
		}
	}

	void MouseControl() {

		if (Input.GetMouseButtonDown (0)) {

			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);	
			if (Physics.Raycast(ray.origin,ray.direction, out hit)) {
				
				Vector3 pos = new Vector3(Mathf.Round(hit.point.x), Mathf.Ceil(hit.point.y), Mathf.Round(hit.point.z));
				blockManager.CreateBlock(null, pos);
			}


		} else if (Input.GetMouseButtonDown (1)) {

		}
	}
}

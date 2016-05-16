using UnityEngine;
using System.Collections;

public class DockCode : MonoBehaviour {

	public GameObject dockDoor;
	public int dockCnt;

	// Use this for initialization
	void Start () {
		dockDoor = GameObject.Find ("dockdoor");
		dockCnt = 0;
	}

	void OnTriggerEnter(Collider other){
		if (dockCnt == 0) {
			dockCnt = 1;
			Debug.Log ("통과함 병시낭");
		} else if (dockCnt == 1) {
			dockCnt = 0;
			Debug.Log("선착장에서 나왔긔 ㅠ");
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}

using UnityEngine;
using System.Collections;

public class LampOnOff : MonoBehaviour {
	public GameObject lamp;
	public int chk = 0;

	// Use this for initialization
	void Start () {
		lamp.SetActive (false);
	}

	void OnTriggerEnter(Collider other) {
		//other.name==사람의 손 일때 ,
		if (chk == 0) {
			lamp.SetActive (true);
			chk = 1;
		} else if (chk == 1) {
			lamp.SetActive(false);
			chk=0;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

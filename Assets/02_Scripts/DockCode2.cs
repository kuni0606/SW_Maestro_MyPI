using UnityEngine;
using System.Collections;

public class DockCode2 : MonoBehaviour {

	public GameObject light1;
	public GameObject light2;
	public GameObject light3;
	public GameObject light4;
	public TOD_Sky skyTime;
	public float time;

	public int lightCnt = 0;

	// Use this for initialization
	void Start () {
		light1.SetActive (false);
		light2.SetActive (false);
		light3.SetActive (false);
		light4.SetActive (false);
	}

	void OnTriggerEnter(Collider other){
		time = skyTime.Cycle.Hour;
		if (lightCnt == 0) {
			if (time >= 9 && time < 19) {
				light1.SetActive (true);
				light2.SetActive (true);
				light3.SetActive (true);
				light4.SetActive (true);
			}
			lightCnt = 1;
		} else if (lightCnt == 1) {
			if (time >= 9 && time < 19) {
				light1.SetActive (false);
				light2.SetActive (false);
				light3.SetActive (false);
				light4.SetActive (false);
			}
			lightCnt = 0;
		}
	}
}

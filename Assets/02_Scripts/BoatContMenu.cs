using UnityEngine;
using System.Collections;

public class BoatContMenu : MonoBehaviour {

	public GameObject powerCanvas;
	public GameObject speedCanvas;
	public int chk=0;

	// Use this for initialization
	void Start () {
		powerCanvas.SetActive (false);
		speedCanvas.SetActive (false);
	}
	
	// Update is called once per frame
	public void activateCanvas(){
		if (chk == 0) {
			powerCanvas.SetActive (true);
			speedCanvas.SetActive (true);
			chk = 1;
		} else if (chk == 1) {
			powerCanvas.SetActive (false);
			speedCanvas.SetActive (false);
			chk = 0;
		}
	}
}

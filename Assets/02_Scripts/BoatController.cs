using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoatController : MonoBehaviour {
	public float speed = 3;
	public float changespeed;
	public float rotationspeed = 30;
	public float a = 0.0001f;
	//public float sspeed = 7;
	//public float mspeed = 17;
	//public float maxspeed = 30;
	public int chk=2;
	public Text tx;
	public string temp;

	public ColorBlock gauge;
	public ColorBlock gauge2;

	public Button[] btn;
	public int btnLength;

	public SightRotate sightRT;

	void Start(){
		btnLength = btn.Length;
		gauge = btn [0].colors;
		gauge2.normalColor = Color.red;
		//gauge.h
	}

	void Update () {
		temp = speed.ToString ("#,#0.0");
		tx.text = temp;
		// Forward movement
		if (chk == 1) {
			Debug.Log (speed);
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
			/*if (speed < sspeed) {
				speed += 0.05f;
			} else if (speed >= sspeed && speed < mspeed) {
				speed += 0.1f;
			} else if (speed >=
			mspeed && speed < maxspeed) {
				speed += 0.2f;
			}*/
			if(speed <= changespeed){
				speed+=0.001f+a;
				a += 0.0005f;
			}else if(speed>changespeed){
				speed-=(0.001f+a);
				a += 0.001f;
			}

		} else if (chk == 0) {
			Debug.Log (speed);
			if(speed > 0){
				transform.Translate (Vector3.forward * speed * Time.deltaTime);
				speed-=(0.001f+a);
				a += 0.001f;
			}
		}

		/* 보트 좌우 움직이는 코드
		// Left movement
		if (Input.GetKey (KeyCode.J)) {
			transform.Rotate (Vector3.down * rotationspeed * Time.deltaTime);
		}
		
		// Right movement
		if (Input.GetKey (KeyCode.L)) {
			transform.Rotate (Vector3.up * rotationspeed * Time.deltaTime);
		}*/
		//transform.Rotate (Vector3.up * sightRT.getBoatSignal () * Time.deltaTime * speed / 60);
	}

	public void SetSpeed(int n){
		changespeed = (n+1)*3;
		for (int i=0; i<btnLength; i++) {
			btn[i].colors = gauge;
		}
		for (int i=0; i<n+1; i++) {
			btn[i].colors = ColorBlock.defaultColorBlock;
		}
		a = 0.0005f;
	}

	public void goHead(){
		speed = 3;
		chk = 1;
	}

	public void stopBoat(){
		chk = 0;
		a = 0.001f;
	}
}

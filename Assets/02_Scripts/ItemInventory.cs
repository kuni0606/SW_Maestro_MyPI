using UnityEngine;
using System.Collections;

public class ItemInventory : MonoBehaviour {

	public GameObject flashLight;
	public GameObject motorBoat;
	public GameObject fireFlame;
	public GameObject lightHouse;
	public GameObject offlight;
	public GameObject glider;
	public GameObject bike;
	public DockCode dock;
	public GameObject person;
	public Vector3 personPosition;
	public Transform boatLocal;
	public Vector3 VehiclePosition;
	public Quaternion personRotate;
	public Quaternion VehicleRotate;
	public Transform gliderLocal;
	public Transform bikeLocal;
	public int flashCnt = 0;
	public int motorCnt = 0;
	public int fireCnt = 0;
	public int lightCnt = 0;
	public SightRotate sightRT;
	public int gliderChk=0;
	public int bikeChk=0;
	public bool bikeEnable = true;
	public BoatController boatCont;


	// Use this for initialization
	void Start () {
		flashLight = GameObject.Find ("FlahsLightCont");
		motorBoat = GameObject.Find ("RibBoat");
		fireFlame = GameObject.Find ("fireFlame");
		lightHouse = GameObject.Find ("LightBulb");
		offlight = GameObject.Find ("OffBulb");
		person = GameObject.Find ("Person");
		glider = GameObject.Find ("Hanglider");
		bike = GameObject.Find ("Motorbike");
		bike.SetActive (false);
		boatCont = motorBoat.GetComponent<BoatController>();
		boatLocal = motorBoat.GetComponent<Transform> ();
		gliderLocal = glider.GetComponent<Transform> ();
		bikeLocal = bike.GetComponent<Transform> ();
		flashLight.SetActive (false);
		//motorBoat.SetActive (false);
		fireFlame.SetActive (false);
		lightHouse.SetActive (false);
		offlight.SetActive (true);
	}

	public void SelectFlashLight(){
		if (flashCnt == 0) {
			flashLight.SetActive (true);
			flashCnt = 1;
		} else if (flashCnt == 1) {
			flashLight.SetActive(false);
			flashCnt = 0;
		}
	}

	public void SelectLight(){
		if (lightCnt == 0) {
			lightHouse.SetActive(true);
			offlight.SetActive(false);
			lightCnt = 1;
		}
		else if(lightCnt == 1){
			lightHouse.SetActive(false);
			offlight.SetActive(true);
			lightCnt = 0;
		}
	}

	public void SelectMotorBoat(){
		/*if (motorCnt == 0) {
			motorBoat.SetActive (true);
			motorCnt = 1;
		} else if (motorCnt == 1) {
			motorBoat.SetActive(false);
			motorCnt = 0;
		}*/
		//보트 탑승 미탑승 코드
		// dockCnt가 0일땐 못탐. 1로 바뀌면 탈수 있음.
		if (dock.dockCnt == 0) {
			Debug.Log ("못타 병신아");
		} else if (dock.dockCnt == 1) {
			Debug.Log ("타고 싶으면 코드를 짜 병신아");
			if(motorCnt == 0){
				Debug.Log ("배 입성!");
				//person의 위치 배위로 변경 , 조작 가능.
				personPosition = person.transform.localPosition;
				VehiclePosition = motorBoat.transform.position;
				VehicleRotate = motorBoat.transform.rotation;
				person.transform.SetParent(boatLocal);
				person.transform.localRotation = new Quaternion(0,0,0,0);
				person.transform.localPosition = new Vector3(0,2f,-1.8f);
				///cah.isGrounded = true;
				//person.transform.position= motorBoat.transform.position.y;
				//이동 없애고 배 조종으로 전환. 메뉴는 소환 가능.
				sightRT.GetComponent<CharacterController>().enabled=false;
				//중력없애.
				sightRT.setBoat(true);
				sightRT.setActiveUpdownBtn(false);
				motorCnt = 1;
			}
			else if(motorCnt == 1){
				Debug.Log ("배에서 내림 뀨");
				boatCont.chk = 2;
				//person의 위치 선착장으로 변경, 조작 불가능.
				motorBoat.transform.position = VehiclePosition;
				motorBoat.transform.rotation = VehicleRotate;
				//Debug.Log (boatPosition);
				//boatPosition = motorBoat.transform.position;
				person.transform.SetParent(null);
				person.transform.localPosition = personPosition;

				//배 조종 없애고 다시 이동 UI소환.
				sightRT.setBoat(false);
				sightRT.GetComponent<CharacterController>().enabled=true;
				sightRT.setActiveUpdownBtn(true);
				motorCnt = 0;
			}
		}
	}

	public void SelectFire(){
		if (fireCnt == 0) {
			fireFlame.SetActive (true);
			fireCnt = 1;
		} else if (fireCnt == 1) {
			fireFlame.SetActive(false);
			fireCnt = 0;
		}
	}

	public void SelectGlider(){
		if (gliderChk == 0) {
			Debug.Log ("탑승");
			gliderChk=1;
			glider.GetComponent<AudioSource>().Play();
			personPosition = person.transform.localPosition;
			VehiclePosition = glider.transform.position;
			VehicleRotate = glider.transform.rotation;
			person.transform.SetParent(gliderLocal);
			person.transform.localRotation = new Quaternion(0,0,0,0);
			person.transform.localPosition = new Vector3(-0.2f,-1f,0f);

			sightRT.GetComponent<CharacterController>().enabled=false;
			sightRT.setGlider(true, glider);
			sightRT.setActiveUpdownBtn(false);
		} else {
			Debug.Log ("해제");
			gliderChk=0;
			glider.GetComponent<AudioSource>().Stop();
			person.transform.SetParent(null);
			person.transform.localPosition = personPosition;

			glider.transform.position = VehiclePosition;
			glider.transform.rotation = VehicleRotate;

			sightRT.setGlider(false, null);
			sightRT.GetComponent<CharacterController>().enabled=true;
			sightRT.setActiveUpdownBtn(true);
		}
	}

	public void SelectMotorBike(){
		if (bikeChk == 0 && bikeEnable) {
			bikeChk = 1;
			bike.SetActive(true);
			bike.transform.localPosition = person.transform.localPosition;
			bike.transform.forward = person.transform.forward;
			person.transform.SetParent (bikeLocal);
			person.transform.localRotation = new Quaternion (0, 0, 0, 0);
			person.transform.localPosition = new Vector3 (0.0f, 15f, 10.0f);
		
			sightRT.GetComponent<CharacterController> ().enabled = false;
			sightRT.setBike (true);
			sightRT.setActiveUpdownBtn (false);
		} else if (bikeChk==1 || bikeEnable==false){
			bikeChk = 0;
			person.transform.SetParent (null);
			person.transform.localPosition = bike.transform.localPosition;

			bike.SetActive(false);

			sightRT.setBike (false);
			sightRT.GetComponent<CharacterController> ().enabled = true;
			sightRT.setActiveUpdownBtn (true);
		}
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Z)) {
			SelectMotorBike();
		}
	}
}

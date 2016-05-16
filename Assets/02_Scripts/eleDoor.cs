using UnityEngine;
using System.Collections;

public class eleDoor : MonoBehaviour {

	public Animation openDoor;
	public AnimationClip[] updown;
	public Animation elv;
	public int clipCnt = 0;
	public GameObject elevator;
	public GameObject person;
	public AudioClip elv_arrive;
	public AudioClip elv_btn;
	public AudioClip elv_updown;
	AudioSource audio;
	public Transform elvLocal;

	// Use this for initialization
	void Start () {
		elv.AddClip (updown[0], "test");
		elv.AddClip (updown [1], "test1");
		elvLocal = elevator.GetComponent<Transform> ();
		audio = GetComponent<AudioSource> ();
		person = GameObject.Find ("Person");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator WaitDing(){
		audio.PlayOneShot (elv_updown, 0.7f);
		yield return new WaitForSeconds (13);
		audio.Stop ();
		yield return new WaitForSeconds (1);
		audio.PlayOneShot (elv_arrive, 0.7f);
		person.transform.SetParent (null);
		openDoor.Play ();
	}

	public void OpenDoor(){
		audio.PlayOneShot (elv_btn, 0.7f);
		//문열림 애니메이션
		person.transform.SetParent (elvLocal);
		openDoor.Play ();
	}

	public void UpElv(){
		audio.PlayOneShot (elv_btn, 0.7f);
		elv.Play ("test");
		StartCoroutine ("WaitDing");
	}

	public void DownElv(){
		audio.PlayOneShot (elv_btn, 0.7f);
		elv.Play ("test1");
		StartCoroutine ("WaitDing");
	}
}

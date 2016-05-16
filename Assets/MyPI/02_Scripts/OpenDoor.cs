using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour {
	public int chkOpen=0;
	public AudioClip openAudio;
	public AudioClip closeAudio;
	AudioSource audio;
	AudioSource caudio;
	public Animation openToInDoor;
	public Animation openToOutDoor;
	public float timeN = 4F;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		caudio = GetComponent<AudioSource> ();
	}
	IEnumerator Wait(){
		yield return new WaitForSeconds (timeN);
		caudio.PlayOneShot(closeAudio, 0.7F);
	}

	void OnTriggerEnter(Collider other) {
		if (other.name == "Person") {
			if (chkOpen == 0) {
				audio.PlayOneShot (openAudio, 0.7F);
				openToInDoor.Play ();
				StartCoroutine ("Wait");
				chkOpen = 1;
			} else if (chkOpen == 1) {
				audio.PlayOneShot (openAudio, 0.7F);
				openToOutDoor.Play ();
				StartCoroutine ("Wait");
				chkOpen = 0;
			}
		}
	}
	// Update is called once per frame
	void Update () {
	}
}

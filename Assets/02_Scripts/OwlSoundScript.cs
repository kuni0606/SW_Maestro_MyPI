using UnityEngine;
using System.Collections;

public class OwlSoundScript : MonoBehaviour {

	public TOD_Sky timeData;
	public float time;
	public AudioClip owlSound;
	AudioSource audio;
	private bool flag;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		flag = false;
	}

	IEnumerator Wait(){
		flag = true;
		audio.PlayOneShot(owlSound,0.1F);
		yield return new WaitForSeconds (12);
		audio.Stop();
		yield return new WaitForSeconds (3);
		Debug.Log ("audiostop");
		flag = false;
	}
	
	// Update is called once per frame
	void Update () {
		time = timeData.Cycle.Hour;


		if (flag)
			return;
		if ((time >= 0 && time < 5F) || (time >= 20F && time <= 23.99F)) {
			StartCoroutine ("Wait");
		}
	}
}

using UnityEngine;
using System.Collections;

public class WalkSoundScript : MonoBehaviour {
	public AudioClip walkSound;
	AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.W))
			audio.PlayOneShot (walkSound, 0.7F);
		if (Input.GetKey (KeyCode.A))
			audio.PlayOneShot (walkSound, 0.7F);
		if (Input.GetKey (KeyCode.S))
			audio.PlayOneShot (walkSound, 0.7F);
		if (Input.GetKey (KeyCode.D))
			audio.PlayOneShot (walkSound, 0.7F);
	}
}

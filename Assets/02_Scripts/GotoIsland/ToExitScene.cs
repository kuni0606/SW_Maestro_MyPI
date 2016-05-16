using UnityEngine;
using System.Collections;

public class ToExitScene : MonoBehaviour {
	public AudioClip clickSound;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnMouseDown(){
		AudioSource.PlayClipAtPoint (clickSound, Vector3.zero);
		Application.Quit ();
	}
}

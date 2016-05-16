using UnityEngine;
using System.Collections;

public class ToEditScene : MonoBehaviour {
	public AudioClip clickSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseUp(){

	}

	void OnMouseDown(){
		AudioSource.PlayClipAtPoint (clickSound, Vector3.zero);
		Application.LoadLevel ("MapEditorScene");
	}
}

using UnityEngine;
using System.Collections;

public class ToCustoScene : MonoBehaviour {
	public AudioClip clickSound;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	IEnumerator OnMouseDown(){
		AudioSource.PlayClipAtPoint (clickSound, Vector3.zero);
		yield return new WaitForSeconds (2);
		Application.LoadLevel ("CustomizingScene");
	}
}

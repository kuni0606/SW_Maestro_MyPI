using UnityEngine;
using System.Collections;

public class ToStartScene : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		Debug.Log ("start");
		yield return new WaitForSeconds(3);
		Debug.Log ("ddddd");
		Application.LoadLevel ("StartScene");
		Debug.Log ("dd");
	}

	
	// Update is called once per frame
	void Update () {
	}
}

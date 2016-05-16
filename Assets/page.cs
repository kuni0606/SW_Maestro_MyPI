using UnityEngine;
using System.Collections;

public class page : MonoBehaviour {

	public static page currentpage;

	void Awake() {
		currentpage = this;
		gameObject.SetActive (false);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

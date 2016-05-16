using UnityEngine;
using System.Collections;

public class SaveLoadManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OpenSave() {
		gameObject.SetActive (true);
	}

	public void OpenLoad() {
		gameObject.SetActive (true);
	}

	public void Close() {
		gameObject.SetActive (false);
	}
}

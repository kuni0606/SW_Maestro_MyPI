using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Browser : MonoBehaviour {

	public static Browser current;

	public Button backbut, homebut, closebut, openbut;
	public GameObject DirPrefab;
	public Transform contents;
	public InputField urltxt;
	public GameObject updownBtn;

	void Awake() {
		current = this;
		gameObject.SetActive (false);
	}

	void OnEnable(){
		updownBtn.SetActive (false);
	}
	void OnDisable(){
		updownBtn.SetActive (true);
	}
}

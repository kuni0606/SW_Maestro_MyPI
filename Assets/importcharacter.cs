using UnityEngine;
using System.Collections;

public class importcharacter : MonoBehaviour {

	[System.Serializable]
	public class CharacterSet {
		public GameObject character;
		public GameObject[] cloths;
	}
	
	public CharacterSet[] characters;

	// Use this for initialization
	void Awake(){
		Debug.Log (Variables.i);
		characters[Variables.i].character.SetActive(true);
		characters[Variables.i].cloths[Variables.j].SetActive(true);	
	}

	void Start () {
	}
	
	// Update is called once per frame

}

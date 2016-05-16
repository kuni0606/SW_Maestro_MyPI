using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		StartCoroutine(StartLoad());
	}

	IEnumerator StartLoad()
	{
		if(Input.GetMouseButton(0)) {
			float fadeTime = GameObject.Find("GameManager").GetComponent<Fading>().BeginFade(1);
			yield return new WaitForSeconds(fadeTime);
			Application.LoadLevel(1);
		}
	}
}

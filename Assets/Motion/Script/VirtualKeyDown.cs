using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using MyLeapMod;

public class VirtualKeyDown : MyLeap {

	bool upStart;
	public GameObject character;
	public float speed = 0.3f;
	public Image img;
	public Sprite pressSprite;
	public Sprite releaseSprite;
	CharacterController rbody;
	// Use this for initialization
	void Start () {
		if (img == null) {
			img = gameObject.GetComponent<Image>();
		}
		rbody = character.GetComponent<CharacterController>();
	}
	
	protected override void onEntered(Collider other){
		upStart  = true;
		img.sprite = pressSprite;
		StartCoroutine("Move");
	}
	protected override void onExited(Collider other){
		upStart = false;
		img.sprite = releaseSprite;
	}
	IEnumerator Move() {
		while (upStart) {
			rbody.Move(-rbody.transform.forward*speed*Time.deltaTime);
			yield return null;
		}
	}
}
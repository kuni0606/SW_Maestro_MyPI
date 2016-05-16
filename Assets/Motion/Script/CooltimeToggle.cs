using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using MyLeapMod;

public class CooltimeToggle : MyLeap {
	public Image img;
	public Toggle btn;
	public float cooltime = 1.0f;
	public bool disableOnStart = true;
	public Collider mycder;
	float leftTime = 1.0f;
	bool cltStart;
	// Use this for initialization
	void Start () {
		if (img == null)
			img = gameObject.GetComponent<Image> ();
		if (btn == null)
			btn = gameObject.GetComponent<Toggle> ();
		if (disableOnStart)
			ResetCooltime ();
		if (mycder == null)
			mycder = gameObject.GetComponent<Collider> ();
		cltStart = false;
	}
	
	protected override void onEntered (Collider other)
	{
		cltStart  = true;
	}
	protected override void onExited (Collider other)
	{
		cltStart = false;
		ResetCooltime ();
	}
	// Update is called once per frame
	void Update () {
		if (cltStart) {
			if (leftTime > 0) {
				leftTime -= Time.deltaTime;
				if (leftTime < 0) {
					leftTime = 0;
					if (btn) {
						btn.enabled = true;
						var pointer = new BaseEventData (EventSystem.current);
						ExecuteEvents.Execute (btn.gameObject, pointer, ExecuteEvents.submitHandler);
						ResetCooltime();
					}
				}
				float ratio = 1.0f - (leftTime / cooltime);
				if (img)
					img.fillAmount = ratio;
			}
		}
	}
	
	public bool CheckCooltime(){
		if (leftTime > 0)
			return false;
		else
			return true;
	}
	
	public void ResetCooltime(){
		leftTime = cooltime;
		img.fillAmount = 0;
		if (btn)
			btn.enabled = false;
	}
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class clickBtn : MonoBehaviour {

	public Sprite pressed,released;
	public Image image;

	void OnTriggerEnter(Collider other){
		if (other.gameObject.name.Equals ("bone3")) {
			var pointer = new BaseEventData (EventSystem.current);
			ExecuteEvents.Execute (gameObject, pointer, ExecuteEvents.submitHandler);
			if (image!=null){
				image.sprite=pressed;
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.name.Equals ("bone3") && image!=null) {
			image.sprite=released;
		}
	}
}

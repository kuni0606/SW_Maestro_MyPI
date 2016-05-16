using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MySlider : MonoBehaviour {
	GameObject m_target=null;
	PointerEventData pointer;

	void OnTriggerEnter(Collider other){
		if (m_target == null && other.gameObject.tag=="clickfg") {
			m_target=other.gameObject;
			pointer = new PointerEventData (EventSystem.current);
			pointer.selectedObject = gameObject;
			ExecuteEvents.Execute(gameObject,pointer,ExecuteEvents.beginDragHandler);

		}
	}
	void OnTriggerExit(Collider other){
		if (m_target == other.gameObject) {
			m_target=null;
			ExecuteEvents.Execute(gameObject,pointer,ExecuteEvents.endDragHandler);
			pointer = null;
		}
	}
	void OnTriggerStay(Collider other){
		if (other.gameObject == m_target) {
			pointer.position = m_target.transform.position;
			ExecuteEvents.Execute(gameObject,pointer,ExecuteEvents.dragHandler);
		}
	}
}
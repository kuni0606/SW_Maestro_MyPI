using System;
using UnityEngine;
namespace MyLeapMod
{
	public abstract class MyLeap : MonoBehaviour
	{
		protected GameObject m_target = null;

		private bool IsHand(Collider collider)
		{
			return collider.transform.parent && collider.transform.parent.parent && collider.transform.parent.parent.GetComponent<HandModel>();
		}
		protected virtual void OnTriggerEnter(Collider other){
			if (m_target == null && other.gameObject.tag.Equals("clickfg")) {
				m_target=other.gameObject;
				onEntered (other);
			}
		}
		protected virtual void OnTriggerExit(Collider other){
			if (other.gameObject == m_target) {
				m_target=null;
				onExited (other);
			}
		}
		protected virtual void OnTriggerStay(Collider other){

		}
		protected abstract void onEntered(Collider other);
		protected abstract void onExited(Collider other);
	}
}
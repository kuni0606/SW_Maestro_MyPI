using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Mypi {
	namespace MapEditor {
		#if UNITY_EDITOR
		[CanEditMultipleObjects]
		#endif
		public class DropDownMenu : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler {

			public RectTransform menuTransform;
			public float animationTime;
			public float upY;
			public float downY;

			public RectTransform contents;
			public Text title;

			private bool opened;

			[Serializable]
			public class MenuClickedEvent : UnityEvent {
			}
			public MenuClickedEvent onClick;

			[Serializable]
			public class Menu {
				public string title;
				public Button button;
			}
			
			public List<Menu> menus;

			private float timeRemain;

			
			void Start() {
				
				opened = false;
				InitializeSelectedMenu ();
				InitializeContentsPosition ();
				InitializeMenus ();
			}
			
			void InitializeSelectedMenu() {
				if (title == null)
					return;

				if (menus.Count <= 0)
					return;
				
				title.text = menus [0].title;
			}
				
			void InitializeContentsPosition() {
				if (contents == null)
					return;

				if (opened) {
					menuTransform.sizeDelta = new Vector2(0f, upY);
					contents.anchoredPosition = new Vector2 (0f, downY);
				} else {
					menuTransform.sizeDelta = Vector2.zero;
					contents.anchoredPosition = new Vector2 (0f, upY);
				}
			}

			void InitializeMenus() {
				foreach (Menu menu in menus) {
					Button button = menu.button;
					if (button == null)
						continue;

					string menuTitle = menu.title;
					button.GetComponentInChildren<Text>().text = menuTitle;
					button.onClick.AddListener(()=>OnSelectMenu(menuTitle));
				}
			}

			public void OnPointerClick (PointerEventData eventData) {
				if (timeRemain > 0f)
					return;

				if (opened) {
					//DoStateTransition(SelectionState.Highlighted, false);
					StartCoroutine("FadeOut");
				} else {
					//DoStateTransition(SelectionState.Pressed, false);
					StartCoroutine("FadeIn");
				}
			
			}

			#region ISubmitHandler implementation

			void ISubmitHandler.OnSubmit (BaseEventData eventData)
			{
				throw new NotImplementedException ();
			}

			#endregion
			
		//	public void OnSubmit (BaseEventData eventData) {
		//		Debug.Log ("Submit");
		//	}

			IEnumerator FadeIn() {
				timeRemain = animationTime;
				while(timeRemain > 0f){
					timeRemain -= Time.deltaTime;
					menuTransform.sizeDelta = new Vector2(0f, Mathf.Lerp (upY, 0f, timeRemain / animationTime));
					contents.anchoredPosition = new Vector2(0f, Mathf.Lerp(downY, upY, timeRemain / animationTime));
					yield return null;
				}
				
				menuTransform.sizeDelta = new Vector2(0f, upY);
				contents.anchoredPosition = new Vector2 (0f, downY);
				opened = true;
			}

			IEnumerator FadeOut() {
				timeRemain = animationTime;
				while(timeRemain > 0f){
					timeRemain -= Time.deltaTime;
					menuTransform.sizeDelta = new Vector2(0f, Mathf.Lerp (0f, upY, timeRemain / animationTime));
					contents.anchoredPosition = new Vector2(0f, Mathf.Lerp(upY, downY, timeRemain / animationTime));
					yield return null;
				}

				menuTransform.sizeDelta = Vector2.zero;
				contents.anchoredPosition = new Vector2 (0f, upY);
				opened = false;
			}

			public void OnSelectMenu(string menuTitle) {
				title.text = menuTitle;
				StartCoroutine ("FadeOut");
			}
		}
	}
}

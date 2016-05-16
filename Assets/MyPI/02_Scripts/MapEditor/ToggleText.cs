using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;

namespace Mypi {
	namespace MapEditor {
		public class ToggleText : MonoBehaviour {
			public string first;
			public string second;

			private Text text;

			public bool toggled { get; set; }


			void Awake() {
				text = GetComponentInChildren<Text> ();
			}

			void Start() {
				if (!toggled)
					text.text = first;
				else
					text.text = second;
			}

			public void Toggle() {
				if (!toggled) {
					text.text = second;
					toggled = true;
				} else {
					text.text = first;
					toggled = false;
				}
			}
		}
	}
}

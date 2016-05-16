using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace Mypi {
	namespace MapEditor {

		public class Tooltip : MonoBehaviour {
			public static Tooltip current { get; set; }

			public Text xText;
			public Text yText;
			public Text zText;
			public Text wText;
			public Text lText;
			public Text hText;

			public int x {
				set {
					xText.text = value.ToString();
				}
			}
			public int y {
				set {
					yText.text = value.ToString();
				}
			}
			public int z {
				set {
					zText.text = value.ToString();
				}
			}
			public int w {
				set {
					wText.text = value.ToString();
				}
			}
			public int l {
				set {
					lText.text = value.ToString();
				}
			}
			public int h {
				set {
					hText.text = value.ToString();
				}
			}

			private RectTransform rt;

			void Awake() {
				current = this;
				rt = GetComponent<RectTransform> ();
			}

			void Start() {
				Show (false);
			}

			void Update() {
				rt.anchoredPosition = new Vector2 (Input.mousePosition.x + 10f, Input.mousePosition.y + 5f);
			}

			public void Show(bool value) {
				gameObject.SetActive (value);
			}

			public void SetCoord(int x, int y, int z) {
				this.x = x;
				this.y = y;
				this.z = z;
			}

			public void SetSize(int w, int l, int h) {
				this.w = w;
				this.l = l;
				this.h = h;
			}

			public void SetCoord(IntVector3 coord) {
				x = coord.x;
				y = coord.y;
				z = coord.z;
			}

			public void SetSize(IntVector3 size) {
				w = size.x;
				l = size.y;
				h = size.z;
			}
		}
	}
}
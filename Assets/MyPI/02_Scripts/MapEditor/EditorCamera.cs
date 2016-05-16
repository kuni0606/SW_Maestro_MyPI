using UnityEngine;
using System.Collections;

namespace Mypi {
	namespace MapEditor {
		public class EditorCamera : MonoBehaviour {
			public Transform frame;
			public Camera camera;

			private enum ViewMode {Normal, Top};
			private ViewMode viewMode;

			private int layerMask;
			
			private Vector3 normalPosition;
			private float normalXAngle;
			private float normalYAngle;

			private float _zoomRatio;
			public float zoomRatio {
				get {
					return _zoomRatio;
				}
				set {
					_zoomRatio = value;
					camera.orthographicSize = Mathf.Pow (1.5f, Mathf.Clamp (value, 0f, 10f));
				}
			}

			void Awake() {
				camera = GetComponent<Camera> ();
				layerMask = LayerMask.GetMask(new string[]{"Block", "Ground"});

				// Init
				normalPosition = new Vector3 (-10f, 10f, -10f);
				normalXAngle = 45f;
				normalYAngle = 30f;

				zoomRatio = 5f;
			}

			// Update is called once per frame
			void Update () {

				HandleMouse ();
				HandleKey ();
			}

			void HandleMouse() {
				if (camera.pixelRect.Contains (Input.mousePosition)) {
					Zoom (-2f * Input.GetAxis ("Mouse ScrollWheel"));
				}

				if (Input.GetMouseButton (0)) {
				} else if (Input.GetMouseButton (1)) {
					Rotate (2f * Input.GetAxis ("Mouse X"), -2f * Input.GetAxis ("Mouse Y"));
				}
			}

			void HandleKey() {

				Move (new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0f));
			}

			// Control camera
			public void SetPosition(Vector3 position) {
				frame.localPosition = position;
			}

			public void SetAngle(float xAngle, float yAngle) {
				frame.localEulerAngles = new Vector3 (0f, xAngle, 0f);
				transform.localEulerAngles = new Vector3 (yAngle, 0f, 0f);
			}

			public void Move(Vector3 move) {
				frame.Translate (transform.TransformVector(move), Space.World);
			}

			public void Rotate(float xAngle, float yAngle) {
				frame.Rotate (new Vector3 (0f, xAngle, 0f));
				transform.Rotate (new Vector3 (yAngle, 0F, 0f));
			}

			public void Zoom(float delta) {
				zoomRatio = Mathf.Clamp (zoomRatio + delta, 0f, 10f);
				camera.orthographicSize = Mathf.Pow (1.5f, zoomRatio);
			}

			// Control view
			public void ChangeFieldOfView(float delta) {
				if (camera.orthographic) {
					camera.orthographicSize += delta;
				} else {
					camera.fieldOfView += delta;
				}
			}

			public void SetNormalViewMode() {
				if (viewMode == ViewMode.Normal)
					return;

				viewMode = ViewMode.Normal;

				frame.position = normalPosition;
				frame.localEulerAngles = new Vector3 (0f, normalXAngle, 0f);
				transform.localEulerAngles = new Vector3 (normalYAngle, 0f, 0f);
			}

			public void SetTopViewMode() {
				if (viewMode == ViewMode.Top)
					return;

				viewMode = ViewMode.Top;
				camera.orthographic = true;
				normalPosition = frame.position;
				normalXAngle = frame.localEulerAngles.y;
				normalYAngle = transform.localEulerAngles.x;

				Vector3 topPosition = GetPivot ();
				frame.position = new Vector3 (topPosition.x, 200f, topPosition.z);
				frame.localEulerAngles = Vector3.zero;
				transform.localEulerAngles = new Vector3 (90f, 0f, 0f);
			}

			public void RotateView(float angle) {
				Vector3 pivot = GetPivot ();
				frame.RotateAround(pivot, Vector3.up, angle);
			}
			
			Vector3 GetPivot() {
				Ray ray = new Ray (transform.position, transform.forward);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
					return hit.point;
				}
				
				return normalPosition;
			}
		}
	}
}

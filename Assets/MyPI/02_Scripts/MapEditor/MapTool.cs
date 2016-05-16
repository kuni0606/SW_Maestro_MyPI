using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Mypi {
	namespace MapEditor {
		public abstract class MapTool : MonoBehaviour {
			private static float COORD_MIN = -7000f;
			private static float COORD_MAX = 7000f;

			protected struct PositionConvertor{

				public Convert convXDelegate;
				public Convert convYDelegate;
				public Convert convZDelegate;
				
				public PositionConvertor(Convert cx, Convert cy, Convert cz) {
					convXDelegate = cx;
					convYDelegate = cy;
					convZDelegate = cz;
				}
			};
			protected delegate float Convert(float number);
			protected delegate bool CheckPosition(Vector3 position);

			protected readonly Color transparentGreen = new Color (0.13f, 0.93f, 0.33f, 0.47f);
			protected readonly Color transparentRed = new Color (0.93f, 0.13f, 0.33f, 0.47f);

			protected bool isInUse { get; set; }

			public MapManager mapManager;
			public Camera mapCamera;


			public virtual void Initialize() {		
			}

			public void SetActive(bool value) {
				isInUse = value;
				gameObject.SetActive (value);
			}

			protected bool Raycast(out RaycastHit hit, float distance, int layerMask) {
				hit = new RaycastHit ();

				if (EventSystem.current.IsPointerOverGameObject ())
					return false;

				Ray ray = mapCamera.ScreenPointToRay (Input.mousePosition);
				if (!Physics.Raycast (ray, out hit, distance, layerMask))
					return false;

				return true;
			}
			
			protected bool IsValidXYZ(Vector3 v) {
				return v.x >= COORD_MIN && v.x <= COORD_MAX
					&& v.y >= COORD_MIN && v.y <= COORD_MAX
						&& v.z >= COORD_MIN && v.z <= COORD_MAX;
			}
			
			protected bool IsValidXZ(Vector3 v) {
				return v.x >= COORD_MIN && v.x <= COORD_MAX
					&& v.z >= COORD_MIN && v.z <= COORD_MAX;
			}
			
			protected bool IsValidY(Vector3 v) {
				return v.y >= COORD_MIN && v.y <= COORD_MAX;
			}
		}
	}
}
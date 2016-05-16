using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Mypi.MapEditor {
	public class MassBuilder : MapTool {
		public Transform xzSizeSelector;

		public Transform locator;
		public Renderer locatorRenderer;
	
		public string[] firstLayerNames;
		public string[] secondLayerNames;
		public string[] checkLayerNames;
							
		private Dictionary<Vector3, PositionConvertor>[] convertors;
		private int[] raycastLayers;
		private int checkLayers;
		
		private bool isValidLocation;
		private bool isBuildable;

		private CheckPosition[] checks;	
		private string selectedBlock;
		private int colliderCount;

		private int currentStep;

		private Vector3 startPosition;
		private Vector3 endPosition;


		void Awake() {
			SetActive (false);

			raycastLayers = new int[2];
			raycastLayers[0] = LayerMask.GetMask(firstLayerNames);
			raycastLayers[1] = LayerMask.GetMask(secondLayerNames);

			checkLayers = LayerMask.GetMask (checkLayerNames);

			convertors = new Dictionary<Vector3, PositionConvertor>[3];
			convertors [0] = new Dictionary<Vector3, PositionConvertor> ();
			convertors [0] [Vector3.forward] = new PositionConvertor (Mathf.Round, Mathf.Round, Mathf.Ceil);
			convertors [0] [Vector3.back] = new PositionConvertor (Mathf.Round, Mathf.Round, Mathf.Floor);
			convertors [0] [Vector3.left] = new PositionConvertor (Mathf.Floor, Mathf.Round, Mathf.Round);
			convertors [0] [Vector3.right] = new PositionConvertor (Mathf.Ceil, Mathf.Round, Mathf.Round);
			convertors [0] [Vector3.up] = new PositionConvertor (Mathf.Round, Mathf.Ceil, Mathf.Round);
			convertors [0] [Vector3.down] = new PositionConvertor (Mathf.Round, Mathf.Floor, Mathf.Round);

			convertors [1] = new Dictionary<Vector3, PositionConvertor> ();
			convertors [1] [Vector3.up] = new PositionConvertor (Mathf.Round, Mathf.Ceil, Mathf.Round);

			checks = new CheckPosition[3];
			checks [0] = IsValidXYZ;
			checks [1] = IsValidXZ;

			selectedBlock = null;
		}

		public override void Initialize() {
			startPosition = Vector3.zero;
			endPosition = Vector3.zero;

			Preview ();
			SetToolActive (true);
			SetBuildable (true);

			isValidLocation = false;
			
			currentStep = 0;
		}

		
		public void SetBlock(string blockName) {
			if (selectedBlock == blockName)
				return;
			
			selectedBlock = blockName;
		}

		void Update() {
			if (Input.GetKeyDown(KeyCode.Q))
			    mapManager.DebugSingleBlocks ();

			if (Input.GetKeyDown (KeyCode.Escape))
				Cancel ();

			if (!mapCamera.pixelRect.Contains (Input.mousePosition)) {
				isValidLocation = true;
				return;
			}
			
			Vector3 point = Vector3.zero;
			if (currentStep == 0) {
				isValidLocation = GetPosition (out point, 0) && checks [0] (point);
				SetStartPosition (point);
				if (isValidLocation) {
					if (Input.GetMouseButtonDown (0))
						currentStep = 1;
					SetToolActive (true);
					Preview ();
					SetBuildable (!mapManager.Contains(new IntVector3(point)));
				} else {
					SetToolActive (false);
					SetBuildable (true);
					colliderCount = 0;
				}
			} else if (currentStep == 1) {
				isValidLocation = GetPosition (out point, 1) && checks [1] (point);
				SetEndPosition (point);
				if (Input.GetMouseButtonUp (0)) {
					BuildBlocks ();
					currentStep = 0;
				}
				
				SetToolActive (true);
				if (isValidLocation) {
					Preview ();
					SetBuildable(!mapManager.Contains(new IntVector3(startPosition), new IntVector3(endPosition)));
				}
			}
			
			Tooltip.current.SetCoord (new IntVector3 (point));
			Tooltip.current.Show (isValidLocation);
		}

		void Preview() {

			Vector3 scale = (endPosition - startPosition);
			Vector3 position = (startPosition + endPosition) * 0.5f;
			scale.x = Mathf.Abs (scale.x) + 1f;
			scale.y = Mathf.Abs (scale.y) + 1f;
			scale.z = Mathf.Abs (scale.z) + 1f;
			locator.localScale = scale;
			locator.localPosition = position;

			Tooltip.current.SetSize (new IntVector3 (scale));
		}

		bool GetPosition(out Vector3 point, int step) {

			RaycastHit hit;	
			point = new Vector3 ();
			
			if (!Raycast (out hit, Mathf.Infinity, raycastLayers[step]))
				return false;
			
			// Convert position and normal from world to local
			point = mapManager.currentMapData.mapTransform.InverseTransformPoint(hit.point);
			Vector3 normal = 100f * hit.transform.InverseTransformDirection(hit.normal);
			normal.Set (Mathf.Round (normal.x), Mathf.Round (normal.y), Mathf.Round (normal.z));
			normal *= 0.01f;

			if (!convertors[step].ContainsKey(normal))
				return false;
			
			PositionConvertor pc = convertors[step][normal];
			point.x = pc.convXDelegate(point.x);
			point.y = pc.convYDelegate(point.y);
			point.z = pc.convZDelegate(point.z);
			
			return true;
		}

		void SetStartPosition(Vector3 position) {
			startPosition = position;
			endPosition = position;

			xzSizeSelector.localPosition = new Vector3 (0f, position.y, 0f);
		}

		void SetEndPosition(Vector3 position) {
			endPosition = position;
		}

		void Cancel() {
			if (currentStep != 1)
				return;

			currentStep = 0;
		}

		void SetToolActive(bool value) {
			locator.gameObject.SetActive (value);			
			xzSizeSelector.gameObject.SetActive (value);
		}

		void BuildBlocks() {
			if (!isValidLocation || !isBuildable)
				return;

			mapManager.BuildBlock (selectedBlock, new IntVector3(startPosition), new IntVector3(endPosition));
		}
//
//		void OnTriggerEnter(Collider collider) {
//			int mask = (1 << collider.gameObject.layer);
//			
//			if ((mask & checkLayers) == 0)
//				return;
//
//			Debug.Log ("Enter:" + colliderCount + "->" + (colliderCount + 1));
//			if (colliderCount++ == 0) {
//				Debug.Log(collider.transform.parent);
//				SetBuildable (false);
//			}
//		}
//		
//		void OnTriggerExit(Collider collider) {
//			int mask = (1 << collider.gameObject.layer);
//			if ((mask & checkLayers) == 0)
//				return;
//
//			Debug.Log ("Exit:" + colliderCount + "->" + (colliderCount - 1));
//			if (--colliderCount == 0)
//				SetBuildable (true);
//		}
		
		void SetBuildable(bool value) {
			if (value) {
				locatorRenderer.material.color = transparentGreen;
				isBuildable = true;
			} else {
				locatorRenderer.material.color = transparentRed;
				isBuildable = false;
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mypi.Block;

namespace Mypi {
	namespace MapEditor {
		public class SingleBuilder : MapTool {
			public Transform container;
			public Transform locatorTransform;
			public MeshRenderer locatorRenderer;

			public string[] layerNames;
			public string[] checkLayerNames;
			public string previewLayerName;

			private BlockObject assignedBlock;

			private Dictionary<string, BlockObject> previewBlocks;	
			private Dictionary<Vector3, PositionConvertor> convertors;
			private int raycastLayers;
			private int checkLayers;
			
			private bool isValidLocation;
			private bool isBuildable;
			
			private string selectedBlock;


			// Temp variable
			private Vector3 angle;
			private Vector3 point;


			void Awake() {
				SetActive (false);

				raycastLayers = LayerMask.GetMask (layerNames);
				checkLayers = LayerMask.GetMask (checkLayerNames);

				convertors = new Dictionary<Vector3, PositionConvertor>();
				convertors [Vector3.forward] = new PositionConvertor (Mathf.Round, Mathf.Round, Mathf.Ceil);
				convertors [Vector3.back] = new PositionConvertor (Mathf.Round, Mathf.Round, Mathf.Floor);
				convertors [Vector3.left] = new PositionConvertor (Mathf.Floor, Mathf.Round, Mathf.Round);
				convertors [Vector3.right] = new PositionConvertor (Mathf.Ceil, Mathf.Round, Mathf.Round);
				convertors [Vector3.up] = new PositionConvertor (Mathf.Round, Mathf.Ceil, Mathf.Round);
				convertors [Vector3.down] = new PositionConvertor (Mathf.Round, Mathf.Floor, Mathf.Round);

				previewBlocks = new Dictionary<string, BlockObject> ();


				List<string> blockNames = PoolManager.current.GetBlockNames();
				foreach (string blockName in blockNames) {
					BlockObject blockObject;
					if (!PoolManager.current.AllocateBlock(out blockObject, blockName))
						continue;

					blockObject.layer = LayerMask.NameToLayer (previewLayerName);
					blockObject.parent = container;

					blockObject.coord = IntVector3.zero;
					blockObject.angle = 0;

					blockObject.color = transparentGreen;

					previewBlocks[blockName] = blockObject;
					blockObject.SetActive(false);
				}

				selectedBlock = null;
				assignedBlock = null;
			}

			void Update() {
				HandleMouse ();
				HandleKey ();

				container.gameObject.SetActive (isValidLocation);
				Tooltip.current.Show (isValidLocation);

				if (!isValidLocation) {
					SetBuildable(true);
				}
			}

			void HandleMouse() {
				if (!mapCamera.pixelRect.Contains (Input.mousePosition)) {
					isValidLocation = false;
					return;
				}

				Preview();
				if (assignedBlock != null) {
					SetBuildable(!mapManager.Contains(new IntVector3(point), assignedBlock.GetRanges(), assignedBlock.GetCoords()));
				}

				container.gameObject.SetActive (isValidLocation);
				Tooltip.current.Show (isValidLocation);

				if (Input.GetMouseButtonDown (0)) {
					Build();
				}
			}

			void HandleKey() {
			}
			
			public override void Initialize() {
				angle = Vector3.zero;
				point = Vector3.zero;
				isValidLocation = false;
				
				container.gameObject.SetActive (true);
				container.localEulerAngles = angle;

				SetBuildable (true);
			}

			public void Preview() {
				if (Input.GetAxis ("Mouse X") == 0f && Input.GetAxis ("Mouse Y") == 0f)
					return;
				
				isValidLocation = GetPosition (out point);
				if (!isValidLocation)
					return;

				if (!IsValidXYZ (point)) {
					isValidLocation = false;
					return;
				}

				point = mapManager.currentMapData.mapTransform.TransformPoint (point);

				container.localPosition = point;
				container.localRotation.eulerAngles.Set (angle.x, angle.y, angle.z);

				Tooltip.current.SetCoord (Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y), Mathf.RoundToInt(point.z));
				Tooltip.current.SetSize (1, 1, 1);
			}
				
			bool GetPosition(out Vector3 point) {
				point = new Vector3 ();

				RaycastHit hit;
				if (!Raycast (out hit, Mathf.Infinity, raycastLayers))
					return false;

				// Convert position and normal from world to local
				point = mapManager.currentMapData.mapTransform.InverseTransformPoint(hit.point);
				Vector3 normal = 100f * hit.transform.InverseTransformDirection(hit.normal);
				normal.Set (Mathf.Round (normal.x), Mathf.Round (normal.y), Mathf.Round (normal.z));
				normal *= 0.01f;

				if (!convertors.ContainsKey(normal))
					return false;

				PositionConvertor pc = convertors[normal];
				point.x = pc.convXDelegate(point.x);
				point.y = pc.convYDelegate(point.y);
				point.z = pc.convZDelegate(point.z);

				return true;
			}
			
			public void Build() {
				if (!isValidLocation || !isBuildable)
					return;
			
				mapManager.BuildBlock (selectedBlock, new IntVector3(point), Mathf.RoundToInt(container.localEulerAngles.y));
			}

			public void RotateLeft() {
				angle.y -= 90f;
				container.localEulerAngles = angle;
			}
			
			public void RotateRight() {
				angle.y += 90f;
				container.localEulerAngles = angle;
			}

			public void SetBlock(string blockName) {
				if (selectedBlock == blockName)
					return;

				selectedBlock = blockName;

				if (assignedBlock != null)
					assignedBlock.SetActive (false);
				assignedBlock = previewBlocks [selectedBlock];
				assignedBlock.angle = 0;
				assignedBlock.SetActive (true);

				container.localEulerAngles = Vector3.zero;
				angle.y = 0;
			}

			void SetBuildable(bool value) {
				if (value) {
					assignedBlock.color = transparentGreen;
					locatorRenderer.material.color = transparentGreen;
					isBuildable = true;
				} else {
					assignedBlock.color = transparentRed;
					locatorRenderer.material.color = transparentRed;
					isBuildable = false;
				}
			}
		}
	}
}

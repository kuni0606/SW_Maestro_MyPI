using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mypi.Block;

namespace Mypi {
	namespace MapEditor {
		public class Remover : MapTool {

			public string[] layerNames;
			
			protected BlockObject selectedBlock { get; set; }

			private Dictionary<Vector3, PositionConvertor> convertor;
			private int raycastLayer;

			private bool isValidLocation;

			private int startY;

			void Awake() {
				SetActive (false);

				// Initialize layer values
				raycastLayer = LayerMask.GetMask(layerNames);
				
				// Initialize coord converting delegators
				convertor = new Dictionary<Vector3, PositionConvertor>();
				convertor [Vector3.forward] = new PositionConvertor (Mathf.Round, Mathf.Round, Mathf.Round);
				convertor [Vector3.back] = new PositionConvertor (Mathf.Round, Mathf.Round, Mathf.Round);
				convertor [Vector3.left] = new PositionConvertor (Mathf.Round, Mathf.Round, Mathf.Round);
				convertor [Vector3.right] = new PositionConvertor (Mathf.Round, Mathf.Round, Mathf.Round);
				convertor [Vector3.up] = new PositionConvertor (Mathf.Round, Mathf.Round, Mathf.Round);
				convertor [Vector3.down] = new PositionConvertor (Mathf.Round, Mathf.Round, Mathf.Round);
			}
			
			void Start() {
				Initialize ();
			}

			// Update is called once per frame
			void Update () {
				if (!mapCamera.pixelRect.Contains (Input.mousePosition)) {
					Tooltip.current.Show (false);
					return;
				}
				
				Preview ();
				
				// Down mouse left button
				if (Input.GetMouseButtonDown (0)) {
					Remove ();
				} else if (Input.GetMouseButton (0)) {
					if (Input.GetKeyDown (KeyCode.LeftControl)) {
						Remove ();
					}
				} else if (Input.GetMouseButtonUp (0)) {
					
				}
			}
			
			public override void Initialize() {
				if (selectedBlock != null)
					selectedBlock.color = Color.white;
				selectedBlock = null;
				isValidLocation = false;
			}

			public void Preview() {
				if (selectedBlock != null && Input.GetAxis ("Mouse X") == 0f && Input.GetAxis ("Mouse Y") == 0f)
					return;

				BlockObject bo;
				isValidLocation = GetBlock (out bo, raycastLayer);

				if (selectedBlock != null && (!isValidLocation || bo != selectedBlock)) {
					selectedBlock.color = Color.white;
					selectedBlock = null;
				}

				if (!isValidLocation)
					return;

				if (bo == selectedBlock)
					return;

				bo.color = transparentGreen;
				selectedBlock = bo;
			}
			
			public void Remove() {
				if (!isValidLocation)
					return;

				if (selectedBlock == null)
					return;

				selectedBlock.color = Color.white;
				mapManager.RemoveBlock(selectedBlock);
				selectedBlock = null;
			}

			private bool GetBlock(out BlockObject bo, int layerMask) {

				RaycastHit hit;		
				bo = null;	
				if (!Raycast (out hit, Mathf.Infinity, layerMask))
					return false;

				bo = hit.collider.GetComponentInParent<BlockObject> ();
				return true;
			}
		}
	}
}

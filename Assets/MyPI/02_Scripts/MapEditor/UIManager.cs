using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Mypi.Block;

namespace Mypi {
	namespace MapEditor {

		public class UIManager : MonoBehaviour {
			#region 1. User defined type
			public delegate void Action ();
			#endregion 1. User defined type

			#region 2. Unity inspector variable
			public MapManager mapManager;
			public ToolManager toolManger;

			public Selectable[] disablesInTopViewMode;
			public Selectable[] disablesInMassBuildMode;

			public GameObject exteriorSelector;
			public GameObject interiorSelector;
			#endregion 2. Unity inspector variable

			#region 3. Private variable
			private Action lastRotate;
			private Action lastSelect;
			#endregion 3. Private variable


			#region 4. Unity function
			void Start () {

				// First!
				lastRotate = RotateLeft90;
				lastSelect = SelectSingleBuildMode;

				SelectFieldMapType (); // 1
				SelectNormalViewMode (); // 2
			}
			#endregion 4. Unity function

			#region 5. System menu bar
			public void New() {
				//var path = EditorUtility.SaveFilePanel ("New", ".", "NewMap", ".dat");
				if (mapManager.IsEdited ()) {
				}
				
				mapManager.ClearMap (false);
			}
			
			public void Open() {
				//var path = EditorUtility.OpenFilePanel ("Open", ".", ".dat");
				//string path = Application.persistentDataPath + "/map.dat";
				string path = "C:/Users/Taekyun/Desktop/map.mypi";
				mapManager.Load (path);
			}
			
			public void Save() {
				//var path = EditorUtility.SaveFilePanel ("Save", ".", "SaveMap", ".dat");
				//string path = Application.persistentDataPath + "/map.dat";
				string path = "C:/Users/Taekyun/Desktop/map.mypi";
				mapManager.Save (path);
			}
			
			public void SaveAs() {
				//var path = EditorUtility.SaveFilePanel ("Save As", ".", "SaveMap", ".dat");
			}

			public void SetOption() {
			}
			
			public void Exit() {
				Application.LoadLevel ("StartScene");
			}
			#endregion 5. System menu bar

			#region 6. Action menu bar
			#region 6-1. Camera menu
			public void MoveUpCamera() {
				mapManager.currentMapData.camera.Move (Vector3.up);
			}

			public void MoveDownCamera() {
				mapManager.currentMapData.camera.Move (Vector3.down);
			}

			public void MoveLeftCamera() {
				mapManager.currentMapData.camera.Move (Vector3.left);
			}

			public void MoveRightCamera() {
				mapManager.currentMapData.camera.Move (Vector3.right);
			}

			public void RotateUpCamera() {
				mapManager.currentMapData.camera.Rotate (0f, -10f);
			}
			
			public void RotateDownCamera() {
				mapManager.currentMapData.camera.Rotate (0f, 10f);
			}
			
			public void RotateLeftCamera() {
				mapManager.currentMapData.camera.Rotate (-10f, 0f);
			}
			
			public void RotateRightCamera() {
				mapManager.currentMapData.camera.Rotate (10f, 0f);
			}

			public void ZoomIn() {
				mapManager.currentMapData.camera.Zoom (-1);
			}

			public void ZoomOut() {
				mapManager.currentMapData.camera.Zoom (1);
			}
			#endregion 6-1. Camera menu

			#region 6-2. View Menu
			public void SelectNormalViewMode() {
				mapManager.currentMapData.camera.SetNormalViewMode ();

				SetInteractable (disablesInTopViewMode, true);
			}

			public void SelectTopViewMode() {
				mapManager.currentMapData.camera.SetTopViewMode ();

				SetInteractable (disablesInTopViewMode, false);
			}

			public void RotateLastAngle() {
				lastRotate ();
			}

			public void RotateLeft90() {
				lastRotate = RotateLeft90;
				mapManager.currentMapData.camera.RotateView (90f);
			}

			public void RotateRight90() {
				lastRotate = RotateRight90;
				mapManager.currentMapData.camera.RotateView (-90f);
			}

			#endregion 6-2. View Menu

			#region 6-3. Build Menu
			public void SelectLastBuildMode() {
				lastSelect ();
			}

			public void SelectSingleBuildMode() {
				lastSelect = SelectSingleBuildMode;
				toolManger.SelectBuildMode (BuildMode.Single);

				SetInteractable (disablesInMassBuildMode, true);
			}

			public void SelectMassBuildMode() {
				lastSelect = SelectMassBuildMode;
				toolManger.SelectBuildMode (BuildMode.Mass);

				SetInteractable (disablesInMassBuildMode, false);
			}

			public void SetInteractable(Selectable[] selectables, bool value) {
				foreach(var s in selectables)
					s.interactable = value;
			}

			public void Clear() {
				mapManager.ClearMap (false);
			}

			public void Remove() {
				toolManger.SelectBuildMode (BuildMode.Remove);
			}

			public void Undo() {
			}

			public void Redo() {
			}
			#endregion 6-3. Build Menu

			#region 6-4. Block Menu
			public void RotateLeftBlock() {
				toolManger.RotateLeft ();
			}

			public void RotateRightBlock() {
				toolManger.RotateRight ();
			}
			#endregion 6-4. Block Menu

			#region 6-5. Map Menu
			public void SelectFieldMapType() {
				mapManager.SelectMap (MapType.Field);
				mapManager.currentMapData.InitializeCamera ();
				exteriorSelector.SetActive (true);
				interiorSelector.SetActive (false);
				SelectBlock (PoolManager.current.firstTerrainBlockName);
			}

			public void SelectHouseMapType() {
				mapManager.SelectMap (MapType.House);
				mapManager.currentMapData.InitializeCamera ();
				exteriorSelector.SetActive (false);
				interiorSelector.SetActive (true);
				SelectBlock (PoolManager.current.firstInteriorBlockName);
			}

			public void SelectHouse1MapType() {
				mapManager.SelectMap (MapType.House1);
				mapManager.currentMapData.InitializeCamera ();
				exteriorSelector.SetActive (false);
				interiorSelector.SetActive (true);
				SelectBlock (PoolManager.current.firstInteriorBlockName);
			}
			#endregion 6-5. Map Menu
			#endregion 6. Action menu bar

			#region 7. Block selector
			public void SelectBlock(string blockName) {
				toolManger.SelectBlock (blockName);
				SelectLastBuildMode ();
			}
			#endregion 7. Block selector
		}
	}
}

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Mypi.Block;

namespace Mypi {
	namespace MapEditor {
		public enum BuildMode{
			Single,
			Mass,
			Remove
		};

		public class ToolManager : MonoBehaviour {
			public MapManager mapManager;
			public Camera mapCamera;

			public SingleBuilder singleBuilder;
			public MassBuilder massBuilder;
			public Remover remover;

			public BuildMode buildMode { get; set; }

			private Dictionary<BuildMode, MapTool> tools;

			void Awake() {
				tools = new Dictionary<BuildMode, MapTool> ();

				tools [BuildMode.Single] = singleBuilder;
				tools [BuildMode.Mass] = massBuilder;
				tools [BuildMode.Remove] = remover;
			}

			// Build action
			public void SelectBuildMode(BuildMode buildMode) {
				this.buildMode = buildMode;

				foreach (var kvp in tools)
					kvp.Value.SetActive(kvp.Key == buildMode);
				tools [buildMode].Initialize ();
			}

			public void UndoBuild() {
			}

			public void RedoBuild() {
			}

			// Block action
			public void RotateLeft() {
				if (buildMode != BuildMode.Single)
					return;

				singleBuilder.RotateLeft ();
			}

			public void RotateRight() {
				if (buildMode != BuildMode.Single)
					return;
				
				singleBuilder.RotateRight ();
			}

			// Block selection
			public void SelectBlock(string blockName) {;
				singleBuilder.SetBlock (blockName);
				massBuilder.SetBlock (blockName);
			}
		}
	}
}
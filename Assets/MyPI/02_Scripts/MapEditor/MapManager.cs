using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using Mypi.Block;
using Mypi.Utils;

namespace Mypi {
	namespace MapEditor {
		public enum MapType {Field, House, House1};

		public class MapManager : MonoBehaviour {

			[Serializable]
			public class MapData {
				public Transform mapTransform;
				public EditorCamera camera;
				public string layerName;
				public MapType mapType;
				public float initX;
				public float initY;
				public float initZ;
				public float initAngleX;
				public float initAngleY;

				public List<BlockObject> blocks { get; set; }
				public int layer { get; set; }
				public BlockData[] datas {
					get {
						BlockData[] d = new BlockData[blocks.Count];
						for (int i = 0; i < d.Length; i++)
							d[i] = blocks[i].blockData;

						return d;
					}
				}

				public MapData() {
					blocks = new List<BlockObject>();
				}

				public void Initialize() {
					layer = LayerMask.NameToLayer (layerName);
				}

				public void SetActive(bool value) {
					mapTransform.gameObject.SetActive (value);
				}

				public void InitializeCamera() {
					camera.SetPosition (new Vector3 (initX, initY, initZ));
					camera.SetAngle (initAngleX, initAngleY);
				}

				public Vector3 ConvertPoint(Vector3 point) {
					return mapTransform.TransformPoint (point);
				}
			}

			private const string MAP_DATA_FILENAME = "map.mypi";

			public MapData fieldMapData;
			public MapData houseMapData;
			public MapData house1MapData;
			public bool edited { get; set; }
			public MapData currentMapData {
				get {
					return _currentMapData;
				}
			}

			private MapData _currentMapData;
			private Dictionary<MapType, MapData> mapDatas;
			private string mapDataFileName;

			private Dictionary<Range, BlockObject> rangeBlocks;
			private Dictionary<IntVector3, BlockObject> singleBlocks;

		
			void Awake() {
				mapDatas = new Dictionary<MapType, MapData> ();
				mapDatas [MapType.Field] = fieldMapData;
				mapDatas [MapType.House] = houseMapData;
				mapDatas [MapType.House1] = house1MapData;

				rangeBlocks = new Dictionary<Range, BlockObject> ();
				singleBlocks = new Dictionary<IntVector3, BlockObject> ();

				rangeBlocks [new Range (0, 0, 0, 2, 2, 2)] = null;

				fieldMapData.Initialize ();
				houseMapData.Initialize ();
				house1MapData.Initialize ();

				edited = false;
			}
			public void BuildBlock(string blockName, IntVector3 baseCoord, int angle) {
				BuildBlock (blockName, baseCoord, angle, currentMapData.mapType);
			}

			public void BuildBlock(string blockName, IntVector3 baseCoord, int angle, MapType mapType) {
				BlockObject newBlock;
				if (!PoolManager.current.AllocateBlock (out newBlock, blockName))
					return;
				if (angle < 0)
					angle = (angle % 360) + 360;
				else
					angle = angle % 360;

				newBlock.layer = mapDatas [mapType].layer;
				newBlock.parent =  mapDatas[mapType].mapTransform; // Must set parent first, before set local variables
				newBlock.coord = baseCoord;
				newBlock.angle = angle;
				mapDatas [mapType].blocks.Add (newBlock);

				Range[] ranges = newBlock.GetRanges ();
				if (ranges != null) {
					foreach(var range in ranges) {
						if (angle == 90)
							range.Rotate90();
						else if (angle == 180)
							range.Rotate180();
						else if (angle == 270)
							range.Rotate270();

						rangeBlocks[range + baseCoord] = newBlock;
					}
				}

				IntVector3[] coords = newBlock.GetCoords ();
				if (coords != null) {
					foreach(var coord in coords) {
						if (angle == 90)
							coord.Rotate90();
						else if (angle == 180)
							coord.Rotate180();
						else if (angle == 270)
							coord.Rotate270();

						singleBlocks[coord + baseCoord] = newBlock;
					}
				}

				edited = true;
			}
			
			public void BuildBlock(string blockName, IntVector3 start, IntVector3 end) {
				BuildBlock (blockName, start, end, currentMapData.mapType);
			}

			public void BuildBlock(string blockName, IntVector3 start, IntVector3 end, MapType mapType) {
				int sx = Mathf.Min (start.x, end.x);
				int sy = Mathf.Min (start.y, end.y);
				int sz = Mathf.Min (start.z, end.z);
				int ex = sx + Mathf.Abs (start.x - end.x);
				int ey = sy + Mathf.Abs (start.y - end.y);
				int ez = sz + Mathf.Abs (start.z - end.z);
				IntVector3 idx = new IntVector3();
				for (idx.x = sx; idx.x <= ex; idx.x++) {
					for (idx.y = sy; idx.y <= ey; idx.y++) {
						for (idx.z = sz; idx.z <= ez; idx.z++) {
							if (rangeBlocks.ContainsKey(idx.ToRange())) {
								continue;
							}
							if (singleBlocks.ContainsKey(idx)) {
								continue;
							}

							BuildBlock(blockName, idx, 0, mapType);
						}
					}
				}
			}

			public void RemoveBlock(BlockObject bo) {
				RemoveBlock (bo, currentMapData.mapType);
			}

			public void RemoveBlock(BlockObject bo, MapType mapType) {

				mapDatas[mapType].blocks.Remove (bo);
				PoolManager.current.ReleaseBlock (bo);

				edited = true;
			}

			public void ClearMap(bool edited) {
//				if (edited && fieldBlocks.Count > 0)
//					this.edited = true;
//				else
//					this.edited = false;

				foreach (var kvp in mapDatas) {
					foreach (var v in kvp.Value.blocks) {
						PoolManager.current.ReleaseBlock (v);
					}
					kvp.Value.blocks.Clear();
				}
			}

			public bool IsEdited() {
				return edited;
			}

			public void Save(string path) {
//				BlockData[] datas = new BlockData[fieldBlocks.Count];
//				int i = 0;
//				foreach (BlockObject block in fieldBlocks) {
//					datas[i++] = block.blockData;
//					}
//				var bf = new BinaryFormatter ();
//				var fs = File.Create (path);
//				bf.Serialize (fs, datas);

				var bf = new BinaryFormatter ();
				var fs = File.Create (path);
				bf.Serialize (fs, mapDatas[MapType.Field].datas);
				bf.Serialize (fs, mapDatas[MapType.House].datas);
				bf.Serialize (fs, mapDatas[MapType.House1].datas);
				fs.Close ();

				edited = false;
			}

			public void Load(string path) {
//				var bf = new BinaryFormatter ();
//				var fs = File.Open (path, FileMode.Open);
//				//mapData = (Dictionary<Coord, BlockData>)bf.Deserialize (fs);
//				BlockData[] datas = (BlockData[])bf.Deserialize (fs);

				var bf = new BinaryFormatter ();
				var fs = File.Open (path, FileMode.Open);
				BlockData[] fieldDatas = (BlockData[])bf.Deserialize (fs);
				BlockData[] houseDatas = (BlockData[])bf.Deserialize (fs);
				BlockData[] house1Datas = (BlockData[])bf.Deserialize (fs);
				fs.Close ();

				edited = false;
				ClearMap (false);
				foreach (BlockData data in fieldDatas) {
					if (!PoolManager.current.GetBlockNames().Contains(data.name)){
						Debug.Log (data.name);
						continue;
					}

					BuildBlock (data.name, data.coord, data.angle, MapType.Field);
				}
				foreach (BlockData data in houseDatas)
					BuildBlock(data.name, data.coord, data.angle, MapType.House);
				foreach (BlockData data in house1Datas)
					BuildBlock(data.name, data.coord, data.angle, MapType.House1);
			}

			public void SelectMap(MapType mapType) {
				foreach (var kvp in mapDatas) {
					kvp.Value.SetActive (mapType == kvp.Key);
				}
				_currentMapData = mapDatas [mapType];
			}

			public bool Contains(IntVector3 baseCoord, Range[] ranges, IntVector3[] coords) {
//				foreach (var v in rangeBlocks) {
//					DebugRange(v.Key, Color.green);
//				}


				if (ranges != null) {
					foreach (var range in ranges) {
						Range newRange = range + baseCoord;
//						Debug.Log (baseCoord);
//						Debug.Log (newRange);
//						DebugRange (newRange, Color.red);

						foreach (var r in rangeBlocks) {
							if (newRange.IsOverLap(r.Key))
								return true;
						}

						foreach (var v in singleBlocks) {
							if (newRange.Contains(v.Key)) {

								return true;
							}
						}
					}
				}
				if (coords != null) {
					foreach (var coord in coords) {
						IntVector3 newCoord = coord + baseCoord;
						if (singleBlocks.ContainsKey (newCoord))
							return true;

						if (rangeBlocks.ContainsKey(newCoord.ToRange()))
							return true;
					}
				}
				return false;
			}

			public bool Contains(IntVector3 start, IntVector3 end) {
				Range range = new Range (start, end);
				if (rangeBlocks.ContainsKey (range)) {
					return true;
				}

				IntVector3 s = range.start;
				IntVector3 e = range.end;
				for (int i = s.x; i <= e.x; i++) {
					for (int j = s.y; j <= e.y; j++) {
						for (int k = s.z; k <= e.z; k++) {
							if (singleBlocks.ContainsKey(new IntVector3(i, j, k))) {
								return true;
							}
						}
					}
				}
				return false;
			}

			public bool Contains(IntVector3 coord) {
				if (rangeBlocks.ContainsKey(coord.ToRange()))
					return true;

				return singleBlocks.ContainsKey (coord);
			}

			public void DebugRangeBlocks() {
				string str = "";
				foreach (var v in rangeBlocks) {
					str += "[" + v.Key + "," + v.Value;
				}
				Debug.Log(str);
			}

			public void DebugSingleBlocks() {
				string str = "";
				foreach (var v in singleBlocks) {
					str += "[" + v.Key + "," + v.Value;
				}
				Debug.Log(str);
			}

			public void DebugRange(Range range, Color color) {
				Debug.DrawLine(new Vector3(range.start.x, range.start.y, range.start.z), new Vector3(range.end.x, range.start.y, range.start.z), color);
				Debug.DrawLine(new Vector3(range.start.x, range.end.y, range.start.z), new Vector3(range.end.x, range.end.y, range.start.z), color);
				Debug.DrawLine(new Vector3(range.start.x, range.end.y, range.end.z), new Vector3(range.end.x, range.end.y, range.end.z), color);
				Debug.DrawLine(new Vector3(range.start.x, range.start.y, range.end.z), new Vector3(range.end.x, range.start.y, range.end.z), color);
				
				Debug.DrawLine(new Vector3(range.start.x, range.start.y, range.start.z), new Vector3(range.start.x, range.end.y, range.start.z), color);
				Debug.DrawLine(new Vector3(range.start.x, range.start.y, range.end.z), new Vector3(range.start.x, range.end.y, range.end.z), color);
				Debug.DrawLine(new Vector3(range.end.x, range.start.y, range.end.z), new Vector3(range.end.x, range.end.y, range.end.z), color);
				Debug.DrawLine(new Vector3(range.end.x, range.start.y, range.start.z), new Vector3(range.end.x, range.end.y, range.start.z), color);
				
				Debug.DrawLine(new Vector3(range.start.x, range.start.y, range.start.z), new Vector3(range.start.x, range.start.y, range.end.z), color);
				Debug.DrawLine(new Vector3(range.start.x, range.end.y, range.start.z), new Vector3(range.start.x, range.end.y, range.end.z), color);
				Debug.DrawLine(new Vector3(range.end.x, range.end.y, range.start.z), new Vector3(range.end.x, range.end.y, range.end.z), color);
				Debug.DrawLine(new Vector3(range.end.x, range.start.y, range.start.z), new Vector3(range.end.x, range.start.y, range.end.z), color);
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Mypi.Block;

namespace Mypi {
	public class PlayMapManager : MonoBehaviour {
		public string layerName;

		GameObject g;


		private string mapDataFileName;
		private int layer;

		void Awake() {
			layer = LayerMask.NameToLayer (layerName);
		}


		// Use this for initialization
		void Start () {
			string path = "C:/Users/Taekyun/Desktop/map.mypi";
			Load (path);
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public BlockObject BuildBlock(string blockName, Transform parent, IntVector3 coord, int angle) {
			BlockObject newBlock;
			if (!PoolManager.current.AllocateBlock (out newBlock, blockName))
				return null;

			newBlock.layer = layer;
			newBlock.parent = parent; // Must set parent first, before set local variables
			newBlock.coord = coord;
			newBlock.angle = angle;

			return newBlock;
		}

		public void RemoveBlock(BlockObject bo) {
			PoolManager.current.ReleaseBlock (bo);
		}

		public void Load(string path) {
			var bf = new BinaryFormatter ();
			var fs = File.Open (path, FileMode.Open);
			BlockData[] fieldDatas = (BlockData[])bf.Deserialize (fs);
			BlockData[] houseDatas = (BlockData[])bf.Deserialize (fs);
			BlockData[] house1Datas = (BlockData[])bf.Deserialize (fs);
			fs.Close ();

			foreach (BlockData data in fieldDatas) {
				BlockObject block = BuildBlock (data.name, transform, data.coord, data.angle);

				if (data.name == "Simple House" && houseDatas != null) {
					foreach (BlockData data2 in houseDatas) {
						BlockObject block2 = BuildBlock(data2.name, block.transform, data2.coord, data2.angle);
						block2.parent = transform;
					}
				} else if (data.name == "Premium House" && house1Datas != null) {
					foreach (BlockData data2 in house1Datas) {
						BlockObject block2 = BuildBlock(data2.name, block.transform, data2.coord, data2.angle);
						block2.parent = transform;
					}
				}
			}


//			foreach (BlockData data in house1Datas)
//				BuildBlock(data.name, data.coord, data.angle, MapType.House1);
//			foreach (BlockData data in house2Datas)
//				BuildBlock(data.name, data.coord, data.angle, MapType.House2);
		}
	}
}
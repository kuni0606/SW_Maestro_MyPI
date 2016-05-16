using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Mypi.Block;

namespace Mypi {
	public sealed class PoolManager : MonoBehaviour {
		public static PoolManager current;
		private static bool exist = false;
	
		[Serializable]
		public struct BlockInfo {
			public int poolInitSize;
			public GameObject prefab;
			public Sprite thumbnail;
		}

		[Serializable]
		public struct Attribute {
			public string name;
			public Mesh mesh;
			public Material material;
			public Sprite thumbnail;
		}

		[Serializable]
		public struct TerrainPoolData {
			public int initSize;
			public GameObject prefab;
			public Attribute[] attributes;
		}

		public bool shadowOff;
		public BlockInfo[] blockInfos;
		public TerrainPoolData terrainPoolData;

		public string firstTerrainBlockName {
			get {
				return blockNames[BlockCategory.Terrain][0];
			}
		}
		public string firstInteriorBlockName {
			get {
				return blockNames[BlockCategory.Interior][0];
			}
		}


		private Dictionary<string, ObjectPool> objectPools;
		private ObjectPool terrainPool;

		private Dictionary<BlockCategory, List<string>> blockNames;
		private Dictionary<string, Sprite> thumbnails;
		private List<string> allBlockNames;

		private Dictionary<string, BlockCategory> categoryMap;
		private Dictionary<string, Attribute> terrainAttributeMap;


		void Awake() {
			if (exist) {
				Destroy (gameObject);
			}
			else {
				exist = true;
				Initialize();
				DontDestroyOnLoad (gameObject);
			}
		}

		void Initialize() {
			current = this;

			terrainPool = new ObjectPool (transform, terrainPoolData.prefab, terrainPoolData.initSize, false);
			objectPools = new Dictionary<string, ObjectPool> ();
			allBlockNames = new List<string> ();
			blockNames = new Dictionary<BlockCategory, List<string>> ();
			categoryMap = new Dictionary<string, BlockCategory> ();
			terrainAttributeMap = new Dictionary<string, Attribute> ();
			thumbnails = new Dictionary<string, Sprite> ();

			foreach (BlockCategory category in Enum.GetValues (typeof(BlockCategory)))
				blockNames[category] = new List<string>();

			// Terrain pool
			foreach (var v in terrainPoolData.attributes) {
				categoryMap[v.name] = BlockCategory.Terrain;
				terrainAttributeMap[v.name] = v;
				objectPools[v.name] = terrainPool;
				allBlockNames.Add(v.name);
				blockNames[BlockCategory.Terrain].Add(v.name);
				thumbnails[v.name] = v.thumbnail;
			}
			// Other pool
			foreach (var v in blockInfos) {
				BlockObject b = v.prefab.GetComponent<BlockObject>();
				objectPools[b.blockName] = new ObjectPool(transform, v.prefab, v.poolInitSize, false);
				categoryMap[b.blockName] = b.blockCategory;
				allBlockNames.Add(b.blockName);
				blockNames[b.blockCategory].Add(b.blockName);
				thumbnails[b.blockName] = v.thumbnail;
			}

			allBlockNames.Sort ();
			foreach (BlockCategory category in Enum.GetValues (typeof(BlockCategory)))
				blockNames [category].Sort ();
		}

		public List<string> GetBlockNames() {
			return new List<string> (allBlockNames);
		}

		public List<string> GetBlockNames(BlockCategory blockCategory) {
			return new List<string> (blockNames[blockCategory]);
		}

		public Sprite GetThumbnail(string blockName) {
			return thumbnails [blockName];
		}

		public bool AllocateBlock(out BlockObject blockObject, string blockName) {

			if (!objectPools[blockName].Allocate(out blockObject)) {
				Debug.Log (blockName + " pool is empty");
				return false;
			}

			if (categoryMap [blockName] == BlockCategory.Terrain) {
				TerrainBlock b = blockObject as TerrainBlock;
				Attribute a = terrainAttributeMap[blockName];
				b.SetAttribute(a.name, a.mesh, a.material);
			}

			return true;
		}

		public void ReleaseBlock(BlockObject blockObject) {
			//Debug.Log (blockObject.blockName);
			objectPools [blockObject.blockName].Release (blockObject);
		}
	}
}

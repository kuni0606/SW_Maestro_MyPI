using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mypi.Block;

namespace Mypi {
	public class ObjectPool {

		private Stack<BlockObject> pool;

		private string _blockName;
		public string blockName {
			get {
				return _blockName;
			}
		}

		private BlockCategory _blockCategory;
		public BlockCategory blockCategory {
			get {
				return _blockCategory;
			}
		}

		public ObjectPool(Transform poolTransform, GameObject prefab, int poolSize, bool shadow) {
			BlockObject b = prefab.GetComponent<BlockObject> ();
			_blockName = b.blockName;
			_blockCategory = b.blockCategory;

			pool = new Stack<BlockObject> ();

			for (int i = 0; i < poolSize; i++) {
				BlockObject bo = GameObject.Instantiate<GameObject> (prefab).GetComponentInChildren<BlockObject>();
				BlockObject.SetShadowCastRecursively(bo.transform, shadow);
				bo.parent = poolTransform;
				bo.SetActive(false);
				pool.Push (bo);
			}
		}

		public bool Allocate(out BlockObject blockObject) {
			if (pool.Count <= 0) {
				blockObject = null;
				return false;
			}
			
			blockObject = pool.Pop ();
			blockObject.SetActive (true);
			return true;
		}
		
		public void Release(BlockObject blockObject) {
			blockObject.SetActive (false);
			pool.Push (blockObject);
		}
	}
}

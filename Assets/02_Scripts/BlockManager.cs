using UnityEngine;
using System.Collections;

public class BlockManager : MonoBehaviour {

	const int POOL_SIZE = 400;
	const string BLOCK_OBJECT_NAME = "blockObject";

	public GameObject blockObject;
	public Transform poolObject;
	public MapManager mapManager;

	private Stack blockPool;

	public void Initialize() {
		blockPool = new Stack (POOL_SIZE);

		GameObject go;
		for (int i = 0; i < POOL_SIZE; i++) {
			go = GameObject.Instantiate(blockObject);
			go.transform.SetParent(poolObject);
			go.name = BLOCK_OBJECT_NAME;
			blockPool.Push(go);
		}
	}

	bool AllocateBlock(out GameObject bo) {
		if (blockPool.Count == 0) {
			bo = null;
			return false;
		}

		bo = blockPool.Pop () as GameObject;
		return true;
	}

	void ReleaseBlock(GameObject bo) {

		bo.transform.SetParent (poolObject);
		blockPool.Push (bo);
	}

	public void CreateBlock(Block block, Vector3 pos) {
		GameObject b;
		if (AllocateBlock (out b)) {
			mapManager.BuildToMap (b);
		} else {
			// fail
		}
	}

	public void RemoveBlock() {
	}

	// Use this for initialization
	void Start () {
		Initialize ();
		// CreateBlock(null);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}

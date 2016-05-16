using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MapManager : MonoBehaviour {

	private const string MAP_DATA_FILENAME = "map.dat";

	public Transform mapObject;

	private Dictionary<Vector3, Block> mapData;

	private List<Block> mapList;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BuildToMap(GameObject bo) {
		bo.transform.SetParent (mapObject);
	}

	void Save() {
		var bf = new BinaryFormatter ();
		var fs = File.Create (Application.persistentDataPath + MAP_DATA_FILENAME);
		bf.Serialize (fs, mapData);
		fs.Close ();
	}

	void Load() {
	}
}

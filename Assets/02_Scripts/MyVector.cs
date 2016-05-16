using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

[Serializable]
public class MyVector : ISerializable {

	private const string X_VALUE = "x";
	private const string Y_VALUE = "y";
	private const string Z_VALUE = "z";

	private Vector3 vector;

	public MyVector() {
		vector = Vector3.zero;
	}

	public MyVector(float x, float y, float z) {
		vector = new Vector3 (x, y, z);
	}

	public MyVector(Vector3 newVector) {
		vector = newVector;
	}

	public MyVector(SerializationInfo info, StreamingContext context) {
		vector = new Vector3 ((float)info.GetDouble (X_VALUE), (float)info.GetDouble (Y_VALUE), (float)info.GetDouble (Z_VALUE));
	}

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue (X_VALUE, (double)vector.x, typeof(double));
		info.AddValue (Y_VALUE, (double)vector.y, typeof(double));
		info.AddValue (Z_VALUE, (double)vector.z, typeof(double));
	}

}

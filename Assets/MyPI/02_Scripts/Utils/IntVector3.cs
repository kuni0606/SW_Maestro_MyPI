using UnityEngine;
using System;
using System.Runtime.Serialization;
using Mypi.Utils;

namespace Mypi {
	[Serializable]
	public struct IntVector3 : ISerializable {
		private const string X_VALUE = "x";
		private const string Y_VALUE = "y";
		private const string Z_VALUE = "z";
		
		public int x;
		public int y;
		public int z;

		public static IntVector3 forward {
			get {
				return new IntVector3 (0, 0, 1);
			}
		}

		public static IntVector3 back {
			get {
				return new IntVector3 (0, 0, -1);
			}
		}

		public static IntVector3 up {
			get {
				return new IntVector3 (0, 1, 0);
			}
		}

		public static IntVector3 down {
			get {
				return new IntVector3 (0, -1, 0);
			}
		}

		public static IntVector3 left {
			get {
				return new IntVector3 (-1, 0, 0);
			}
		}

		public static IntVector3 right {
			get {
				return new IntVector3 (1, 0, 0);
			}
		}

		public static IntVector3 zero {
			get {
				return new IntVector3 (0, 0, 0);
			}
		}

		public static IntVector3 one {
			get {
				return new IntVector3 (1, 1, 1);
			}
		}

		public IntVector3(IntVector3 other) {
			x = other.x;
			y = other.y;
			z = other.z;
		}

		public IntVector3(int x, int y, int z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public IntVector3(Vector3 v) {
			x = Mathf.RoundToInt (v.x);
			y = Mathf.RoundToInt (v.y);
			z = Mathf.RoundToInt (v.z);
		}
		
		public IntVector3(SerializationInfo info, StreamingContext context) {
			x = info.GetInt32 (X_VALUE);
			y = info.GetInt32 (Y_VALUE);
			z = info.GetInt32 (Z_VALUE);
		}

		public void Set(int x, int y, int z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public void Set(Vector3 v) {
			x = Mathf.RoundToInt (v.x);
			y = Mathf.RoundToInt (v.y);
			z = Mathf.RoundToInt (v.z);
		}

		public override bool Equals (object other) {
			if (other is Range) {
				Range range = (Range)other;
				return range.Contains(this);
			} else if (other is IntVector3) {
				IntVector3 coord = (IntVector3)other;
				return this.x.Equals (coord.x) && this.y.Equals (coord.y) && this.z.Equals (coord.z);
			}
			return false;
		}

		public void Rotate90() {
			int newX = z;
			int newZ = -x;
			x = newX;
			z = newZ;
		}

		public void Rotate180() {
			int newX = -x;
			int newZ = -z;
			x = newX;
			z = newZ;
		}

		public void Rotate270() {
			int newX = -z;
			int newZ = x;
			x = newX;
			z = newZ;
		}

		public Vector3 ToVector3() {
			return new Vector3 (x, y, z);
		}

		public Range ToRange() {
			return new Range (this, this);
		}

		public override string ToString ()
		{
			return "(" + x + "," + y + "," + z + ")";
		}

		public static bool operator == (IntVector3 lhs, IntVector3 rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
		}

		public static bool operator != (IntVector3 lhs, IntVector3 rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
		}

		public static IntVector3 operator + (IntVector3 a, IntVector3 b)
		{
			return new IntVector3 (a.x + b.x, a.y + b.y, a.z + b.z);
		}
		
		public static IntVector3 operator - (IntVector3 a, IntVector3 b)
		{
			return new IntVector3 (a.x - b.x, a.y - b.y, a.z - b.z);
		}
		
		public static IntVector3 operator - (IntVector3 a)
		{
			return new IntVector3 (-a.x, -a.y, -a.z);
		}

		public static IntVector3 operator * (int d, IntVector3 a)
		{
			return new IntVector3 (a.x * d, a.y * d, a.z * d);
		}

		public static IntVector3 operator * (IntVector3 a, int d)
		{
			return new IntVector3 (a.x * d, a.y * d, a.z * d);
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue (X_VALUE, x, typeof(int));
			info.AddValue (Y_VALUE, y, typeof(int));
			info.AddValue (Z_VALUE, z, typeof(int));
		}
	}
}
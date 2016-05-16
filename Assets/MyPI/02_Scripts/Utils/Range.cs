using UnityEngine;
using System.Collections;

namespace Mypi.Utils {

	public struct Range {
		private int sx;
		private int sy;
		private int sz;
		private int ex;
		private int ey;
		private int ez;

		public IntVector3 start {
			get {
				return new IntVector3 (sx, sy, sz);
			}
			set {
				sx = value.x;
				sy = value.y;
				sz = value.z;
				Normalize();
			}
		}
		public IntVector3 end {
			get {
				return new IntVector3 (ex, ey, ez);
			}
			set {
				ex = value.x;
				ey = value.y;
				ez = value.z;
				Normalize();
			}
		}

		public Range(IntVector3 start, IntVector3 end) {
			sx = start.x;
			sy = start.y;
			sz = start.z;
			ex = end.x;
			ey = end.y;
			ez = end.z;
			Normalize ();
		}

		public Range (int sx, int sy, int sz, int ex, int ey, int ez) {
			this.sx = sx;
			this.sy = sy;
			this.sz = sz;
			this.ex = ex;
			this.ey = ey;
			this.ez = ez;
			Normalize ();
		}

		public void Normalize() {
			int temp;
			if (sx > ex) {
				temp = sx;
				sx = ex;
				ex = temp;
			} 
			if (sy > ey) {
				temp = sy;
				sy = ey;
				ey = temp;
			} 
			if (sz > ez) {
				temp = sz;
				sz = ez;
				ez = temp;
			}
		}

		public void Rotate90() {
			int newSX = sz, newSZ = -sx;
			int newEX = ez, newEZ = -ex;
			sx = newSX;
			sz = newSZ;
			ex = newEX;
			ez = newEZ;
			Normalize ();
		}

		public void Rotate180() {
			sx = -sx;
			sz = -sz;
			ex = -ex;
			ez = -ez;
			Normalize ();
		}

		public void Rotate270() {
			int newSX = -sz, newSZ = sx;
			int newEX = -ez, newEZ = ex;
			sx = newSX;
			sz = newSZ;
			ex = newEX;
			ez = newEZ;
			Normalize ();
		}

		public override string ToString () {
			return "[" + start + ", " + end + "]";
		}

		public override bool Equals (object other)
		{
			if (other is IntVector3) {
				return Contains((IntVector3)other);
			} else if (other is Range) {
				Range range = (Range)other;
				if (ex < range.sx || range.ex < sx)
					return false;
				if (ey < range.sy || range.ey < sy)
					return false;
				return ez >= range.sz && range.ez >= sz;
			}
			return false;
		}

		public bool Contains(IntVector3 coord) {
			if (coord.x < sx || coord.x > ex)
				return false;
			if (coord.y < sy || coord.y > ey)
				return false;
			return coord.z >= sz && coord.z <= ez;
		}

		public bool IsOverLap(Range range) {
			if (ex < range.sx || range.ex < sx)
				return false;
			if (ey < range.sy || range.ey < sy)
				return false;
			return ez >= range.sz && range.ez >= sz;
		}

		public static bool operator == (Range lhs, Range rhs)	{
			if (lhs.ex < rhs.sx || rhs.ex < lhs.sx)
				return false;
			if (lhs.ey < rhs.sy || rhs.ey < lhs.sy)
				return false;
			return lhs.ez >= rhs.sz && rhs.ez >= lhs.sz;
		}
		
		public static bool operator != (Range lhs, Range rhs)	{
			if (lhs.ex < rhs.sx || rhs.ex < lhs.sx)
				return true;
			if (lhs.ey < rhs.sy || rhs.ey < lhs.sy)
				return true;
			return lhs.ez < rhs.sz || rhs.ez < lhs.sz;
		}

		public static Range operator + (IntVector3 v, Range a) {
			return new Range(a.start + v, a.end + v);
		}

		public static Range operator + (Range a, IntVector3 v) {
			return new Range(a.start + v, a.end + v);
		}
	}
}
using UnityEngine;
using System.Collections;
using Mypi.Utils;

namespace Mypi.Block {
	public class BedBlock : InteriorBlock {
		public override Range[] GetRanges () {
			return new Range[]{
				new Range(0, 0, 0, 2, 1, 3),
				new Range(0, 1, 0, 2, 1, 0)
			};
		}
	}
}

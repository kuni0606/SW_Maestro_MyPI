using UnityEngine;
using System.Collections;
using Mypi.Utils;

namespace Mypi.Block {
	public class BushBlock : StructureBlock {
		public override Range[] GetRanges () {
			return new Range[]{
				new Range(0, 0, 0, 1, 1, 1)
			};
		}
	}
}

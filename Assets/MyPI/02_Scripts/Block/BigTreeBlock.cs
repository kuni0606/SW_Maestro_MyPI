using UnityEngine;
using System.Collections;
using Mypi.Utils;

namespace Mypi.Block {
	public class BigTreeBlock : StructureBlock {
		public override Range[] GetRanges () {
			return new Range[]{
				new Range(0, 0, 0, 0, 5, 0)
			};
		}
	}
}
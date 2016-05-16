using UnityEngine;
using Mypi.Utils;

namespace Mypi.Block {
	public class PremiumHouseBlock : StructureBlock {
		public override Range[] GetRanges () {
			return new Range[]{
				new Range(0, 0, 0, 3, 2, 4),
				new Range(-2, 0, 5, 15, 8, 16)
			};
		}
	}
}

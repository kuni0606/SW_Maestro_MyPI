using UnityEngine;
using System.Collections;
using Mypi.Utils;

namespace Mypi.Block {
	public class HangGliderTowerBlock : StructureBlock {
		public override Range[] GetRanges ()
		{
			return new Range[]{
				new Range(0, 0, 0, 2, 49, 1),
				new Range(-2, 0, 2, 4, 0, 8),
				new Range(-2, 49, 2, 4, 49, 8),
				new Range(0, 1, 4, 2, 1, 6),
				new Range(0, 1, 4, 2, 48, 6)
			};
		}
	}
}

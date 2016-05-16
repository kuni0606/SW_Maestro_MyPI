using UnityEngine;
using System.Collections;
using Mypi.Utils;

namespace Mypi.Block {
	public class DockBlock : StructureBlock {
		public override Range[] GetRanges ()
		{
			return new Range[]{
				new Range(0, 0, 0, 19, 2, 5),
				new Range(14, 0, 6, 19, 2, 10),
				new Range(14, 0, 11, 19, 1, 14),
			};
		}
	}
}
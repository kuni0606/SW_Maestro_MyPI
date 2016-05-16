using UnityEngine;
using System.Collections;

namespace Mypi.Block {
	public class LPPlayerBlock : InteriorBlock {
		public override IntVector3[] GetCoords () {
			return new IntVector3[] { IntVector3.zero };
		}
	}
}
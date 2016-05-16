using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mypi.Primitive {
	[ExecuteInEditMode]
	public class Cube : Primitive {

		public float baseX;
		public float baseY;
		public float baseZ;

		protected override void Initialize() {
			SetBackFace (baseX, baseY);
			SetForwardFace (baseX, baseY);
			SetLeftFace (baseZ, baseY);
			SetRightFace (baseZ, baseY);
			SetDownFace (baseX, baseZ);
			SetUpFace (baseX, baseZ);
		}
	}
}

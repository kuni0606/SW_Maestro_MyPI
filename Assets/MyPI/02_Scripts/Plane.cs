using UnityEngine;
using System.Collections;

namespace Mypi.Primitive {
	[ExecuteInEditMode]
	public class Plane : Primitive {

		public float baseX;
		public float baseZ;

		protected override void Initialize ()
		{
			SetUpFace (baseX, baseZ);
		}
	}
}
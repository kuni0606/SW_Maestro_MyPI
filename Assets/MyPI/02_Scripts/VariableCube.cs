using UnityEngine;
using System.Collections;

namespace Mypi.Primitive {
	[ExecuteInEditMode]
	public class VariableCube : Primitive {

		public bool forward;
		public bool back;
		public bool left;
		public bool right;
		public bool up;
		public bool down;

		public float textureWidth;
		public float textureLength;
		public float textureHeight;

		protected override void Initialize ()
		{
			if (forward)
				SetForwardFace (textureWidth, textureHeight);
			if (back)
				SetBackFace (textureWidth, textureHeight);
			if (left)
				SetLeftFace (textureLength, textureHeight);
			if (right)
				SetRightFace (textureLength, textureHeight);
			if (up)
				SetUpFace (textureWidth, textureLength);
			if (down)
				SetDownFace (textureWidth, textureLength);
		}
	}
}
using UnityEngine;
using System.Collections;

namespace Mypi.Block {
	public class TerrainBlock : ExteriorBlock {
		public MeshFilter meshFilter;
		public MeshRenderer meshRenderer;

		public Mesh mesh {
			get {
				return meshFilter.sharedMesh;
			}
		}

		public Material material {
			get {
				return meshRenderer.sharedMaterial;
			}
		}

		public void SetAttribute(string blockName, Mesh mesh, Material material) {
			this.blockName = blockName;
			meshFilter.mesh = mesh;
			meshRenderer.material = material;
		}

		public override IntVector3[] GetCoords () {
			return new IntVector3[] { IntVector3.zero };
		}
	}
}

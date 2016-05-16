using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mypi.Primitive {
	public abstract class Primitive : MonoBehaviour {
		MeshFilter meshFilter;

		List<Vector3> vertices;
		List<int> triangles;
		List<Vector2> uvs;
		List<Vector3> normals;
		List<Vector4> tangents;

		Vector3 point;
		Vector3 delta;

		void Awake() {
			meshFilter = GetComponent<MeshFilter> ();
			if (meshFilter == null)
				return;

			vertices = new List<Vector3> ();
			triangles = new List<int> ();
			uvs = new List<Vector2> ();
			normals = new List<Vector3> ();
			tangents = new List<Vector4> ();

			point = transform.localPosition + 0.5f * Vector3.one;
			delta = transform.localScale * 0.5f;

			Initialize ();

			Mesh mesh = new Mesh ();
			
			mesh.vertices = vertices.ToArray();
			mesh.triangles = triangles.ToArray();
			mesh.uv = uvs.ToArray();
			mesh.normals = normals.ToArray();
			mesh.tangents = tangents.ToArray();
			
			mesh.Optimize ();

			meshFilter.mesh = mesh;
		}

		protected abstract void Initialize ();


		void AddTriangles(int vertexNum) {
			triangles.Add (vertexNum);
			triangles.Add (vertexNum + 1);
			triangles.Add (vertexNum + 2);
			triangles.Add (vertexNum + 3);
			triangles.Add (vertexNum + 1);
			triangles.Add (vertexNum + 0);
		}

		protected void SetLeftFace(float width, float height) {
			AddTriangles (vertices.Count);
			
			vertices.Add (new Vector3 (-0.5f, -0.5f, 0.5f));
			vertices.Add (new Vector3 (-0.5f, 0.5f, -0.5f));
			vertices.Add (new Vector3 (-0.5f, -0.5f, -0.5f));
			vertices.Add (new Vector3 (-0.5f, 0.5f, 0.5f));

			uvs.Add (new Vector2 ((point.z + delta.z) / width, (point.y - delta.y) / height));
			uvs.Add (new Vector2 ((point.z - delta.z) / width, (point.y + delta.y) / height));
			uvs.Add (new Vector2 ((point.z - delta.z) / width, (point.y - delta.y) / height));
			uvs.Add (new Vector2 ((point.z + delta.z) / width, (point.y + delta.y) / height));
			
			for (int i = 0; i < 4; i++) {
				normals.Add (Vector3.left);
				tangents.Add (new Vector4 (1f, 0f, 0f, -1f));
			}
		}
		
		protected void SetRightFace(float width, float height) {
			AddTriangles (vertices.Count);

			vertices.Add (new Vector3 (0.5f, -0.5f, -0.5f));
			vertices.Add (new Vector3 (0.5f, 0.5f, 0.5f));
			vertices.Add (new Vector3 (0.5f, -0.5f, 0.5f));
			vertices.Add (new Vector3 (0.5f, 0.5f, -0.5f));

			uvs.Add (new Vector2 ((point.z - delta.z) / width, (point.y - delta.y) / height));
			uvs.Add (new Vector2 ((point.z + delta.z) / width, (point.y + delta.y) / height));
			uvs.Add (new Vector2 ((point.z + delta.z) / width, (point.y - delta.y) / height));
			uvs.Add (new Vector2 ((point.z - delta.z) / width, (point.y + delta.y) / height));
			
			for (int i = 0; i < 4; i++) {
				normals.Add (Vector3.right);
				tangents.Add (new Vector4 (-1f, 0f, 0f, -1f));
			}
		}
		
		protected void SetForwardFace(float width, float height) {
			AddTriangles (vertices.Count);
			
			vertices.Add (new Vector3 (0.5f, -0.5f, 0.5f));
			vertices.Add (new Vector3 (-0.5f, 0.5f, 0.5f));
			vertices.Add (new Vector3 (-0.5f, -0.5f, 0.5f));
			vertices.Add (new Vector3 (0.5f, 0.5f, 0.5f));

			uvs.Add (new Vector2 ((point.x + delta.x) / width, (point.y - delta.y) / height));
			uvs.Add (new Vector2 ((point.x - delta.x) / width, (point.y + delta.y) / height));
			uvs.Add (new Vector2 ((point.x + delta.x) / width, (point.y - delta.y) / height));
			uvs.Add (new Vector2 ((point.x - delta.x) / width, (point.y + delta.y) / height));
			
			for (int i = 0; i < 4; i++) {
				normals.Add (Vector3.forward);
				tangents.Add (new Vector4 (0f, 0f, -1f, -1f));
			}
		}
		
		protected void SetBackFace(float width, float height) {
			AddTriangles (vertices.Count);
			
			vertices.Add (new Vector3 (-0.5f, -0.5f, -0.5f));
			vertices.Add (new Vector3 (0.5f, 0.5f, -0.5f));
			vertices.Add (new Vector3 (0.5f, -0.5f, -0.5f));
			vertices.Add (new Vector3 (-0.5f, 0.5f, -0.5f));

			uvs.Add (new Vector2 ((point.x - delta.x) / width, (point.y - delta.y) / height));
			uvs.Add (new Vector2 ((point.x + delta.x) / width, (point.y + delta.y) / height));
			uvs.Add (new Vector2 ((point.x + delta.x) / width, (point.y - delta.y) / height));
			uvs.Add (new Vector2 ((point.x - delta.x) / width, (point.y + delta.y) / height));
			
			for (int i = 0; i < 4; i++) {
				normals.Add (Vector3.back);
				tangents.Add (new Vector4 (0f, 0f, 1f, -1f));
			}
		}
		
		protected void SetUpFace(float width, float height) {
			AddTriangles (vertices.Count);

			vertices.Add (new Vector3 (-0.5f, 0.5f, -0.5f));
			vertices.Add (new Vector3 (0.5f, 0.5f, 0.5f));
			vertices.Add (new Vector3 (0.5f, 0.5f, -0.5f));
			vertices.Add (new Vector3 (-0.5f, 0.5f, 0.5f));
			
			uvs.Add (new Vector2 ((point.x - delta.x) / width, (point.z - delta.z) / height));
			uvs.Add (new Vector2 ((point.x + delta.x) / width, (point.z + delta.z) / height));
			uvs.Add (new Vector2 ((point.x + delta.x) / width, (point.z - delta.z) / height));
			uvs.Add (new Vector2 ((point.x - delta.x) / width, (point.z + delta.z) / height));

			for (int i = 0; i < 4; i++) {
				normals.Add (Vector3.up);
				tangents.Add (new Vector4 (0f, -1f, 0f, -1f));
			}
		}
		
		protected void SetDownFace(float width, float height) {
			AddTriangles (vertices.Count);
			
			vertices.Add (new Vector3 (-0.5f, -0.5f, 0.5f));
			vertices.Add (new Vector3 (0.5f, -0.5f, -0.5f));
			vertices.Add (new Vector3 (0.5f, -0.5f, 0.5f));
			vertices.Add (new Vector3 (-0.5f, -0.5f, -0.5f));

			uvs.Add (new Vector2 ((point.x + delta.x) / width, (point.z + delta.z) / height));
			uvs.Add (new Vector2 ((point.x - delta.x) / width, (point.z - delta.z) / height));
			uvs.Add (new Vector2 ((point.x - delta.x) / width, (point.z + delta.z) / height));
			uvs.Add (new Vector2 ((point.x + delta.x) / width, (point.z - delta.z) / height));
			
			for (int i = 0; i < 4; i++) {
				normals.Add (Vector3.down);
				tangents.Add (new Vector4 (0f, -1f, 0f, -1f));
			}
		}
	}
}
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;
using Mypi.Utils;

namespace Mypi.Block {
	public enum BlockCategory {Exterior, Interior, Structure, Terrain};

	[Serializable]
	public struct BlockData {
		public string name;
		public IntVector3 coord;
		public int angle;
		
		public override string ToString ()
		{
			return string.Format ("name:{0}, coord:{1}, angle:{2}", name, coord, angle);
		}
	}

	public class BlockObject : MonoBehaviour {
		// Basic Attribute
		public static int hey;
		public Transform _transform;
		public string blockName;
		public BlockCategory blockCategory;
	
		public IntVector3 coord {
			get {
				return new IntVector3(_transform.localPosition);
			}
			set {
				_transform.localPosition = value.ToVector3();
			}
		}

		public int angle {
			get {
				return Mathf.RoundToInt(_transform.localEulerAngles.y);
			}
			set {
				_transform.localEulerAngles = new Vector3(0f, value, 0f);
			}
		}

		public int layer {
			get {
				return gameObject.layer;
			}
			set {
				SetLayerRecursively(_transform, value);
			}
		}

		public Color color {
			set {
				SetColorRecursively(transform, value);
			}
		}
		
		public Transform parent {
			get {
				return _transform.parent;
			}
			set {
				_transform.SetParent(value);
			}
		}

		private BlockData _blockData;
		public BlockData blockData {
			get {
				_blockData.name = blockName;
				_blockData.coord = coord;
				_blockData.angle = angle;
				return _blockData;
			}
		}

		private List<Collider> colliders;
		private List<Renderer> renderers;


		public static void SetShadowCast(Transform t, bool value) {
			SetShadowCastRecursively (t, value);
		}

		public static void SetShadowCastRecursively(Transform t, bool value) {
			Renderer r = t.GetComponent<Renderer> ();
			if (r != null) {
				r.receiveShadows = value;
				if (value)
					r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
				else
					r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			}

			foreach (Transform child in t)
				SetShadowCastRecursively(child, value);
		}

		public static void SetColorRecursively(Transform t, Color color) {
			Renderer r = t.GetComponent<Renderer> ();
			if (r != null) {
				foreach(var c in r.materials)
					c.color = color;
			}
			
			foreach (Transform child in t)
				SetColorRecursively(child, color);
		}

		public virtual void Action() {
		}
				
		public void RotateLeft() {
			_transform.Rotate (-90f * Vector3.up);
		}

		public void RotateRight() {
			_transform.Rotate (90f * Vector3.up);
		}


		public void SetActive(bool value) {
			gameObject.SetActive (value);
		}

		protected void SetLayerRecursively(Transform t, int layer) {
			t.gameObject.layer = layer;
			foreach (Transform child in t)
				SetLayerRecursively(child, layer);
		}

		public virtual Range[] GetRanges() {
			return null;
		}

		public virtual IntVector3[] GetCoords() {
			return null;
		}
	}
}
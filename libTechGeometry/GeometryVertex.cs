using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace libTechGeometry {
	public struct Vertex {
		public Vector3 Position;
		public Vector3 Normal;
		public Vector3 Tangent;
		public Vector4 Color;
		public Vector2 UV;

		public static implicit operator Vertex(Vector3 Position) {
			return new Vertex() { Position = Position };
		}
	}

	public static class VertexUtils {
		public static Vertex[] ToVertices(this Vector3[] Positions) {
			Vertex[] Verts = new Vertex[Positions.Length];

			for (int i = 0; i < Verts.Length; i++)
				Verts[i] = Positions[i];

			return Verts;
		}

		public static Vertex[] ExpandIndices(this Vertex[] Verts, uint[] Inds) {
			Vertex[] Expanded = new Vertex[Inds.Length];

			for (int i = 0; i < Inds.Length; i++)
				Expanded[i] = Verts[Inds[i]];

			return Expanded;
		}
	}
}

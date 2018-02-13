using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace libTechGeometry {
	public class GeometryMesh : IImportable {
		public GeometryMaterial Material;

		public Vertex[] Vertices;
		public uint[] Indices;

		public GeometryMesh() {
			Material = GeometryMaterial.Empty;

			Vertices = null;
			Indices = null;
		}

		public void GenerateSequentialIndices() {
			Indices = new uint[Vertices.Length];
			for (int i = 0; i < Indices.Length; i++)
				Indices[i] = (uint)i;
		}

		public Vertex[] GetExpandedVertices() {
			if (Vertices == null)
				throw new InvalidOperationException("Vertices cannot be null");
			if (Indices == null)
				throw new Exception("Indices cannot be null");

			return Vertices.ExpandIndices(Indices);
		}

		public override string ToString() {
			return string.Format("Mesh({0})", Vertices?.Length.ToString() ?? "null");
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace libTechGeometry {
	public class GeometryMesh : IImportable {
		public GeometryMaterial Material;

		public Vector3[] Vertices;
		public Vector4[] Colors;
		public Vector2[] UVs;
		public uint[] Indices;

		public GeometryMesh() {
			Material = GeometryMaterial.Empty;

			Vertices = null;
			Colors = null;
			UVs = null;
			Indices = null;
		}

		public override string ToString() {
			return string.Format("Mesh({0})", Vertices?.Length.ToString() ?? "null");
		}
	}
}

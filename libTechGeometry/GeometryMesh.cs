using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace libTechGeometry {
	public class GeometryMesh : IImportable {
		public bool IsTransparent;

		public GeometryMaterial Material;
		public Vector3[] Vertices;
		public Vector4[] Colors;
		public Vector2[] UVs;
		public uint[] Indices;

		public GeometryMesh() {
			IsTransparent = false;

			Vertices = null;
			Colors = null;
			UVs = null;
			Indices = null;
		}
	}
}

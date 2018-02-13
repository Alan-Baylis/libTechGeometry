using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libTechGeometry {
	public struct GeometryMaterial : IImportable {
		public static readonly GeometryMaterial Empty = new GeometryMaterial() {
			Name = null,
			DiffuseTexture = null,
			IsTransparent = false,
		};

		public string Name;

		public string DiffuseTexture;

		public bool IsTransparent;

		public override string ToString() {
			return string.Format("Material({0})", Name);
		}
	}
}

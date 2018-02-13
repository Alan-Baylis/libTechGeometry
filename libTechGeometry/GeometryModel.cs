using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libTechGeometry {
	public class GeometryModel : IImportable {
		public List<GeometryMesh> Meshes;

		public GeometryModel() {
			Meshes = new List<GeometryMesh>();
		}

		public override string ToString() {
			return string.Format("Model({0})", Meshes.Count);
		}
	}
}

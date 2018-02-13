using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace libTechGeometry.Importers {
	class Example : Importer<GeometryModel> {
		public override bool CanImportFile(string FilePath, string FileName, string FileExtension) {
			return FileExtension == ".ext";
		}

		public override GeometryModel Load(string FilePath) {
			GeometryModel Mdl = new GeometryModel();

			GeometryMesh MeshA = new GeometryMesh();
			MeshA.Vertices = new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1) };
			MeshA.Material = new GeometryMaterial("material a");
			Mdl.Meshes.Add(MeshA);

			return Mdl;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace libTechGeometry.Importers {
	class Example : Importer<GeometryModel> {
		public override bool CanImportFile(string FilePath, string FileName, string FileExtension) {
			return FileExtension == ".ext";
		}

		public override bool TryLoad(string FilePath, out GeometryModel Mdl) {
			Mdl = new GeometryModel();

			// Virtual file system support by replacing the default FileExists/Open functions
			// Equivalent to Importer.OpenIfExists
			Stream FileStream = Importer.FileExists(FilePath) ? Importer.Open(FilePath) : null;

			GeometryMesh MeshA = new GeometryMesh();
			MeshA.Vertices = new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1) }.ToVertices();
			
			MeshA.Material.Name = "glass";
			MeshA.Material.DiffuseTexture = "glass.png";
			MeshA.Material.IsTransparent = true;

			Mdl.Meshes.Add(MeshA);

			return true;
		}
	}
}

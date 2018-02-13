using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace libTechGeometry {
	public interface IImportable {
	}

	public abstract class Importer<T> where T : IImportable {
		public bool CanImportFile(string FilePath) {
			return CanImportFile(Path.GetFullPath(FilePath), Path.GetFileName(FilePath), Path.GetExtension(FilePath));
		}

		public abstract bool CanImportFile(string FilePath, string FileName, string FileExtension);

		public abstract T Load(string FilePath);
	}
}

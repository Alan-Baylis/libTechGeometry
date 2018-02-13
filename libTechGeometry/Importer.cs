using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace libTechGeometry {
	public interface IImportable {
	}

	interface IImporter {
		bool CanImportFile(string FilePath);
		object LoadBoxed(string FilePath);
	}

	abstract class Importer<T> : IImporter where T : IImportable {
		public bool CanImportFile(string FilePath) {
			return CanImportFile(Path.GetFullPath(FilePath), Path.GetFileName(FilePath), Path.GetExtension(FilePath));
		}

		public object LoadBoxed(string FilePath) {
			if (TryLoad(FilePath, out T Val))
				return Val;

			return null;
		}

		public abstract bool CanImportFile(string FilePath, string FileName, string FileExtension);
		public abstract bool TryLoad(string FilePath, out T Val);
	}

	public delegate bool FileExistsFunc(string FileName);
	public delegate Stream OpenFunc(string FileName, FileMode Mode = FileMode.Open, FileAccess Access = FileAccess.Read);

	public static class Importer {
		static List<IImporter> Importers;

		static Importer() {
			Importers = new List<IImporter>();
			Type[] Types = Assembly.GetExecutingAssembly().GetTypes();

			foreach (var T in Types) {
				if (!T.IsAbstract && !T.IsInterface && typeof(IImporter).IsAssignableFrom(T))
					Importers.Add((IImporter)Activator.CreateInstance(T));
			}
		}

		public static FileExistsFunc FileExists = File.Exists;
		public static OpenFunc Open = File.Open;

		public static Stream OpenIfExists(string FileName, FileMode Mode = FileMode.Open, FileAccess Access = FileAccess.Read) {
			return FileExists(FileName) ? Open(FileName, Mode, Access) : null;
		}

		public static T Load<T>(string FilePath) where T : IImportable {
			foreach (var Imp in Importers)
				if (Imp.CanImportFile(FilePath)) {
					object Value = Imp.LoadBoxed(FilePath);

					if (Value == null)
						continue;

					return (T)Value;
				}

			return default(T);
		}
	}
}

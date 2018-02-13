using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libTechGeometry;

namespace Test {
	class Program {
		static void Main(string[] args) {
			GeometryModel Mdl = Importer.Load<GeometryModel>("test.ext");


			Console.WriteLine("Done!");
			Console.ReadLine();
		}
	}
}

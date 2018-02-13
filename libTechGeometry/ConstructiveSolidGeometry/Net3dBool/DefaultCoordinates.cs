using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Point3d = System.Numerics.Vector3;
using Vector3d = System.Numerics.Vector3;

namespace Net3dBool {
	public class DefaultCoordinates {
		public static Point3d[] DEFAULT_BOX_VERTICES = new Point3d[] {
			new Point3d(-0.5f, -0.5f, -0.5f),
			new Point3d(0.5f, -0.5f, -0.5f),
			new Point3d(-0.5f, 0.5f, -0.5f),
			new Point3d(0.5f, 0.5f, -0.5f),
			new Point3d(-0.5f, -0.5f, 0.5f),
			new Point3d(0.5f, -0.5f, 0.5f),
			new Point3d(-0.5f, 0.5f, 0.5f),
			new Point3d(0.5f, 0.5f, 0.5f)
		};

		public static uint[] DEFAULT_BOX_COORDINATES = new uint[] {
			0, 2, 3,
			3, 1, 0,
			4, 5, 7,
			7, 6, 4,
			0, 1, 5,
			5, 4, 0,
			1, 3, 7,
			7, 5, 1,
			3, 2, 6,
			6, 7, 3,
			2, 0, 4,
			4, 6, 2
		};
	}
}
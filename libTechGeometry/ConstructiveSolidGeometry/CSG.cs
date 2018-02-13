using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using Net3dBool;

namespace libTechGeometry.ConstructiveSolidGeometry {
	public static class CSG {
		static GeometryMesh SolidToMesh(Solid S) {
			GeometryMesh Mesh = new GeometryMesh();
			Mesh.Vertices = S.Vertices.ToVertices(S.Colors);
			Mesh.Indices = S.Indices;
			return Mesh;
		}

		public static GeometryMesh CreateBox(Vector3 Position, Vector3 Scale, Vector4 Clr) {
			Vector4[] Colors = new Vector4[DefaultCoordinates.DEFAULT_BOX_VERTICES.Length];
			for (int i = 0; i < Colors.Length; i++)
				Colors[i] = Clr;

			Solid BoxSolid = new Solid(DefaultCoordinates.DEFAULT_BOX_VERTICES, DefaultCoordinates.DEFAULT_BOX_COORDINATES, Colors);
			BoxSolid.scale(Scale.X, Scale.Y, Scale.Z);
			BoxSolid.translate(Position.X, Position.Y);
			BoxSolid.zoom(Position.Z);

			return SolidToMesh(BoxSolid);
		}

		public static GeometryMesh Intersection(GeometryMesh A, GeometryMesh B) {
			Solid SolidA = new Solid(A.Vertices.Select((V) => V.Position).ToArray(), A.Indices, A.Vertices.Select((V) => V.Color).ToArray());
			Solid SolidB = new Solid(B.Vertices.Select((V) => V.Position).ToArray(), B.Indices, B.Vertices.Select((V) => V.Color).ToArray());

			BooleanModeller Modeller = new BooleanModeller(SolidA, SolidB);
			return SolidToMesh(Modeller.getIntersection());
		}
	}
}

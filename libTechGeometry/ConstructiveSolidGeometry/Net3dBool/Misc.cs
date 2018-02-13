using System;
using System.Collections;
using System.Collections.Generic;

using Point3d = System.Numerics.Vector3;
using Vector3d = System.Numerics.Vector3;

namespace Net3dBool {

	/*public struct Color {
		public float R, G, B, A;

		public Color3f(double r, double g, double b) {
			this.R = r;
			this.G = g;
			this.B = b;
		}

		public Color3f Clone() {
			return new Color3f(R, G, B);
		}
	}*/

	public class Shape3D {

	}

	/*public class Tuple3d {
		public double X;
		public double Y;
		public double Z;

		public Tuple3d(double X, double Y, double Z) {
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}

		public Tuple3d() {
		}

	}

	public class Point3d : Tuple3d {
		public Point3d(double x, double y, double z)
			: base(x, y, z) {
		}

		public Point3d() {
		}

		public double distance(Point3d p1) {
			double dx, dy, dz;

			dx = this.X - p1.X;
			dy = this.Y - p1.Y;
			dz = this.Z - p1.Z;
			return Math.Sqrt(dx * dx + dy * dy + dz * dz);
		}

		public Point3d Clone() {
			return new Point3d(X, Y, Z);
		}

	}

	public class Vector3d : Tuple3d {

		public Vector3d Clone() {
			return new Vector3d(X, Y, Z);
		}

		public double length() {
			return Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
		}

		public double angle(Vector3d v1) {
			double vDot = this.dot(v1) / (this.length() * v1.length());
			if (vDot < -1.0)
				vDot = -1.0;
			if (vDot > 1.0)
				vDot = 1.0;
			return ((double)(Math.Acos(vDot)));
		}

		public double dot(Vector3d v1) {
			return (this.X * v1.X + this.Y * v1.Y + this.Z * v1.Z);
		}

		public Vector3d(double x, double y, double z)
			: base(x, y, z) {
		}

		public Vector3d() {
		}

		public void cross(Vector3d v1, Vector3d v2) {
			double x, y;

			x = v1.Y * v2.Z - v1.Z * v2.Y;
			y = v2.X * v1.Z - v2.Z * v1.X;
			this.Z = v1.X * v2.Y - v1.Y * v2.X;
			this.X = x;
			this.Y = y;
		}

		public void normalize() {
			double norm;

			norm = 1.0 / Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
			this.X *= norm;
			this.Y *= norm;
			this.Z *= norm;
		}
	}*/

	public static class Helper {
		public static float Angle(this Vector3d A, Vector3d B) {
			float vDot = Vector3d.Dot(A, B) / (A.Length() * B.Length());

			if (vDot < -1.0f)
				vDot = -1.0f;
			if (vDot > 1.0f)
				vDot = 1.0f;

			return (float)Math.Acos(vDot);
		}

		public static void fill<T>(this T[] self, T value) {
			for (var i = 0; i < self.Length; i++) {
				self[i] = value;
			}
		}

		//        public static double dot(this Vector3d self, Vector3d v)
		//        {
		//            double dot = self.X * v.X + self.Y * v.Y + self.Z * v.Z;
		//            return dot;
		//        }
		//
		//        public static Vector3d cross(this Vector3d self, Vector3d v)
		//        {
		//            double crossX = self.Y * v.Z - v.Y * self.Z;
		//            double crossY = self.Z * v.X - v.Z * self.X;
		//            double crossZ = self.X * v.Y - v.X * self.Y;
		//            return new Vector3d(crossX, crossY, crossZ);
		//        }
		//
		//        public static double distance(this Vector3d v1, Vector3d v2)
		//        {
		//            double dx = v1.X - v2.X;
		//            double dy = v1.Y - v2.Y;
		//            double dz = v1.Z - v2.Z;
		//            return (double)Math.Sqrt(dx * dx + dy * dy + dz * dz);
		//        }

		private static Random rnd = new Random();

		public static float random() {
			return (float)rnd.NextDouble();
		}
	}
}


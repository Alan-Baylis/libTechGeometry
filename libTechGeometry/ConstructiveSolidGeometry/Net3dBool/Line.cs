/**
 * Representation of a 3d line or a ray (represented by a direction and a point).
 * 
 * <br><br>See: 
 * D. H. Laidlaw, W. B. Trumbore, and J. F. Hughes.  
 * "Constructive Solid Geometry for Polyhedral Objects" 
 * SIGGRAPH Proceedings, 1986, p.161.
 *  
 * original author: Danilo Balby Silva Castanheira (danbalby@yahoo.com)
 * 
 * Ported from Java to C# by Sebastian Loncar, Web: http://loncar.de
 * Project: https://github.com/Arakis/Net3dBool
 */

using System;
using System.Collections;
using System.Collections.Generic;

using Point3d = System.Numerics.Vector3;
using Vector3d = System.Numerics.Vector3;

namespace Net3dBool {
	public class Line {
		/** a line point */
		private Point3d point;
		/** line direction */
		private Vector3d direction;

		/** tolerance value to test equalities */
		private static double TOL = 1e-10f;

		//----------------------------------CONSTRUCTORS---------------------------------//

		/**
     * Constructor for a line. The line created is the intersection between two planes 
     * 
     * @param face1 face representing one of the planes 
     * @param face2 face representing one of the planes
     */
		public Line(Face face1, Face face2) {
			Vector3d normalFace1 = face1.getNormal();
			Vector3d normalFace2 = face2.getNormal();

			//direction: cross product of the faces normals
			//direction = new Vector3d();
			//direction.cross(normalFace1, normalFace2);

			direction = Vector3d.Cross(normalFace1, normalFace2);

			//if direction lenght is not zero (the planes aren't parallel )...
			if (!(direction.Length() < TOL)) {
				//getting a line point, zero is set to a coordinate whose direction 
				//component isn't zero (line intersecting its origin plan)
				point = new Point3d();
				float d1 = -(normalFace1.X * face1.v1.X + normalFace1.Y * face1.v1.Y + normalFace1.Z * face1.v1.Z);
				float d2 = -(normalFace2.X * face2.v1.X + normalFace2.Y * face2.v1.Y + normalFace2.Z * face2.v1.Z);
				if (Math.Abs(direction.X) > TOL) {
					point.X = 0;
					point.Y = (d2 * normalFace1.Z - d1 * normalFace2.Z) / direction.X;
					point.Z = (d1 * normalFace2.Y - d2 * normalFace1.Y) / direction.X;
				} else if (Math.Abs(direction.Y) > TOL) {
					point.X = (d1 * normalFace2.Z - d2 * normalFace1.Z) / direction.Y;
					point.Y = 0;
					point.Z = (d2 * normalFace1.X - d1 * normalFace2.X) / direction.Y;
				} else {
					point.X = (d2 * normalFace1.Y - d1 * normalFace2.Y) / direction.Z;
					point.Y = (d1 * normalFace2.X - d2 * normalFace1.X) / direction.Z;
					point.Z = 0;
				}
			}

			direction = Vector3d.Normalize(direction);
		}

		private Line() {
		}

		/**
     * Constructor for a ray
     * 
     * @param direction direction ray
     * @param point beginning of the ray
     */
		public Line(Vector3d direction, Point3d point) {
			this.direction = Vector3d.Normalize(direction);
			this.point = point;
			//direction.normalize();
		}

		//---------------------------------OVERRIDES------------------------------------//

		/**
     * Clones the Line object
     * 
     * @return cloned Line object
     */
		public Line Clone() {
			Line clone = new Line();
			clone.direction = direction;
			clone.point = point;
			return clone;
		}

		/**
     * Makes a string definition for the Line object
     * 
     * @return the string definition
     */
		public String toString() {
			return "Direction: " + direction.ToString() + "\nPoint: " + point.ToString();
		}

		//-----------------------------------GETS---------------------------------------//

		/**
     * Gets the point used to represent the line
     * 
     * @return point used to represent the line
     */
		public Point3d getPoint() {
			return point;
		}

		/**
     * Gets the line direction
     * 
     * @return line direction
     */
		public Vector3d getDirection() {
			return direction;
		}

		//-----------------------------------SETS---------------------------------------//

		/**
     * Sets a new point
     * 
     * @param point new point
     */
		public void setPoint(Point3d point) {
			this.point = point;
		}

		/**
     * Sets a new direction
     * 
     * @param direction new direction
     */
		public void setDirection(Vector3d direction) {
			this.direction = direction;
		}

		//--------------------------------OTHERS----------------------------------------//

		/**
     * Computes the distance from the line point to another point
     * 
     * @param otherPoint the point to compute the distance from the line point. The point 
     * is supposed to be on the same line.
     * @return points distance. If the point submitted is behind the direction, the 
     * distance is negative 
     */
		public double computePointToPointDistance(Point3d otherPoint) {
			//float distance = otherPoint.distance(point);
			float distance = Vector3d.Distance(otherPoint, point);

			Vector3d vec = Vector3d.Normalize(new Vector3d(otherPoint.X - point.X, otherPoint.Y - point.Y, otherPoint.Z - point.Z));

			if (Vector3d.Dot(vec, direction) < 0) {
				return -distance;
			} else {
				return distance;
			}
		}

		/**
     * Computes the point resulting from the intersection with another line
     * 
     * @param otherLine the other line to apply the intersection. The lines are supposed
     * to intersect
     * @return point resulting from the intersection. If the point coundn't be obtained, return null
     */
		public Point3d? computeLineIntersection(Line otherLine) {
			//x = x1 + a1*t = x2 + b1*s
			//y = y1 + a2*t = y2 + b2*s
			//z = z1 + a3*t = z2 + b3*s

			Point3d linePoint = otherLine.getPoint();
			Vector3d lineDirection = otherLine.getDirection();

			float t;

			if (Math.Abs(direction.Y * lineDirection.X - direction.X * lineDirection.Y) > TOL) {
				t = (-point.Y * lineDirection.X + linePoint.Y * lineDirection.X + lineDirection.Y * point.X - lineDirection.Y * linePoint.X) / (direction.Y * lineDirection.X - direction.X * lineDirection.Y);
			} else if (Math.Abs(-direction.X * lineDirection.Z + direction.Z * lineDirection.X) > TOL) {
				t = -(-lineDirection.Z * point.X + lineDirection.Z * linePoint.X + lineDirection.X * point.Z - lineDirection.X * linePoint.Z) / (-direction.X * lineDirection.Z + direction.Z * lineDirection.X);
			} else if (Math.Abs(-direction.Z * lineDirection.Y + direction.Y * lineDirection.Z) > TOL) {
				t = (point.Z * lineDirection.Y - linePoint.Z * lineDirection.Y - lineDirection.Z * point.Y + lineDirection.Z * linePoint.Y) / (-direction.Z * lineDirection.Y + direction.Y * lineDirection.Z);
			} else
				return null;

			float x = point.X + direction.X * t;
			float y = point.Y + direction.Y * t;
			float z = point.Z + direction.Z * t;

			return new Point3d(x, y, z);
		}

		/**
     * Compute the point resulting from the intersection with a plane
     * 
     * @param normal the plane normal
     * @param planePoint a plane point. 
     * @return intersection point. If they don't intersect, return null
     */
		public Point3d? computePlaneIntersection(Vector3d normal, Point3d planePoint) {
			//Ax + By + Cz + D = 0
			//x = x0 + t(x1 � x0)
			//y = y0 + t(y1 � y0)
			//z = z0 + t(z1 � z0)
			//(x1 - x0) = dx, (y1 - y0) = dy, (z1 - z0) = dz
			//t = -(A*x0 + B*y0 + C*z0 )/(A*dx + B*dy + C*dz)

			float A = normal.X;
			float B = normal.Y;
			float C = normal.Z;
			float D = -(normal.X * planePoint.X + normal.Y * planePoint.Y + normal.Z * planePoint.Z);

			float numerator = A * point.X + B * point.Y + C * point.Z + D;
			float denominator = A * direction.X + B * direction.Y + C * direction.Z;

			//if line is paralel to the plane...
			if (Math.Abs(denominator) < TOL) {
				//if line is contained in the plane...
				if (Math.Abs(numerator) < TOL) {
					return point;
				} else {
					return null;
				}
			}
			//if line intercepts the plane...
			else {
				float t = -numerator / denominator;
				Point3d resultPoint = new Point3d();
				resultPoint.X = point.X + t * direction.X;
				resultPoint.Y = point.Y + t * direction.Y;
				resultPoint.Z = point.Z + t * direction.Z;

				return resultPoint;
			}
		}

		/** Changes slightly the line direction */
		public void perturbDirection() {
			direction.X += 1e-5f * Helper.random();
			direction.Y += 1e-5f * Helper.random();
			direction.Z += 1e-5f * Helper.random();
		}
	}
}


/** 
 * Represents of a 3d face vertex.
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
using System.Numerics;
using Point3d = System.Numerics.Vector3;
using Vector3d = System.Numerics.Vector3;

namespace Net3dBool {
	public class Vertex {
		/** vertex coordinate in X */
		public float X;
		/** vertex coordinate in Y */
		public float Y;
		/** vertex coordinate in Z */
		public float Z;
		/** references to vertices conected to it by an edge  */
		private List<Vertex> adjacentVertices;
		/** vertex status relative to other object */
		private int status;
		/** vertex color */
		private Vector4 color;

		/** tolerance value to test equalities */
		private static double TOL = 1e-5f;

		/** vertex status if it is still unknown */
		public static int UNKNOWN = 1;
		/** vertex status if it is inside a solid */
		public static int INSIDE = 2;
		/** vertex status if it is outside a solid */
		public static int OUTSIDE = 3;
		/** vertex status if it on the boundary of a solid */
		public static int BOUNDARY = 4;

		//----------------------------------CONSTRUCTORS--------------------------------//

		/**
     * Constructs a vertex with unknown status
     * 
     * @param position vertex position
     * @param color vertex color
     */
		public Vertex(Point3d position, Vector4 color) {
			this.color = color;

			X = position.X;
			Y = position.Y;
			Z = position.Z;

			adjacentVertices = new List<Vertex>();
			status = UNKNOWN;
		}

		/**
     * Constructs a vertex with unknown status
     * 
     * @param x coordinate on the x axis
     * @param y coordinate on the y axis
     * @param z coordinate on the z axis
     * @param color vertex color
     */
		public Vertex(float x, float y, float z, Vector4 color) {
			this.color = color;

			this.X = x;
			this.Y = y;
			this.Z = z;

			adjacentVertices = new List<Vertex>();
			status = UNKNOWN;
		}

		/**
     * Constructs a vertex with definite status
     * 
     * @param position vertex position
     * @param color vertex color
     * @param status vertex status - UNKNOWN, BOUNDARY, INSIDE or OUTSIDE
     */
		public Vertex(Point3d position, Vector4 color, int status) {
			this.color = color;

			X = position.X;
			Y = position.Y;
			Z = position.Z;

			adjacentVertices = new List<Vertex>();
			this.status = status;
		}

		/**
     * Constructs a vertex with a definite status
     * 
     * @param x coordinate on the x axis
     * @param y coordinate on the y axis
     * @param z coordinate on the z axis
     * @param color vertex color
     * @param status vertex status - UNKNOWN, BOUNDARY, INSIDE or OUTSIDE
     */
		public Vertex(float x, float y, float z, Vector4 color, int status) {
			this.color = color;

			this.X = x;
			this.Y = y;
			this.Z = z;

			adjacentVertices = new List<Vertex>();
			this.status = status;
		}

		private Vertex() {
		}

		//-----------------------------------OVERRIDES----------------------------------//

		/**
     * Clones the vertex object
     * 
     * @return cloned vertex object
     */
		public Vertex Clone() {
			Vertex clone = new Vertex();
			clone.X = X;
			clone.Y = Y;
			clone.Z = Z;
			clone.color = color;
			clone.status = status;
			clone.adjacentVertices = new List<Vertex>();
			for (int i = 0; i < adjacentVertices.Count; i++) {
				clone.adjacentVertices.Add(adjacentVertices[i].Clone());
			}

			return clone;
		}

		/**
     * Makes a string definition for the Vertex object
     * 
     * @return the string definition
     */
		public String toString() {
			return "(" + X + ", " + Y + ", " + Z + ")";
		}

		/**
     * Checks if an vertex is equal to another. To be equal, they have to have the same
     * coordinates(with some tolerance) and color
     * 
     * @param anObject the other vertex to be tested
     * @return true if they are equal, false otherwise. 
     */
		public bool equals(Vertex vertex) {
			return Math.Abs(X - vertex.X) < TOL && Math.Abs(Y - vertex.Y) < TOL
			&& Math.Abs(Z - vertex.Z) < TOL && color.Equals(vertex.color);
		}

		//--------------------------------------SETS------------------------------------//

		/**
     * Sets the vertex status
     * 
     * @param status vertex status - UNKNOWN, BOUNDARY, INSIDE or OUTSIDE
     */
		public void setStatus(int status) {
			if (status >= UNKNOWN && status <= BOUNDARY) {
				this.status = status;
			}
		}

		//--------------------------------------GETS------------------------------------//

		/**
     * Gets the vertex position
     * 
     * @return vertex position
     */
		public Point3d getPosition() {
			return new Point3d(X, Y, Z);
		}

		/**
     * Gets an array with the adjacent vertices
     * 
     * @return array of the adjacent vertices 
     */
		public Vertex[] getAdjacentVertices() {
			Vertex[] vertices = new Vertex[adjacentVertices.Count];
			for (int i = 0; i < adjacentVertices.Count; i++) {
				vertices[i] = adjacentVertices[i];
			}
			return vertices;
		}

		/**
     * Gets the vertex status
     * 
     * @return vertex status - UNKNOWN, BOUNDARY, INSIDE or OUTSIDE
     */
		public int getStatus() {
			return status;
		}

		/**
     * Gets the vertex color
     * 
     * @return vertex color
     */
		public Vector4 getColor() {
			return color;
		}

		//----------------------------------OTHERS--------------------------------------//

		/**
     * Sets a vertex as being adjacent to it
     * 
     * @param adjacentVertex an adjacent vertex
     */
		public void addAdjacentVertex(Vertex adjacentVertex) {
			if (!adjacentVertices.Contains(adjacentVertex)) {
				adjacentVertices.Add(adjacentVertex);
			}
		}

		/**
     * Sets the vertex status, setting equally the adjacent ones
     * 
     * @param status new status to be set
     */
		public void mark(int status) {
			//mark vertex
			this.status = status;

			//mark adjacent vertices
			Vertex[] adjacentVerts = getAdjacentVertices();
			for (int i = 0; i < adjacentVerts.Length; i++) {
				if (adjacentVerts[i].getStatus() == Vertex.UNKNOWN) {
					adjacentVerts[i].mark(status);
				}
			}
		}
	}
}


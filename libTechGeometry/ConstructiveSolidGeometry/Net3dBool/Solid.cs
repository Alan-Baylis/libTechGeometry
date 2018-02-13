/**
 * Class representing a 3D solid.
 *  
 * original author: Danilo Balby Silva Castanheira (danbalby@yahoo.com)
 * 
 * Ported from Java to C# by Sebastian Loncar, Web: http://loncar.de
 * Project: https://github.com/Arakis/Net3dBool
 */

using System;
using System.IO;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

using Point3d = System.Numerics.Vector3;
using Vector3d = System.Numerics.Vector3;
using Color = System.Numerics.Vector4;
using System.Numerics;

namespace Net3dBool {


	public class Solid : Shape3D {
		/** array of indices for the vertices from the 'vertices' attribute */
		public uint[] Indices;
		/** array of points defining the solid's vertices */
		public Point3d[] Vertices;
		/** array of color defining the vertices colors */
		public Vector4[] Colors;

		//--------------------------------CONSTRUCTORS----------------------------------//

		/** Constructs an empty solid. */
		public Solid() {
			setInitialFeatures();
		}

		/**
     * Construct a solid based on data arrays. An exception may occur in the case of 
     * abnormal arrays (indices making references to inexistent vertices, there are less
     * colors than vertices...)
     * 
     * @param vertices array of points defining the solid vertices
     * @param indices array of indices for a array of vertices
     * @param colors array of colors defining the vertices colors 
     */
		public Solid(Point3d[] vertices, uint[] indices, Vector4[] colors)
			: this() {
			setData(vertices, indices, colors);
		}

		/**
     * Constructs a solid based on a coordinates file. It contains vertices and indices, 
     * and its format is like this:
     * 
     * <br><br>4
     * <br>0 -5.00000000000000E-0001 -5.00000000000000E-0001 -5.00000000000000E-0001
     * <br>1  5.00000000000000E-0001 -5.00000000000000E-0001 -5.00000000000000E-0001
     * <br>2 -5.00000000000000E-0001  5.00000000000000E-0001 -5.00000000000000E-0001
     * <br>3  5.00000000000000E-0001  5.00000000000000E-0001 -5.00000000000000E-0001
     * 
     * <br><br>2
     * <br>0 0 2 3
     * <br>1 3 1 0 
     * 
     * @param solidFile file containing the solid coordinates
     * @param color solid color
     */
		public Solid(FileInfo solidFile, Color color)
			: this() {
			loadCoordinateFile(solidFile, color);
		}

		/** Sets the initial features common to all constructors */
		protected void setInitialFeatures() {
			Vertices = new Point3d[0];
			Colors = new Vector4[0];
			Indices = new uint[0];

			//            setCapability(Shape3D.ALLOW_GEOMETRY_WRITE);
			//            setCapability(Shape3D.ALLOW_APPEARANCE_WRITE);
			//            setCapability(Shape3D.ALLOW_APPEARANCE_READ);
		}

		//---------------------------------------GETS-----------------------------------//

		/**
     * Gets the solid vertices
     * 
     * @return solid vertices
     */
		/*public Point3d[] getVertices() {
			Point3d[] newVertices = new Point3d[vertices.Length];
			for (int i = 0; i < newVertices.Length; i++) {
				newVertices[i] = vertices[i];
			}
			return newVertices;
		}*/

		/** Gets the solid indices for its vertices
     * 
     * @return solid indices for its vertices
     */
		/*public int[] getIndices() {
			int[] newIndices = new int[indices.Length];
			Array.Copy(indices, 0, newIndices, 0, indices.Length);
			return newIndices;
		}*/

		/** Gets the vertices colors
     * 
     * @return vertices colors
     */
		/*public Color[] getColors() {
			Color[] newColors = new Color[colors.Length];
			for (int i = 0; i < newColors.Length; i++) {
				newColors[i] = colors[i];
			}
			return newColors;
		}*/

		/**
     * Gets if the solid is empty (without any vertex)
     * 
     * @return true if the solid is empty, false otherwise
     */
		public bool isEmpty() {
			if (Indices.Length == 0) {
				return true;
			} else {
				return false;
			}
		}

		//---------------------------------------SETS-----------------------------------//

		/**
     * Sets the solid data. Each vertex may have a different color. An exception may 
     * occur in the case of abnormal arrays (e.g., indices making references to  
     * inexistent vertices, there are less colors than vertices...)
     * 
     * @param vertices array of points defining the solid vertices
     * @param indices array of indices for a array of vertices
     * @param colors array of colors defining the vertices colors 
     */
		public void setData(Point3d[] vertices, uint[] indices, Vector4[] colors) {
			this.Vertices = new Point3d[vertices.Length];
			this.Colors = new Vector4[colors.Length];
			this.Indices = new uint[indices.Length];
			if (indices.Length != 0) {
				for (int i = 0; i < vertices.Length; i++) {
					this.Vertices[i] = vertices[i];
					this.Colors[i] = colors[i];
				}
				Array.Copy(indices, 0, this.Indices, 0, indices.Length);

				defineGeometry();
			}
		}

		/**
     * Sets the solid data. Defines the same color to all the vertices. An exception may 
     * may occur in the case of abnormal arrays (e.g., indices making references to  
     * inexistent vertices...)
     * 
     * @param vertices array of points defining the solid vertices
     * @param indices array of indices for a array of vertices
     * @param color the color of the vertices (the solid color) 
     */
		public void setData(Point3d[] vertices, uint[] indices, Vector4 color) {
			Vector4[] colors = new Vector4[vertices.Length];
			colors.fill(color);
			setData(vertices, indices, colors);
		}

		//-------------------------GEOMETRICAL_TRANSFORMATIONS-------------------------//

		/**
     * Applies a translation into a solid
     * 
     * @param dx translation on the x axis
     * @param dy translation on the y axis
     */
		public void translate(float dx, float dy) {
			if (dx != 0 || dy != 0) {
				for (int i = 0; i < Vertices.Length; i++) {
					Vertices[i].X += dx;
					Vertices[i].Y += dy;
				}

				defineGeometry();
			}
		}

		/**
     * Applies a rotation into a solid
     * 
     * @param dx rotation on the x axis
     * @param dy rotation on the y axis
     */
		public void rotate(float dx, float dy) {
			float cosX = (float)Math.Cos(dx);
			float cosY = (float)Math.Cos(dy);
			float sinX = (float)Math.Sin(dx);
			float sinY = (float)Math.Sin(dy);

			if (dx != 0 || dy != 0) {
				//get mean
				Point3d mean = getMean();

				float newX, newY, newZ;
				for (int i = 0; i < Vertices.Length; i++) {
					Vertices[i].X -= mean.X;
					Vertices[i].Y -= mean.Y;
					Vertices[i].Z -= mean.Z;

					//x rotation
					if (dx != 0) {
						newY = Vertices[i].Y * cosX - Vertices[i].Z * sinX;
						newZ = Vertices[i].Y * sinX + Vertices[i].Z * cosX;
						Vertices[i].Y = newY;
						Vertices[i].Z = newZ;
					}

					//y rotation
					if (dy != 0) {
						newX = Vertices[i].X * cosY + Vertices[i].Z * sinY;
						newZ = -Vertices[i].X * sinY + Vertices[i].Z * cosY;
						Vertices[i].X = newX;
						Vertices[i].Z = newZ;
					}

					Vertices[i].X += mean.X;
					Vertices[i].Y += mean.Y;
					Vertices[i].Z += mean.Z;
				}
			}

			defineGeometry();
		}

		/**
     * Applies a zoom into a solid
     * 
     * @param dz translation on the z axis
     */
		public void zoom(float dz) {
			if (dz != 0) {
				for (int i = 0; i < Vertices.Length; i++) {
					Vertices[i].Z += dz;
				}

				defineGeometry();
			}
		}

		/**
     * Applies a scale changing into the solid
     * 
     * @param dx scale changing for the x axis 
     * @param dy scale changing for the y axis
     * @param dz scale changing for the z axis
     */
		public void scale(float dx, float dy, float dz) {
			for (int i = 0; i < Vertices.Length; i++) {
				Vertices[i].X *= dx;
				Vertices[i].Y *= dy;
				Vertices[i].Z *= dz;
			}

			defineGeometry();
		}

		//-----------------------------------PRIVATES--------------------------------//

		/** Creates a geometry based on the indexes and vertices set for the solid */
		protected void defineGeometry() {
			//            GeometryInfo gi = new GeometryInfo(GeometryInfo.TRIANGLE_ARRAY);
			//            gi.setCoordinateIndices(indices);
			//            gi.setCoordinates(vertices);
			//            NormalGenerator ng = new NormalGenerator();
			//            ng.generateNormals(gi);
			//
			//            gi.setColors(colors);
			//            gi.setColorIndices(indices);
			//            gi.recomputeIndices();
			//
			//            setGeometry(gi.getIndexedGeometryArray());
		}

		/**
     * Loads a coordinates file, setting vertices and indices 
     * 
     * @param solidFile file used to create the solid
     * @param color solid color
     */
		protected void loadCoordinateFile(FileInfo solidFile, Color color) {
			//            try
			//            {
			//                BufferedReader reader = new BufferedReader(new FileReader(solidFile));
			//
			//                String line = reader.readLine();
			//                int numVertices = Integer.parseInt(line);
			//                vertices = new Point3d[numVertices];
			//
			//                StringTokenizer tokens;
			//                String token;
			//
			//                for(int i=0;i<numVertices;i++)
			//                    {
			//                        line = reader.readLine();
			//                        tokens = new StringTokenizer(line);
			//                        tokens.nextToken();
			//                        vertices[i]= new Point3d(Double.parseDouble(tokens.nextToken()), Double.parseDouble(tokens.nextToken()), Double.parseDouble(tokens.nextToken()));
			//                    }
			//
			//                reader.readLine();
			//
			//                line = reader.readLine();
			//                int numTriangles = Integer.parseInt(line);
			//                indices = new int[numTriangles*3];
			//
			//                for(int i=0,j=0;i<numTriangles*3;i=i+3,j++)
			//                    {
			//                        line = reader.readLine();
			//                        tokens = new StringTokenizer(line);
			//                        tokens.nextToken();
			//                        indices[i] = Integer.parseInt(tokens.nextToken());
			//                        indices[i+1] = Integer.parseInt(tokens.nextToken());
			//                        indices[i+2] = Integer.parseInt(tokens.nextToken());
			//                    }
			//
			//                colors = new Color3f[vertices.Length];
			//                Arrays.fill(colors, color);
			//
			//                defineGeometry();
			//            }
			//
			//            catch(IOException e)
			//            {
			//                System.out.println("invalid file!");
			//                e.printStackTrace();
			//            }
		}

		/**
     * Gets the solid mean
     * 
     * @return point representing the mean
     */
		protected Point3d getMean() {
			Point3d mean = new Point3d();
			for (int i = 0; i < Vertices.Length; i++) {
				mean.X += Vertices[i].X;
				mean.Y += Vertices[i].Y;
				mean.Z += Vertices[i].Z;
			}
			mean.X /= Vertices.Length;
			mean.Y /= Vertices.Length;
			mean.Z /= Vertices.Length;

			return mean;
		}
	}
}


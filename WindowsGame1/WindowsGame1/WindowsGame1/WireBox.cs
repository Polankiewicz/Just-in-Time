#region File Description
//-----------------------------------------------------------------------------
// CubePrimitive.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace WindowsGame1
{
    /// <summary>
    /// Geometric primitive class for drawing cubes.
    /// </summary>
    public class WireBox : GeometricPrimitive
    {
        /// <summary>
        /// Constructs a new cube primitive, using default settings.
        /// </summary>
        public WireBox(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, new Vector3(0),new Vector3(10))
        {
        }


        /// <summary>
        /// Constructs a new cube primitive, with the specified size.
        /// </summary>
        public WireBox(GraphicsDevice graphicsDevice, Vector3 min, Vector3 max)
        {
            // A cube has six faces, each one pointing in a different direction.
            Vector3[] normals =
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1),
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, -1, 0),
            };


         
            Vector3 normal;

            normal = normals[0];
            Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
            Vector3 side2 = Vector3.Cross(normal, side1);

            // Six indices (two triangles) per face.
            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 2);

            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 2);
            AddIndex(CurrentVertex + 3);

            AddVertex(new Vector3(min.X, min.Y, min.Z), normals[0]);
            AddVertex(new Vector3(max.X, min.Y, min.Z), normals[0]);
            AddVertex(new Vector3(min.X, min.Y, max.Z), normals[0]);
            AddVertex(new Vector3(max.X, min.Y, max.Z), normals[0]);

            normal = normals[1];
            side1 = new Vector3(normal.Y, normal.Z, normal.X);
            side2 = Vector3.Cross(normal, side1);

            // Six indices (two triangles) per face.
            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 2);

            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 2);
            AddIndex(CurrentVertex + 3);

            AddVertex(new Vector3(min.X, min.Y, min.Z), normals[1]);
            AddVertex(new Vector3(min.X, min.Y, max.Z), normals[1]);
            AddVertex(new Vector3(max.X, min.Y, min.Z), normals[1]);
            AddVertex(new Vector3(max.X, min.Y, max.Z), normals[1]);

            normal = normals[2];
            side1 = new Vector3(normal.Y, normal.Z, normal.X);
            side2 = Vector3.Cross(normal, side1);

            // Six indices (two triangles) per face.
            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 2);

            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 2);
            AddIndex(CurrentVertex + 3);

            AddVertex(new Vector3(min.X, max.Y, min.Z), normals[2]);
            AddVertex(new Vector3(max.X, max.Y, min.Z), normals[2]);
            AddVertex(new Vector3(min.X, max.Y, max.Z), normals[2]);
            AddVertex(new Vector3(max.X, max.Y, max.Z), normals[2]);

            normal = normals[3];
            side1 = new Vector3(normal.Y, normal.Z, normal.X);
            side2 = Vector3.Cross(normal, side1);

            // Six indices (two triangles) per face.
            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 2);

            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 2);
            AddIndex(CurrentVertex + 3);

            AddVertex(new Vector3(min.X, max.Y, min.Z), normals[3]);
            AddVertex(new Vector3(min.X, max.Y, max.Z), normals[3]);
            AddVertex(new Vector3(max.X, max.Y, min.Z), normals[3]);
            AddVertex(new Vector3(max.X, max.Y, max.Z), normals[3]);

            normal = normals[4];
            side1 = new Vector3(normal.Y, normal.Z, normal.X);
            side2 = Vector3.Cross(normal, side1);

            // Six indices (two triangles) per face.
            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 2);

            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 2);
            AddIndex(CurrentVertex + 3);

            AddVertex(new Vector3(min.X, min.Y, min.Z), normals[4]);
            AddVertex(new Vector3(min.X, max.Y, min.Z), normals[4]);
            AddVertex(new Vector3(max.X, min.Y, min.Z), normals[4]);
            AddVertex(new Vector3(max.X, max.Y, min.Z), normals[4]);

            normal = normals[5];
            side1 = new Vector3(normal.Y, normal.Z, normal.X);
            side2 = Vector3.Cross(normal, side1);

            // Six indices (two triangles) per face.
            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 2);

            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 2);
            AddIndex(CurrentVertex + 3);

            AddVertex(new Vector3(min.X, min.Y, max.Z), normals[5]);
            AddVertex(new Vector3(min.X, max.Y, max.Z), normals[5]);
            AddVertex(new Vector3(max.X, min.Y, max.Z), normals[5]);
            AddVertex(new Vector3(max.X, max.Y, max.Z), normals[5]);


            InitializePrimitive(graphicsDevice);
        }
    }
}

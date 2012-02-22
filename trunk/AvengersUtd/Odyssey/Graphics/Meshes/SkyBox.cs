#region #Disclaimer

// /* 
//  * Timer
//  *
//  * Created on 21 August 2007
//  * Last update on 29 July 2010
//  * 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-Mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com
//  *
//  * Part of the Odyssey Engine.
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */

#endregion

#region Using directives

using AvengersUtd.Odyssey.Graphics.Resources;
using SlimDX;
using SlimDX.Direct3D11;

#endregion

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public class SkyBox : BaseMesh<Textured3DVertex>, Materials.IColor4Material, Materials.IDiffuseMap
    {
        private string diffuseMapKey;

        public Color4 DiffuseColor { get; set; }

        public Color4 SpecularColor4 { get; set; }

        public Color4 AmbientColor4 { get; set; }

        public SkyBox() : base(Textured3DVertex.Description)
        {
            Create();
            DiffuseColor = new Color4(System.Drawing.Color.CornflowerBlue);
            diffuseMapKey = Global.TexturePath + "OutputCube.dds";
            
            ShaderResourceList.Add(ResourceManager.LoadTexture2DResource(diffuseMapKey));
        }

        public string DiffuseMapKey
        {
            get { return diffuseMapKey; }
            set
            {
                if (diffuseMapKey == value) return;

                diffuseMapKey = value;

                ShaderResourceView srv = ResourceManager.GetResource(diffuseMapKey);
                DiffuseMapTexture2D = (Texture2D) srv.Resource;
                if (ShaderResourceList.Count == 0)
                    ShaderResourceList.Add(srv);
                else
                    ShaderResourceList[0] = srv;
            }
        }

        public Texture2D DiffuseMapTexture2D { get; private set; }

        public ShaderResourceView DiffuseMapResource
        {
            get { return ShaderResourceList[0]; }
        }

        private void Create()
        {
            Vector3 vExtents = new Vector3(1, 1, 1);
            Vertices = new Textured3DVertex[4*6];

            #region Back Face

            Vertices[0] = new Textured3DVertex
                              {
                                  Position = new Vector4(vExtents.X, -vExtents.Y, -vExtents.Z, 1.0f),
                                  TextureCoordinate = new Vector3(1.0f, -1.0f,-1.0f)
                              };
            Vertices[1] = new Textured3DVertex
                              {
                                  Position = new Vector4(vExtents.X, vExtents.Y, -vExtents.Z, 1.0f),
                                  TextureCoordinate = new Vector3(1.0f, 1.0f,-1.0f)
                              };
            Vertices[2] = new Textured3DVertex
                              {
                                  Position = new Vector4(-vExtents.X, vExtents.Y, -vExtents.Z, 1.0f),
                                  TextureCoordinate = new Vector3(-1.0f, 1.0f, -1.0f)
                              };
            Vertices[3] = new Textured3DVertex
                              {
                                  Position = new Vector4(-vExtents.X, -vExtents.Y, -vExtents.Z, 1.0f),
                                  TextureCoordinate = new Vector3(-1.0f, -1.0f, -1.0f)
                              };
            #endregion

            #region Front face

            Vertices[4] = new Textured3DVertex
                              {
                                  Position = new Vector4(-vExtents.X, -vExtents.Y, vExtents.Z, 1.0f),
                                  TextureCoordinate = new Vector3(-1.0f, -1.0f, 1.0f)
                              };

            Vertices[5] = new Textured3DVertex
                              {
                                  Position = new Vector4(-vExtents.X, vExtents.Y, vExtents.Z, 1.0f),
                                  TextureCoordinate = new Vector3(-1.0f, 1.0f, 1.0f)
                              };
            Vertices[6] = new Textured3DVertex
                              {
                                  Position = new Vector4(vExtents.X, vExtents.Y, vExtents.Z, 1.0f),
                                  TextureCoordinate = new Vector3(1.0f, 1.0f, 1.0f)
                              };
            Vertices[7] = new Textured3DVertex
                              {
                                  Position = new Vector4(vExtents.X, -vExtents.Y, vExtents.Z, 1.0f),
                                  TextureCoordinate = new Vector3(1.0f, -1.0f, 1.0f)

                              };
            #endregion

            #region Bottom face

            Vertices[8] = new Textured3DVertex
                              {
                                  Position = new Vector4(-vExtents.X, -vExtents.Y, -vExtents.Z, 1.0f),
                                  TextureCoordinate = new Vector3(1.0f, 1.0f, -1.0f)
                              };
            Vertices[9] = new Textured3DVertex
                              {
                                  Position = new Vector4(-vExtents.X, -vExtents.Y, vExtents.Z, 1.0f),
                                  TextureCoordinate = new Vector3(-1.0f, -1.0f, 1.0f)
                              };
            Vertices[10] = new Textured3DVertex
                               {
                                   Position = new Vector4(vExtents.X, -vExtents.Y, vExtents.Z, 1.0f),
                                   TextureCoordinate = new Vector3(1.0f, -1.0f, 1.0f)
                               };
            Vertices[11] = new Textured3DVertex
                               {
                                   Position = new Vector4(vExtents.X, -vExtents.Y, -vExtents.Z, 1.0f),
                                   TextureCoordinate = new Vector3(1.0f, -1.0f, -1.0f)
                               };
            #endregion

            #region Top face

            Vertices[12] = new Textured3DVertex
                               {
                                   Position = new Vector4(vExtents.X, vExtents.Y, -vExtents.Z, 1.0f),
                                   TextureCoordinate = new Vector3(1.0f, 1.0f, -1.0f)
                               };
            Vertices[13] = new Textured3DVertex
                               {
                                   Position = new Vector4(vExtents.X, vExtents.Y, vExtents.Z, 1.0f),
                                   TextureCoordinate = new Vector3(1.0f, 1.0f, 1.0f)
                               };

            Vertices[14] = new Textured3DVertex
                               {
                                   Position = new Vector4(-vExtents.X, vExtents.Y, vExtents.Z, 1.0f),
                                   TextureCoordinate = new Vector3(-1.0f, 1.0f, 1.0f)
                               };

            Vertices[15] = new Textured3DVertex
                               {
                                   Position = new Vector4(-vExtents.X, vExtents.Y, -vExtents.Z, 1.0f),
                                   TextureCoordinate = new Vector3(-1.0f, 1.0f, -1.0f)
                               };
            #endregion
 
            #region Left face

            Vertices[16] = new Textured3DVertex
                               {
                                   Position = new Vector4(-vExtents.X, vExtents.Y, -vExtents.Z, 1.0f),
                                   TextureCoordinate = new Vector3(-1.0f, 1.0f, -1.0f)
                               };

            Vertices[17] = new Textured3DVertex
                               {
                                   Position = new Vector4(-vExtents.X, vExtents.Y, vExtents.Z, 1.0f),
                                   TextureCoordinate = new Vector3(-1.0f, 1.0f, 1.0f)
                               };

            Vertices[18] = new Textured3DVertex
                               {
                                   Position = new Vector4(-vExtents.X, -vExtents.Y, vExtents.Z, 1.0f),
                                   TextureCoordinate = new Vector3(-1.0f, -1.0f, 1.0f)
                               };

            Vertices[19] = new Textured3DVertex
                               {
                                   Position = new Vector4(-vExtents.X, -vExtents.Y, -vExtents.Z, 1.0f),
                                   TextureCoordinate = new Vector3(-1.0f, -1.0f, -1.0f)
                               };
            #endregion
                                           
            #region Right face

            Vertices[20] = new Textured3DVertex
                               {
                                   Position = new Vector4(vExtents.X, -vExtents.Y, -vExtents.Z, 1.0f),
                                   TextureCoordinate = new Vector3(1.0f, -1.0f, -1.0f)
                               };

            Vertices[21] = new Textured3DVertex
                               {
                                   Position = new Vector4(vExtents.X, -vExtents.Y, vExtents.Z, 1.0f),
                                   TextureCoordinate = new Vector3(1.0f, -1.0f, 1.0f)
                               };

            Vertices[22] = new Textured3DVertex
                               {
                                   Position = new Vector4(vExtents.X, vExtents.Y, vExtents.Z, 1.0f),
                                   TextureCoordinate = new Vector3(1.0f, 1.0f, 1.0f)
                               };

            Vertices[23] = new Textured3DVertex
                               {
                                   Position = new Vector4(vExtents.X, vExtents.Y, -vExtents.Z, 1.0f),
                                   TextureCoordinate = new Vector3(1.0f, 1.0f, -1.0f)
                               };
            #endregion

            Indices = new ushort[6 * 6];

            for (int x = 0; x < 6; x++)
            {
                Indices[x * 6 + 0] = (ushort)(x * 4 + 0);
                Indices[x * 6 + 2] = (ushort)(x * 4 + 1);
                Indices[x * 6 + 1] = (ushort)(x * 4 + 2);

                Indices[x * 6 + 3] = (ushort)(x * 4 + 2);
                Indices[x * 6 + 5] = (ushort)(x * 4 + 3);
                Indices[x * 6 + 4] = (ushort)(x * 4 + 0);
            }

        }


    }
}
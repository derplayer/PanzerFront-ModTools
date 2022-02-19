//using ShenmueDKSharp.Files.Models._NTD;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenmueDKSharp.Files.Models
{
    /// <summary>
    /// Wavefront NTD format
    /// </summary>
    public class NTD : BaseModel
    {
        public static bool EnableBuffering = false;
        public override bool BufferingEnabled => EnableBuffering;

        private static CultureInfo m_cultureInfo = CultureInfo.InvariantCulture;

        public short ModelVersion;
        public ushort PartCount;

        public NTD(BaseModel model)
        {
            model.CopyTo(this);
            FilePath = Path.ChangeExtension(model.FilePath, "ntd");
        }
        public NTD(string filepath)
        {
            Read(filepath);
        }
        public NTD(Stream stream)
        {
            Read(stream);
        }
        public NTD(BinaryReader reader)
        {
            Read(reader);
        }

        protected override void _Read(BinaryReader reader)
        {
            Textures.Clear();
            RootNode = new ModelNode();
            RootNode.HasMesh = true;
            RootNode.ID = 0;

            //string fdir = Path.GetFileNameWithoutExtension(filepath);
            int tFactor = 10000;
            string dc = "DC_models" + Path.DirectorySeparatorChar;
            string obj = "#Export from PZ Tool\n";
            string vert = "";
            string face = "";
            string uvs = "";
            string mtl = "";

            //MeshFace currentFace = null;

            //if (!Directory.Exists(dc + fdir))
            //{
            //    Directory.CreateDirectory(dc + fdir);
            //}

            ModelVersion = reader.ReadInt16(); //model ver
            PartCount = reader.ReadUInt16();

            for (uint i = 0; i < PartCount; i++)
            {
                ModelNode PartNode = new ModelNode();
                PartNode.ID = i;

                obj = "#Export from PZ Tool\no PART_" + i.ToString("D3") + "\n";
                vert = "";
                face = "";
                uvs = "";
                int c = 1;
                uint blockCount = reader.ReadUInt32();
                if (blockCount == 0) break;

                for (int j = 0; j < blockCount; j++)
                {
                    // quad vertex
                    for (int k = 0; k < 4; k++)
                    {
                        float x = (float)(reader.ReadInt16() << 8) / tFactor;
                        float y = (float)(reader.ReadInt16() << 8) / tFactor;
                        float z = (float)(reader.ReadInt16() << 8) / tFactor;
                        //vert += "v " + x.ToString() + " " + y.ToString() + " " + z.ToString() + "\n";
                        Vector3 vertex = new Vector3(x, y, z);
                        PartNode.VertexPositions.Add(vertex);
                    }

                    MeshFace currentFace = new MeshFace();

                    //face += "f "
                    //    + (c).ToString() + "/" + (c).ToString()
                    //    + " " + (c + 2).ToString() + "/" + (c + 2).ToString()
                    //    + " " + (c + 3).ToString() + "/" + (c + 3).ToString()
                    //    + " " + (c + 1).ToString() + "/" + (c + 1).ToString() + "\n";
                    //c += 4;

                    currentFace.PositionIndices.Add((ushort)(c - 1));
                    currentFace.UVIndices.Add((ushort)(c - 1));

                    currentFace.PositionIndices.Add((ushort)(c + 2 - 1));
                    currentFace.UVIndices.Add((ushort)(c + 2 - 1));

                    currentFace.PositionIndices.Add((ushort)(c + 3 - 1));
                    currentFace.UVIndices.Add((ushort)(c + 3 - 1));

                    currentFace.PositionIndices.Add((ushort)(c + 1 - 1));
                    currentFace.UVIndices.Add((ushort)(c + 1 - 1));

                    c += 4;

                    PartNode.Faces.Add(currentFace);

                    float ux0 = (float)reader.ReadByte() / (256 - 1);
                    float ux1 = (float)reader.ReadByte() / (256 - 1);
                    float ux2 = (float)reader.ReadByte() / (256 - 1);
                    float ux3 = (float)reader.ReadByte() / (256 - 1);
                    float uy0 = 1f - (float)reader.ReadByte() / (256 - 1);
                    float uy1 = 1f - (float)reader.ReadByte() / (256 - 1);
                    float uy2 = 1f - (float)reader.ReadByte() / (256 - 1);
                    float uy3 = 1f - (float)reader.ReadByte() / (256 - 1);

                    //uvs += "vt " + ux0.ToString() + " " + uy0.ToString() + "\n";
                    //uvs += "vt " + ux1.ToString() + " " + uy1.ToString() + "\n";
                    //uvs += "vt " + ux2.ToString() + " " + uy2.ToString() + "\n";
                    //uvs += "vt " + ux3.ToString() + " " + uy3.ToString() + "\n";
                    Vector2 uv0 = new Vector2(ux0, uy0);
                    Vector2 uv1 = new Vector2(ux1, uy1);
                    Vector2 uv2 = new Vector2(ux2, uy2);
                    Vector2 uv3 = new Vector2(ux3, uy3);

                    PartNode.VertexUVs.Add(uv0);
                    PartNode.VertexUVs.Add(uv1);
                    PartNode.VertexUVs.Add(uv2);
                    PartNode.VertexUVs.Add(uv3);
                }

                RootNode.Children.Add(PartNode);
                //obj += "mtllib MAT_" + fdir + ".mtl\n" + vert + uvs + "usemtl MAT_" + fdir + "\n" + face;
                //File.WriteAllText(dc + fdir + Path.DirectorySeparatorChar + "PART_" + i.ToString("D3") + ".obj", obj);
            }
            RootNode.ResolveFaceTextures(Textures);
            RootNode.GenerateTree(null);

            //mtl += "#Export from PZ Tool\n" +
            //            "newmtl MAT_" + fdir + "\n" +
            //            "Ns 225.000000\nKa 1.000000 1.000000 1.000000\nKd 0.800000 0.800000 0.800000\nKs 0.000000 0.000000 0.000000\nKe 0.000000 0.000000 0.000000\nNi 1.450000\nd 1.000000\nillum 1\n";
            //mtl += "map_Kd " + fdir + ".bmp";
            //File.WriteAllText(dc + fdir + Path.DirectorySeparatorChar + "MAT_" + fdir + ".mtl", mtl);
            //getDCTexture(filepath);
        }

        protected override void _Write(BinaryWriter writer)
        {
            throw new Exception("Not implemented yet!");
        }

    }
}

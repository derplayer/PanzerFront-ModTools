//using ShenmueDKSharp.Files.Models._NTD;
using ShenmueDKSharp.Files.Images;
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
            int tFactor = 10000;

            ModelVersion = reader.ReadInt16(); //model ver
            PartCount = reader.ReadUInt16();

            for (uint i = 0; i < PartCount; i++)
            {
                ModelNode PartNode = new ModelNode();
                PartNode.ID = i;

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

                        Vector3 vertex = new Vector3(x, y, z);
                        PartNode.VertexPositions.Add(vertex);
                    }

                    MeshFace currentFace = new MeshFace();

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

                    // Normal UV
                    //Vector2 uv0 = new Vector2(ux0, uy0);
                    //Vector2 uv1 = new Vector2(ux1, uy1);
                    //Vector2 uv2 = new Vector2(ux2, uy2);
                    //Vector2 uv3 = new Vector2(ux3, uy3);

                    // Y-Flip UV
                    Vector2 uv0 = new Vector2(ux0, 1.0f - uy0);
                    Vector2 uv1 = new Vector2(ux1, 1.0f - uy1);
                    Vector2 uv2 = new Vector2(ux2, 1.0f - uy2);
                    Vector2 uv3 = new Vector2(ux3, 1.0f - uy3);

                    PartNode.VertexUVs.Add(uv0);
                    PartNode.VertexUVs.Add(uv1);
                    PartNode.VertexUVs.Add(uv2);
                    PartNode.VertexUVs.Add(uv3);
                }

                PartNode.Parent = RootNode;
                RootNode.Children.Add(PartNode);
            }

            //Add PVR Texture
            Texture tex = new Texture();
            string pvrTextPath = Path.ChangeExtension(this.FilePath, "pvr");
            tex.Image = new PVRT(pvrTextPath);

            Textures.Add(tex);
            RootNode.ResolveFaceTextures(Textures);

            //Camera visual stuff
            RootNode.Rotation.Y = 270;
            RootNode.Rotation.Z = 180;
            RootNode.Center.Z += 100;
        }

        protected override void _Write(BinaryWriter writer)
        {
            throw new Exception("Not implemented yet!");
        }

    }
}

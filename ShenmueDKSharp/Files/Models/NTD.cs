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

        public ushort UnknownParam;
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

            PartCount = reader.ReadUInt16(); //model ver
            UnknownParam = reader.ReadUInt16();

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
                        short x_pre = reader.ReadInt16();
                        short y_pre = reader.ReadInt16();
                        short z_pre = reader.ReadInt16();

                        float x = (float)(x_pre << 8) / tFactor;
                        float y = (float)(y_pre << 8) / tFactor;
                        float z = (float)(z_pre << 8) / tFactor;

                        Vector3 vertex = new Vector3(x, y, z);
                        PartNode.VertexPositions.Add(vertex);
                    }

                    MeshFace currentFace = new MeshFace();
                    currentFace.Type = MeshFace.PrimitiveType.Quads;

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

                    // debug only, remove later
                    float bux0 = (float)reader.ReadByte();
                    float bux1 = (float)reader.ReadByte();
                    float bux2 = (float)reader.ReadByte();
                    float bux3 = (float)reader.ReadByte();
                    float buy0 = (float)reader.ReadByte();
                    float buy1 = (float)reader.ReadByte();
                    float buy2 = (float)reader.ReadByte();
                    float buy3 = (float)reader.ReadByte();

                    float ux0 = (float)bux0 / (256 - 1);
                    float ux1 = (float)bux1 / (256 - 1);
                    float ux2 = (float)bux2 / (256 - 1);
                    float ux3 = (float)bux3 / (256 - 1);
                    float uy0 = (float)buy0 / (256 - 1); //1f - (float)buy0 / (256 - 1) - uv flip?
                    float uy1 = (float)buy1 / (256 - 1); //1f - (float)buy1 / (256 - 1) - uv flip?
                    float uy2 = (float)buy2 / (256 - 1); //1f - (float)buy2 / (256 - 1) - uv flip?
                    float uy3 = (float)buy3 / (256 - 1); //1f - (float)buy3 / (256 - 1) - uv flip?

                    // Normal UV
                    Vector2 uv0 = new Vector2(ux0, uy0);
                    Vector2 uv1 = new Vector2(ux1, uy1);
                    Vector2 uv2 = new Vector2(ux2, uy2);
                    Vector2 uv3 = new Vector2(ux3, uy3);

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
            //RootNode.Rotation.Y = 270;
            //RootNode.Rotation.Z = 180;
            //RootNode.Center.Z += 100;
        }

        protected override void _Write(BinaryWriter writer)
        {
            //Reset Camera visual stuff
            //RootNode.Rotation.Y = 0;
            //RootNode.Rotation.Z = 0;
            //RootNode.Center.Z -= 100;

            int tFactor = 10000;

            writer.Write(PartCount);
            writer.Write(UnknownParam);

            for (uint i = 0; i < PartCount; i++)
            {
                ModelNode PartNode = RootNode.Children[(int)i];
                PartNode.ID = i;

                int blockCount = (PartNode.Faces.Count);
                writer.Write((uint)blockCount);

                if (blockCount == 0) break;
                int kCache = 0;

                for (int j = 0; j < blockCount; j++)
                {
                    // quad vertex
                    for (int k = 0; k < 4; k++)
                    {
                        Vector3 vertex = PartNode.VertexPositions[(j * k)];

                        short x_pre = (short)(((int)(vertex.X * tFactor)) >> 8);
                        short y_pre = (short)(((int)(vertex.Y * tFactor)) >> 8);
                        short z_pre = (short)(((int)(vertex.Z * tFactor)) >> 8);

                        writer.Write(x_pre);
                        writer.Write(y_pre);
                        writer.Write(z_pre);
                        kCache++;
                    }

                    Vector2 uv0 = PartNode.VertexUVs[kCache-4];
                    Vector2 uv1 = PartNode.VertexUVs[kCache-3];
                    Vector2 uv2 = PartNode.VertexUVs[kCache-2];
                    Vector2 uv3 = PartNode.VertexUVs[kCache-1];

                    byte ux0 = (byte)(uv0.X * (256 - 1));
                    byte ux1 = (byte)(uv1.X * (256 - 1));
                    byte ux2 = (byte)(uv2.X * (256 - 1));
                    byte ux3 = (byte)(uv3.X * (256 - 1));
                    byte uy0 = (byte)(uv0.Y * (256 - 1));
                    byte uy1 = (byte)(uv1.Y * (256 - 1));
                    byte uy2 = (byte)(uv2.Y * (256 - 1));
                    byte uy3 = (byte)(uv3.Y * (256 - 1));

                    writer.Write(ux0);
                    writer.Write(ux1);
                    writer.Write(ux2);
                    writer.Write(ux3);

                    writer.Write(uy0);
                    writer.Write(uy1);
                    writer.Write(uy2);
                    writer.Write(uy3);
                    return;
                }
            }
        }

    }
}

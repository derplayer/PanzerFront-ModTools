using ShenmueDKSharp.Files.Images;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenmueDKSharp.Files.Models._OBJ
{
    /// <summary>
    /// Wavefront OBJ format
    /// </summary>
    public class MTL : BaseFile
    {
        public static bool EnableBuffering = false;
        public override bool BufferingEnabled => EnableBuffering;

        public List<Texture> Textures = new List<Texture>();
        public List<string> MaterialNames = new List<string>();

        public MTL(List<Texture> textures)
        {
            Textures = textures;
        }
        public MTL(string filepath)
        {
            Read(filepath);
        }
        public MTL(Stream stream)
        {
            Read(stream);
        }
        public MTL(BinaryReader reader)
        {
            Read(reader);
        }

        protected override void _Read(BinaryReader reader)
        {
            string[] lines = reader.ReadToEnd().Split('\n');

            Textures.Clear();
            Texture currentTexture = null;

            foreach (string line in lines)
            {
                if (line.StartsWith("newmtl "))
                {
                    string[] values = line.Split(' ');
                    if (values.Length != 2) continue;

                    MaterialNames.Add(values[1]);
                    if (currentTexture != null)
                    {
                        Textures.Add(currentTexture);
                    }
                    currentTexture = new Texture();
                }
                else if (line.StartsWith("map_Kd "))
                {
                    string[] values = line.Split(' ');
                    if (values.Length != 2) continue;

                    string diffusePath = "";
                    string dir = Path.GetDirectoryName(FilePath);
                    diffusePath = String.Format("{0}\\{1}", Path.GetDirectoryName(FilePath), values[1]);

                    string extension = Path.GetExtension(diffusePath);
                    Type fileType = FileHelper.GetFileTypeFromExtension(extension.Substring(1, extension.Length - 1).ToUpper());
                    currentTexture.Image = (BaseImage)Activator.CreateInstance(fileType, new object[] { diffusePath });
                }
            }
            Textures.Add(currentTexture);
        }

        protected override void _Write(BinaryWriter writer)
        {
            writer.WriteASCII("# MTL Generated by ShenmueDKSharp\n");
            for (int i = 0; i < Textures.Count; i++)
            {
                Texture texture = Textures[i];
                if (texture == null || texture.TextureID == null) continue;
                string name = texture.TextureID.Data.ToString("x16");
                string materialName = String.Format("mat_{0}\n", name);
                MaterialNames.Add(materialName);
                writer.WriteASCII(String.Format("newmtl {0}\n", materialName));

                writer.WriteASCII("Ns 0.000000\n");
                writer.WriteASCII("Ka 1.000000 1.000000 1.000000\n");
                writer.WriteASCII("Kd 0.800000 0.800000 0.800000\n");
                writer.WriteASCII("Ks 0.200000 0.200000 0.200000\n");
                writer.WriteASCII("Ke 0.0 0.0 0.0\n");
                writer.WriteASCII("Ni 1.450000\n");
                writer.WriteASCII("d 1.000000\n");
                writer.WriteASCII("illum 1\n");

                string textureName = String.Format("tex_{0}.png", name);
                if (String.IsNullOrEmpty(FilePath))
                {
                    //TODO: Make this somehow better
                    throw new ArgumentException("Filepath was not given.");
                }

                string texturePath = "";
                string dir = Path.GetDirectoryName(FilePath);
                if (String.IsNullOrEmpty(dir) || dir == "\\")
                {
                    texturePath = textureName;
                }
                else
                {
                    texturePath = String.Format("{0}\\{1}", Path.GetDirectoryName(FilePath), textureName);
                    string texDir = Path.GetDirectoryName(texturePath);
                    if (!String.IsNullOrEmpty(texDir))
                    {
                        if (!Directory.Exists(texDir))
                        {
                            Directory.CreateDirectory(texDir);
                        }
                    }
                }

                Bitmap bmp = texture.Image.CreateBitmap();
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(texturePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        bmp.Save(memory, ImageFormat.Png);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
                writer.WriteASCII(String.Format("map_Kd {0}\n\n", textureName));
            }
        }

        public int GetMaterialTextureIndex(string materialName)
        {
            for (int i = 0; i < MaterialNames.Count; i++)
            {
                if (MaterialNames[i] == materialName)
                {
                    return i;
                }
            }
            return -1;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

public class Tex
{
    const short HEADER_SIGN = 0x4D42;
    const int HEADER_RESERVED = 0x00000000;
    const int HEADER_LENGTH = 14;
    const short INFO_HEADER_PLANES = 0x0001;
    const short INFO_HEADER_BITCOUNT = 0x0008;
    const int INFO_HEADER_COMPRESSION = 0x00000000;
    const int INFO_HEADER_COLORSIMPORTANT = 0x00000000;
    const int INFO_HEADER_IMAGESIZE = 0x00000000;
    const int INFO_HEADER_HOR = 0x00000000;
    const int INFO_HEADER_VER = 0x00000000;
    const int INFO_HEADER_LENGTH = 40;

    public static void TKD3_tex_All(string filepath)
    {
        int palSize = 64;
        int texWidth = 256;
        int texHeight = 128;
        int number = 0;

        string pdir = Directory.GetParent(filepath).FullName;
        string fdir = Path.GetFileNameWithoutExtension(filepath);
        string outDir = "PS1_textures";

        if (!Directory.Exists(outDir))
        {
            Directory.CreateDirectory(outDir);
        }

        BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open));

        while (reader.BaseStream.Position!=reader.BaseStream.Length)
        {
            reader.ReadBytes(0x4060); //model block
            reader.ReadBytes(0x4); //null skip

            List<byte> textureData = new List<byte>();
            List<byte> texturePalette = new List<byte>();

            //Read texture pixel data
            for (int i = 0; i < texHeight; i++)
            {
                for (int j = 0; j < texWidth; j++)
                {
                    byte tmp = reader.ReadByte();
                    textureData.Add(tmp);
                }
            }
            //Read palette data
            for (int j = 0; j < palSize; j++)
            {
                ushort color16 = reader.ReadUInt16();
                byte r8 = (byte)((color16 & 0x1F) << 3);
                byte g8 = (byte)(((color16 >> 5) & 0x1F) << 3);
                byte b8 = (byte)(((color16 >> 10) & 0x1F) << 3);

                texturePalette.Add(b8);
                texturePalette.Add(g8);
                texturePalette.Add(r8);
                texturePalette.Add(0xff);
            }
            //Other data
            reader.ReadBytes(0x157f); //cfg block
            reader.ReadBytes(0x1420); //tim1
            reader.ReadBytes(0x1420); //tim2
            reader.ReadByte(); //end flag

            //Image formation
            int exPix = texWidth % 4 == 0 ? 0 : 4 - texWidth % 4;

            List<byte> bitmap = new List<byte>();
            //Header
            bitmap.AddRange(BitConverter.GetBytes(HEADER_SIGN));
            bitmap.AddRange(BitConverter.GetBytes(GetFileSize(texWidth, texHeight, exPix)));
            bitmap.AddRange(BitConverter.GetBytes(HEADER_RESERVED));
            bitmap.AddRange(BitConverter.GetBytes(GetImageDataOffset()));
            //INFO_HEADER
            bitmap.AddRange(BitConverter.GetBytes((int)INFO_HEADER_LENGTH));
            bitmap.AddRange(BitConverter.GetBytes(texWidth));
            bitmap.AddRange(BitConverter.GetBytes(texHeight));
            bitmap.AddRange(BitConverter.GetBytes(INFO_HEADER_PLANES));
            bitmap.AddRange(BitConverter.GetBytes(INFO_HEADER_BITCOUNT));
            bitmap.AddRange(BitConverter.GetBytes(INFO_HEADER_COMPRESSION));
            bitmap.AddRange(BitConverter.GetBytes(INFO_HEADER_IMAGESIZE));
            bitmap.AddRange(BitConverter.GetBytes(INFO_HEADER_HOR));
            bitmap.AddRange(BitConverter.GetBytes(INFO_HEADER_VER));
            bitmap.AddRange(BitConverter.GetBytes(palSize));
            bitmap.AddRange(BitConverter.GetBytes(INFO_HEADER_COLORSIMPORTANT));
            //Palette
            bitmap.AddRange(texturePalette.ToArray());
            //Pixel Data
            bitmap.AddRange(textureData.ToArray().Reverse());
            //Tex save
            File.WriteAllBytes(outDir + Path.DirectorySeparatorChar + "TEX_"+number.ToString("D3")+".bmp", bitmap.ToArray());
            number++;
        }
        reader.Close();
    }

    public static void textureExtract(string filepath, int texWidth, int texHeight,long texOffset)
    {
        int palSize = 64;
        List<byte> textureData = new List<byte>();
        List<byte> texturePalette = new List<byte>();
        string pdir = Directory.GetParent(filepath).FullName;

        using (BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open)))
        {
            //Read texture pixel data
            reader.BaseStream.Seek(texOffset, SeekOrigin.Begin);
            for (int i = 0; i < texHeight; i++)
            {
                for (int j = 0; j < texWidth; j++)
                {
                    byte tmp=reader.ReadByte();
                    textureData.Add(tmp);
                }
            }

            //Read palette data
            //reader.BaseStream.Seek(palOffset, SeekOrigin.Begin);
            for (int j = 0; j < palSize; j++)
            {
                ushort color16 = reader.ReadUInt16();
                byte r8 = (byte)((color16 & 0x1F) << 3);
                byte g8 = (byte)(((color16 >> 5) & 0x1F) << 3);
                byte b8 = (byte)(((color16 >> 10) & 0x1F) << 3);

                //texturePalette.Add(Color.FromArgb(r8, g8, b8));
                texturePalette.Add(b8);
                texturePalette.Add(g8);
                texturePalette.Add(r8);
                texturePalette.Add(0xff);
            }
        }

        //Image formation
        int exPix = texWidth % 4 == 0 ? 0 : 4 - texWidth % 4;

        List<byte> bitmap = new List<byte>();
        //Header
        bitmap.AddRange(BitConverter.GetBytes(HEADER_SIGN));
        bitmap.AddRange(BitConverter.GetBytes(GetFileSize(texWidth, texHeight, exPix)));
        bitmap.AddRange(BitConverter.GetBytes(HEADER_RESERVED));
        bitmap.AddRange(BitConverter.GetBytes(GetImageDataOffset()));
        //INFO_HEADER
        bitmap.AddRange(BitConverter.GetBytes((int)INFO_HEADER_LENGTH));
        bitmap.AddRange(BitConverter.GetBytes(texWidth));
        bitmap.AddRange(BitConverter.GetBytes(texHeight));
        bitmap.AddRange(BitConverter.GetBytes(INFO_HEADER_PLANES));
        bitmap.AddRange(BitConverter.GetBytes(INFO_HEADER_BITCOUNT));
        bitmap.AddRange(BitConverter.GetBytes(INFO_HEADER_COMPRESSION));
        bitmap.AddRange(BitConverter.GetBytes(INFO_HEADER_IMAGESIZE));
        bitmap.AddRange(BitConverter.GetBytes(INFO_HEADER_HOR));
        bitmap.AddRange(BitConverter.GetBytes(INFO_HEADER_VER));
        bitmap.AddRange(BitConverter.GetBytes(palSize));
        bitmap.AddRange(BitConverter.GetBytes(INFO_HEADER_COLORSIMPORTANT));
        //Palette
        bitmap.AddRange(texturePalette.ToArray());
        //Pixel Data
        bitmap.AddRange(textureData.ToArray().Reverse());
        //Tex save
        File.WriteAllBytes(pdir + Path.DirectorySeparatorChar + "tank_tex.bmp", bitmap.ToArray());
    }

    private static int GetImageDataOffset()
    {
        return HEADER_LENGTH + INFO_HEADER_LENGTH + 64 * 4;
    }

    private static int GetFileSize(int wid, int hei, int eP)
    {
        return GetImageDataOffset() + hei * (wid + eP);
    }

    public static void getDCTexture(string filepath)
    {
        string outDir = "DC_textures";
        string pdir = Directory.GetParent(filepath).Name;
        string fdir = Path.GetFileNameWithoutExtension(filepath);
        string ndir = Directory.GetParent(filepath).FullName;
        string[] dirs = Directory.GetFiles(@""+ ndir, "*.pvr");

        if (!Directory.Exists(outDir))
        {
            Directory.CreateDirectory(outDir);

            Process ToBMP;
            string converter = @"conv.exe";
            byte fl = 1;

            for (int i = 0; i < dirs.Length; i++)
            {
                if (fl == 1)
                {
                    fl = 0;
                    ToBMP = new Process();
                    ToBMP.StartInfo.FileName = @"cmd.exe";
                    ToBMP.StartInfo.Arguments = @"/C " + converter + " " + pdir + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(dirs[i]) + ".PVR ";
                    ToBMP.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    ToBMP.StartInfo.UseShellExecute = false;
                    ToBMP.StartInfo.CreateNoWindow = true;
                    ToBMP.Start();
                    ToBMP.WaitForExit();
                    fl = 1;
                }
                if (fl == 1) File.Move(@"" + Path.GetFileNameWithoutExtension(dirs[i]) + ".bmp", @"" + outDir + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(dirs[i]) + ".bmp");
            }
        }
        else
        {
        }
    }
}


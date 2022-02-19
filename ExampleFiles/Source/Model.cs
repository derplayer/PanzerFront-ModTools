using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

public class Model
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

    public static bool onPS1;
    public static bool onDC;
    public static bool onTex;

    public static void exportPS1(string filepath)
    {
        int tFactor = 10000;
        string ps = "PS1_models" + Path.DirectorySeparatorChar + "TANK_";
        string obj = "#Export from PZ Tool\n";
        string vert = "";
        string face = "";
        string uvs = "";
        string mtl = "";
        int modelCount = calcModels(filepath);

        using (BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open)))
        {
            long modelRet = 0;

            for (int e = 0; e < modelCount; e++)
            {
                mtl = "";
                if (!Directory.Exists(ps+e.ToString("D3")))
                {
                    Directory.CreateDirectory(ps + e.ToString("D3"));
                }
                modelRet = reader.BaseStream.Position;
                reader.ReadInt16(); //model ver
                int partNum = reader.ReadUInt16();

                for (int i = 0; i < partNum; i++)
                {

                    obj = "#Export from PZ Tool\no PART_" + i.ToString("D2") + "\n";
                    vert = "";
                    face = "";
                    uvs = "";
                    int c = 1;
                    uint blockCount = reader.ReadUInt32();

                    if (blockCount == 0)
                    {
                        reader.BaseStream.Seek(modelRet + 0x4060, SeekOrigin.Begin);
                        break;
                    }

                    for (int j = 0; j < blockCount; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            float x = (float)(reader.ReadInt16() << 8) / tFactor; ;
                            float y = (float)(reader.ReadInt16() << 8) / tFactor; ;
                            float z = (float)(reader.ReadInt16() << 8) / tFactor; ;
                            vert += "v " + x.ToString() + " " + y.ToString() + " " + z.ToString() + "\n";
                        }

                        face += "f "
                            + (c).ToString() + "/" + (c).ToString()
                            + " " + (c + 2).ToString() + "/" + (c + 2).ToString()
                            + " " + (c + 3).ToString() + "/" + (c + 3).ToString()
                            + " " + (c + 1).ToString() + "/" + (c + 1).ToString() + "\n";
                        c += 4;

                        float ux0 = 1f - (float)reader.ReadByte() / (256 - 1);
                        float ux1 = 1f - (float)reader.ReadByte() / (256 - 1);
                        float ux2 = 1f - (float)reader.ReadByte() / (256 - 1);
                        float ux3 = 1f - (float)reader.ReadByte() / (256 - 1);
                        float uy0 = 1f - (float)reader.ReadByte() / (256 - 1);
                        float uy1 = 1f - (float)reader.ReadByte() / (256 - 1);
                        float uy2 = 1f - (float)reader.ReadByte() / (256 - 1);
                        float uy3 = 1f - (float)reader.ReadByte() / (256 - 1);

                        uvs += "vt " + ux0.ToString() + " " + uy0.ToString() + "\n";
                        uvs += "vt " + ux1.ToString() + " " + uy1.ToString() + "\n";
                        uvs += "vt " + ux2.ToString() + " " + uy2.ToString() + "\n";
                        uvs += "vt " + ux3.ToString() + " " + uy3.ToString() + "\n";
                    }
                    obj += "mtllib MAT_"+ e.ToString("D3") + ".mtl\n"+vert + uvs + "usemtl MAT_"+ e.ToString("D3")+"\n" + face;
                    File.WriteAllText(ps + e.ToString("D3")+Path.DirectorySeparatorChar+"PART_" + i.ToString("D3") + ".obj", obj);                 
                }
                mtl += "#Export from PZ Tool\n" +
                        "newmtl MAT_" + e.ToString("D3") + "\n" +
                        "Ns 225.000000\nKa 1.000000 1.000000 1.000000\nKd 0.800000 0.800000 0.800000\nKs 0.000000 0.000000 0.000000\nKe 0.000000 0.000000 0.000000\nNi 1.450000\nd 1.000000\nillum 1\n";
                mtl += "map_Kd TEX_" + e.ToString("D3")+".bmp";
                File.WriteAllText(ps + e.ToString("D3") + Path.DirectorySeparatorChar + "MAT_" + e.ToString("D3") + ".mtl", mtl);
                getPS1Texture(reader, ps + e.ToString("D3"), e);
            }
        }
    }

    public static void exportDC(string filepath)
    {
        string fdir = Path.GetFileNameWithoutExtension(filepath);
        int tFactor = 10000;
        string dc = "DC_models" + Path.DirectorySeparatorChar;
        string obj = "#Export from PZ Tool\n";
        string vert = "";
        string face = "";
        string uvs = "";
        string mtl = "";

        if (!Directory.Exists(dc + fdir))
        {
            Directory.CreateDirectory(dc + fdir);
        }

        using (BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open)))
        {
            reader.ReadInt16(); //model ver
            int partCount = reader.ReadUInt16();

            for (int i = 0; i < partCount; i++)
            {
                obj = "#Export from PZ Tool\no PART_" + i.ToString("D3") + "\n";
                vert = "";
                face = "";
                uvs = "";
                int c = 1;
                uint blockCount = reader.ReadUInt32();
                if (blockCount == 0) break;

                for (int j = 0; j < blockCount; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        float x = (float)(reader.ReadInt16() << 8) / tFactor; ;
                        float y = (float)(reader.ReadInt16() << 8) / tFactor; ;
                        float z = (float)(reader.ReadInt16() << 8) / tFactor; ;
                        vert += "v " + x.ToString() + " " + y.ToString() + " " + z.ToString() + "\n";
                    }

                    face += "f "
                        + (c).ToString() + "/" + (c).ToString()
                        + " " + (c + 2).ToString() + "/" + (c + 2).ToString()
                        + " " + (c + 3).ToString() + "/" + (c + 3).ToString()
                        + " " + (c + 1).ToString() + "/" + (c + 1).ToString() + "\n";
                    c += 4;

                    float ux0 = (float)reader.ReadByte() / (256 - 1);
                    float ux1 = (float)reader.ReadByte() / (256 - 1);
                    float ux2 = (float)reader.ReadByte() / (256 - 1);
                    float ux3 = (float)reader.ReadByte() / (256 - 1);
                    float uy0 = 1f - (float)reader.ReadByte() / (256 - 1);
                    float uy1 = 1f - (float)reader.ReadByte() / (256 - 1);
                    float uy2 = 1f - (float)reader.ReadByte() / (256 - 1);
                    float uy3 = 1f - (float)reader.ReadByte() / (256 - 1);

                    uvs += "vt " + ux0.ToString() + " " + uy0.ToString() + "\n";
                    uvs += "vt " + ux1.ToString() + " " + uy1.ToString() + "\n";
                    uvs += "vt " + ux2.ToString() + " " + uy2.ToString() + "\n";
                    uvs += "vt " + ux3.ToString() + " " + uy3.ToString() + "\n";
                }
                obj += "mtllib MAT_" + fdir + ".mtl\n"+vert + uvs + "usemtl MAT_" + fdir + "\n" + face;
                File.WriteAllText(dc+fdir+ Path.DirectorySeparatorChar + "PART_" + i.ToString("D3") + ".obj", obj);
            }

            mtl += "#Export from PZ Tool\n" +
                        "newmtl MAT_" + fdir + "\n" +
                        "Ns 225.000000\nKa 1.000000 1.000000 1.000000\nKd 0.800000 0.800000 0.800000\nKs 0.000000 0.000000 0.000000\nKe 0.000000 0.000000 0.000000\nNi 1.450000\nd 1.000000\nillum 1\n";
            mtl += "map_Kd " + fdir + ".bmp";
            File.WriteAllText(dc + fdir + Path.DirectorySeparatorChar + "MAT_" + fdir + ".mtl", mtl);
            getDCTexture(filepath);
        }
    }

    public static int calcModels(string filepath)
    {
        int temp = 0;
        BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open));

        while (reader.BaseStream.Position != reader.BaseStream.Length)
        {
            reader.ReadBytes(0x4060);
            reader.ReadBytes(0x8004);
            reader.ReadBytes(0x80);
            reader.ReadBytes(0x157f);
            reader.ReadBytes(0x1420);
            reader.ReadBytes(0x1420);
            reader.ReadByte();
            temp++;
        }

        reader.Close();

        return temp;
    }

    private static void getPS1Texture(BinaryReader reader, string outDir, int number)
    {
        int palSize = 64;
        int texWidth = 256;
        int texHeight = 128;

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
        File.WriteAllBytes(outDir + Path.DirectorySeparatorChar + "TEX_" + number.ToString("D3") + ".bmp", bitmap.ToArray());
    }

    public static void getDCTexture(string filepath)
    {
        string dc = "DC_models" + Path.DirectorySeparatorChar;
        string pdir = Directory.GetParent(filepath).Name;
        string fdir = Path.GetFileNameWithoutExtension(filepath);
        string ndir = Directory.GetParent(filepath).FullName;
        string outDir = dc+fdir;

        if (!File.Exists(outDir+Path.DirectorySeparatorChar+fdir+".bmp"))
        {
            Process ToBMP;
            string converter = @"conv.exe";

            ToBMP = new Process();
            ToBMP.StartInfo.FileName = @"cmd.exe";
            ToBMP.StartInfo.Arguments = @"/C " + converter + " " + pdir + Path.DirectorySeparatorChar + fdir + ".PVR ";
            ToBMP.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ToBMP.StartInfo.UseShellExecute = false;
            ToBMP.StartInfo.CreateNoWindow = true;
            ToBMP.Start();
            ToBMP.WaitForExit();
            File.Move(@"" + fdir + ".bmp", @"" + outDir + Path.DirectorySeparatorChar + fdir + ".bmp");
        }
    }

    private static int GetImageDataOffset()
    {
        return HEADER_LENGTH + INFO_HEADER_LENGTH + 64 * 4;
    }

    private static int GetFileSize(int wid, int hei, int eP)
    {
        return GetImageDataOffset() + hei * (wid + eP);
    }
}


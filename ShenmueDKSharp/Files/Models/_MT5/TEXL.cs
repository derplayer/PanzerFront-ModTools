﻿using ShenmueDKSharp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenmueDKSharp.Files.Models._MT5
{
    public class TEXL
    {
        public readonly static List<byte[]> Identifiers = new List<byte[]>()
        {
            new byte[4] { 0x54, 0x45, 0x58, 0x4C } //TEXL
        };

        public static bool IsValid(uint identifier)
        {
            return IsValid(BitConverter.GetBytes(identifier));
        }

        public static bool IsValid(byte[] identifier)
        {
            for (int i = 0; i < Identifiers.Count; i++)
            {
                if (FileHelper.CompareSignature(Identifiers[i], identifier)) return true;
            }
            return false;
        }

        public uint Offset;

        public uint Identifier;
        public uint Size;
        public uint Unknown;
        public uint TextureCount;

        public TEXL(BinaryReader reader)
        {
            Offset = (uint)reader.BaseStream.Position;

            Identifier = reader.ReadUInt32();
            Size = reader.ReadUInt32();

            //Unknown offset?
            Unknown = reader.ReadUInt32();

            TextureCount = reader.ReadUInt32();

            reader.BaseStream.Seek(Offset + Size, SeekOrigin.Begin);
        }
    }
}

using ShenmueDKSharp.Utils;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenmueDKSharp.Files.Misc
{
    /// <summary>
    /// Scene audio/subtitles stuff
    /// </summary>
    public class SCNF : BaseFile
    {
        public static bool EnableBuffering = true;
        public override bool BufferingEnabled => EnableBuffering;

        public readonly static List<byte[]> Identifiers = new List<byte[]>()
        {
            new byte[4] { 0x53, 0x43, 0x4E, 0x46 } //SCNF
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

        public uint Identifier;

        public SCNF() { }

        protected override void _Read(BinaryReader reader)
        {
            Identifier = reader.ReadUInt32();

        }

        protected override void _Write(BinaryWriter writer)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenmueDKSharp.Files.Tokens
{
    /// <summary>
    /// SCRL Token.
    /// Unknown 4 byte value.
    /// </summary>
    public class SCRL : BaseToken
    {
        public static readonly string Identifier = "SCRL";

        public UInt32 Value;

        protected override void _Read(BinaryReader reader)
        {
            Value = reader.ReadUInt32();
        }

        protected override void _Write(BinaryWriter writer)
        {
            writer.Write(Content);
        }
    }
}

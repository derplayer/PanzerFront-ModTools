using System.Collections.Generic;

namespace ShenmueDKSharp.Files.Images._PVRT.WQuantizer
{
    public class LookupData
    {
        public LookupData(int granularity)
        {
            Lookups = new List<Lookup>();
            Tags = new int[granularity, granularity, granularity, granularity];
        }

        public IList<Lookup> Lookups { get; private set; }
        public int[, , ,] Tags { get; private set; }
    }
}
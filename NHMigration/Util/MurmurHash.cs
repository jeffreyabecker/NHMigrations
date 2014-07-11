using System;
using System.Linq;
using System.Text;

namespace NHMigration.Util
{
    public static class MurmurHash
    {
        const uint c1 = 0xcc9e2d51;
        const uint c2 = 0x1b873593;
        const int r1 = 15;
        const int r2 = 13;
        const uint m = 5;
        const uint n = 0xe6546b64;

        private static uint HashPart(uint hash, byte[] part)
        {
            if (part.Length == 4)
            {
                var k = BitConverter.ToUInt32(part, 0);
                k = k* m;
                k = (k << r1) | (k >> (32-r1));
                k  = k * c2;
                hash = hash ^ k;
                hash = (hash << r2) | (hash >> (32 - r2));
                hash = (hash*m) + n;
                return hash;
            }
            part = part.Concat(new byte[] {0x0, 0x0, 0x0, 0x0,}).Take(4).ToArray();
            var remainingBytes = BitConverter.ToUInt32(part, 0);
            remainingBytes = remainingBytes*c1;
            remainingBytes = (remainingBytes << r1) | (remainingBytes >> (32 - r1));
            remainingBytes = remainingBytes*c2;
            hash = hash ^ remainingBytes;
            return hash;
        }

        public static uint Hash(string data)
        {
            var bytes = Encoding.Unicode.GetBytes(data);
            return Hash(bytes, (uint)0xc58f1a7b);
        }
        public static uint Hash(byte[] data, uint seed)
        {
            
            var len = (uint)data.Length;
            var hash = data
                .Select((c, i) => new {Index = i, Data = c})
                .GroupBy(x => x.Index/4)
                .Select(x => x.Select(y => y.Data).ToArray())
                .Aggregate(seed, HashPart);

            hash = hash ^ len;
            hash = hash ^ (hash >> 16);
            hash = hash*0x85ebca6b;
            hash = hash ^ (hash >> 13);
            hash = hash*0xc2b2ae35;
            hash = hash ^ (hash >> 16);
            return hash;
        }

    }
}

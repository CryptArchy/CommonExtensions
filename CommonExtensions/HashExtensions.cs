using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonExtensions.HashExtensions
{
    public static class HashExt
    {
        public const int HASH_PRIME = 16777619;
        public const int HASH_OFFSET_BASIS = unchecked((int)2166136261);
        public const long LONG_HASH_PRIME = 1099511628211;
        public const long LONG_HASH_OFFSET_BASIS = unchecked((long)14695981039346656037);
        /* Hash_Size  |           FNV_Prime  |  FNV_Offset_Basis
         * --------------------------------------------
         *         32 |             16777619 |  2166136261
         *         64 |        1099511628211 |  14695981039346656037
         *        128 |  2**88 + 2**8 + 0x3b |  144066263297769815596495629667062367629
         *        256 | 2**168 + 2**8 + 0x63 |  100029257958052580907070968620625704837092796014241193945225284501741471925557
         *        512 | 2**344 + 2**8 + 0x57 |  9659303129496669498009435400716310466090418745672637896108374329434462657994582932197716438449813051892206539805784495328239340083876191928701583869517785
         *       1024 | 2**680 + 2**8 + 0x8d |  14197795064947621068722070641403218320880622795441933960878474914617582723252296732303717722150864096521202355549365628174669108571814760471015076148029755969804077320157692458563003215304957150157403644460363550505412711285966361610267868082893823963790439336411086884584107735010676915
        */

        public static int Hash(this object value)
        {
            return _Hash(HASH_OFFSET_BASIS, value.GetHashCode());
        }

        public static int Hash(this string value)
        {
            var hash = HASH_OFFSET_BASIS;
            for (var i = 0; i < value.Length; i++)
                hash = _Hash(hash, Convert.ToInt32(value[i]));
            return hash;
        }

        public static long LongHash(this object value)
        {
            return _Hash(LONG_HASH_OFFSET_BASIS, (long)value.GetHashCode());
        }

        public static long LongHash(this string value)
        {
            var hash = LONG_HASH_OFFSET_BASIS;
            for (var i = 0; i < value.Length; i++)
                hash = _Hash(hash, Convert.ToInt64(value[i]));
            return hash;
        }

        public static int Hash(this int value)
        {
            return _Hash(HASH_OFFSET_BASIS, value);
        }

        public static long Hash(this long value)
        {
            return _Hash(LONG_HASH_OFFSET_BASIS, value);
        }

        public static int Hash(this int basis, int value)
        {
            if (basis == 0) basis = HASH_OFFSET_BASIS;
            else if (basis == HASH_OFFSET_BASIS) basis = 0;

            return _Hash(basis, value);
        }

        public static long Hash(long basis, long value)
        {
            if (basis == 0) basis = LONG_HASH_OFFSET_BASIS;
            else if (basis == LONG_HASH_OFFSET_BASIS) basis = 0;

            return _Hash(basis, value);
        }

        public static int Hash(this object basis, params object[] values)
        {
            var hash = Hash(basis);
            for (int i = 0; i < values.Length; i++)
                hash = _Hash(hash, values[i].GetHashCode());
            return hash;
        }

        public static int Hash(this int basis, params int[] values)
        {
            if (basis == 0) basis = HASH_OFFSET_BASIS;
            else if (basis == HASH_OFFSET_BASIS) basis = 0;
            
            for (int i = 0; i < values.Length; i++)
                basis = _Hash(basis, values[i]);
            return basis;
        }

        public static long Hash(this long basis, params long[] values)
        {
            if (basis == 0) basis = LONG_HASH_OFFSET_BASIS;
            else if (basis == LONG_HASH_OFFSET_BASIS) basis = 0;

            for (int i = 0; i < values.Length; i++)
                basis = _Hash(basis, values[i]);
            return basis;
        }

        private static int _Hash(int basis, int value)
        {
            return (basis ^ value) * HASH_PRIME;
        }

        private static long _Hash(long basis, long value)
        {
            return (basis ^ value) * LONG_HASH_PRIME;
        }
    }
}

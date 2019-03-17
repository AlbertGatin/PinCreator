using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pin
{
    public static class Feistel
    {
        /// <summary>
        /// G преобразования (сеть Фейстеля)
        /// </summary>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        /// <param name="k3"></param>
        /// <param name="k4"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="s"></param>
        public static void GEncode(byte k1, byte k2, byte k3, byte k4, ref byte a, ref byte b, Substitutions s)
        {
            a = (byte)(((int)a + (int)s.S0[((int)b + (int)k1) % 100]) % 100);
            Helper.Swap(ref a, ref b);
            a = (byte)(((int)a + (int)s.S1[((int)b + (int)k2) % 100]) % 100);
            Helper.Swap(ref a, ref b);
            a = (byte)(((int)a + (int)s.S2[((int)b + (int)k3) % 100]) % 100);
            Helper.Swap(ref a, ref b);
            a = (byte)(((int)a + (int)s.S3[((int)b + (int)k4) % 100]) % 100);
        }

        /// <summary>
        /// Обратные G преобразования (сеть Фейстеля) 
        /// </summary>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        /// <param name="k3"></param>
        /// <param name="k4"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="s"></param>
        public static void GDecode(byte k1, byte k2, byte k3, byte k4, ref byte a, ref byte b, Substitutions s)
        {
            a += (byte)(100 - (int)s.S3[((int)b + (int)k4) % 100]);
            Helper.Swap(ref a, ref b);
            a += (byte)(100 - (int)s.S2[((int)b + (int)k3) % 100]);
            Helper.Swap(ref a, ref b);
            a += (byte)(100 - (int)s.S1[((int)b + (int)k2) % 100]);
            Helper.Swap(ref a, ref b);
            a += (byte)(100 - (int)s.S0[((int)b + (int)k1) % 100]);
        }
    }
}

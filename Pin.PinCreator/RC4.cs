using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Pin
{
    public class RC4
    {
     
        public RC4(byte[] key)
        {
            Init(key);
        }

        byte[] S = new byte[256];
        int iRC4 = 0;
        int jRC4 = 0;

        /// <summary>
        /// Инициализация RC4
        /// </summary>
        /// <param name="mk">Мастер ключ</param>
        private void Init(byte[] mk)
        {
            int keyLength = mk.Length;
            
            for (int i = 0; i < 256; i++)
            {
                S[i] = (byte)i;
            }

            int j = 0;

            for (int i = 0; i < 256; i++)
            {
                j = (j + S[i] + mk[i % keyLength]) % 256;
                S.Swap(i, j);
            }
        }

        /// <summary>
        /// Генератор псевдослучайной последовательности
        /// </summary>
        /// <returns>Байт ключевого потока</returns>
        public byte PseudiRandGenerator()
        {
            iRC4 = (iRC4 + 1) % 256;
            jRC4 = (jRC4 + S[iRC4]) % 256;

            S.Swap(iRC4, jRC4);
            int t = (S[iRC4] + S[jRC4]) % 256;
            return S[t];
        }

        /// <summary>
        /// Получение псевдослучайного байта в диапозоне
        /// </summary>
        /// <returns>Байт ключевого потока</returns>
        public byte RC4M(int i)
        {
            int k = 0;
            int j = i;
            while (j < 127)
            {
                k = k + 1;
                j = j << 1;
            }
            byte c;
            bool f = true;
            while (f)
            {
                c = PseudiRandGenerator();
                c = Convert.ToByte((Convert.ToInt32(c) << k >> k));
                if (Convert.ToInt32(c) <= i)
                {
                    f = false;
                    return c;
                }
            }
            return new byte();
        }
        public byte CreateSimpleByte()
        {
            byte k;
            bool itsSimple = false;
            while (!itsSimple)
            {
                k = RC4M(99);
                if (Helper.NOD((int)k, 100) == 1)
                    return k;
            }
            throw new Exception("Ошибка при генерации простого числа k");
        }
        /// <summary>
        /// Зашифровка
        /// </summary>
        /// <param name="inputStream">Входной поток</param>
        /// <param name="size">Размер зашифровываемых данных</param>
        /// <returns>Шифр</returns>
        public byte[] Encoder(byte[] inputStream, int size)
        {
            byte[] data = inputStream.Take(size).ToArray();
            byte[] cipher = new byte[data.Length];

            for(int m = 0; m<data.Length; m++)
            {
                cipher[m] = (byte)(data[m] ^ PseudiRandGenerator());
            }

            return cipher;
        }

        /// <summary>
        /// Расшифровка
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public byte[] Decoder(byte[] inputStream, int size)
        {
            return Encoder(inputStream, size);
        }
    }
}

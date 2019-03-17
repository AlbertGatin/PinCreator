using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Globalization;
using System.Xml;
using Pin.Properties;

namespace Pin
{
    /// <summary>
    /// Класс построения подстановок (использует датчик RC4)
    /// </summary>
    public class Substitutions
    {
        public Substitutions(byte[] key, int size)
        {
            Init(key, size);
        }
        public Substitutions(RC4 rc, int size)
        {
            Init(rc, size);
        }
        public Substitutions(RC4 rc)
        {
            int size = 100;
            Init(rc, size);
        }


        /// <summary>
        /// Датчик RC4
        /// </summary>
        RC4 RC4;
        public byte[] S0;
        public byte[] S2;
        public byte[] S1;
        public byte[] S3;

        /// <summary>
        /// Инициализация данных
        /// </summary>
        private void Init(RC4 rc, int size)
        {

            RC4 = rc;
            //this.S0 = BuildS0S2(size);
            //this.S2 = BuildS0S2(size);

            int writedCount = 0;
            if (!ReadSubsFromXml(ref this.S0, 0, size, ref writedCount))
            {
                this.S0 = BuildS0S2(size);
              //  WriteSubsToXml(this.S0, 0, size);
            }
            if (!ReadSubsFromXml(ref this.S2, 2, size, ref writedCount))
            {                  
                this.S2 = BuildS0S2Alternative(size);
               // WriteSubsToXml(this.S2, 2, size);
            }
            //if (!ReadSubsFromXml(ref this.S1, 1, size, ref writedCount) || writedCount < size)
            //{
            //    this.S1 = BuildS1(this.S1,size);
            //  //  WriteSubsToXml(this.S1, 1, size);
            //}
            if (!ReadSubsFromXml(ref this.S1, 1, size, ref writedCount))
            {
                this.S1 = BuildS1Alt(size);
              //  WriteSubsToXml(this.S1, 1, size);
            }
            if (!ReadSubsFromXml(ref this.S3, 3, size, ref writedCount))
            {
                this.S3 = BuildS3(S1);
                //WriteSubsToXml(this.S3, 3, size);
            }
        }

        /// <summary>
        /// Инициализация данных
        /// </summary>
        private void Init(byte[] key, int size)
        {
            //this.S0 = BuildS0S2(size);
            //this.S2 = BuildS0S2(size);
            Init(new RC4(key), size);
        }

        /// <summary>
        /// Алгаритм подстановки для S0 или S2
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private byte[] BuildS0S2Alternative(int size)
        {
            byte[] S = new byte[255];
            byte[] temp = new byte[255];
            for (int i = 0; i < 255; i++)
            {
                S[i] = (byte)i;
                temp[i] = (byte)i;
            }
            bool f = true;

            while (f)
            {

                for (int i = 0; i < S.Length; i++)
                {
                    byte k = RC4.RC4M(S.Length - 1);
                    byte m = RC4.RC4M(S.Length - 1);

                    S.Swap<byte>((int)k, (int)m);
                }
                if (S.IsChengePosition<byte>(temp))
                {
                    for (int i = S.Length - 1; i >= 0; i--)
                    {
                        if ((int)S[i] >= size)
                            S = S.RemoveAt(i);
                    }
                    f = false;
                }
            }

            return S;
        }

        /// <summary>
        /// Алгаритм подстановки для S0 или S2
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private byte[] BuildS0S2(int size)
        {
            byte[] S = new byte[size];
            for (int i = 0; i < size; i++)
            {
                S[i] = (byte)i;
            }
            byte[] R = new byte[size];
            for (int i = 0; i < size; i++)
            {
                byte c = size - 1 != i ? RC4.RC4M(size - 1 - i) : (byte)0;
                int index = (int)c;
                R[i] = S[index];
                S = S.RemoveAt(index);
            }

            return R;
        }

        /// <summary>
        /// Алгоритм подстановки на основе дискретных логорифмов (при решении в лоб)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private byte[] BuildS1(byte[] s, int size)
        {
             return EqSolutions(s, size);
            //return EqSolutionsAlt(s, size);
        }

        /// <summary>
        /// Алгоритм подстановки на основе дискретных логорифмов (использовать его)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private byte[] BuildS1Alt(int size)
        {
            // return EqSolutions(s, size);
            return EqSolutionsAlt(size);
        }

        /// <summary>
        /// Алгоритм подстановки для последовательности S3 (на основе решений дискретных логорифмов)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private byte[] BuildS3(byte[] x)
        {
            byte k1;
            byte k2;
            byte[] y = new byte[100];
            k1 = RC4.CreateSimpleByte();
            k2 = RC4.CreateSimpleByte();

            for (int i = 0; i < 100; i++)
            {
                y[i] = (byte)(((int)k1 * (int)x[i] + (int)k2) % 100);
            }

            return y;
        }

        

        /// <summary>
        /// Обратное решение(перебор корней и подстановка в полученные ячейки)
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private static byte[] EqSolutionsAlt(int size)
        {
            byte[] sol = new byte[size];
            //byte[] sol = new byte[size];
            for (int i = 100; i > 0; i--)
            {
                int index = (int)(BigInteger.Pow(2, i) % 101 - 1);
                int value = i % 100;
                sol[index] = (byte)value;

            }

            return sol;
        }

        /// <summary>
        /// Решение "в лоб"
        /// </summary>
        /// <param name="sol"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static byte[] EqSolutions(byte[] sol, int size)
        {
            //byte[] sol = new byte[size];
            //for (int i = 0; i < 100; i++)
            //{
            //    if (sol[i] == 0)
            //    {
            //        sol[i] = (byte)(SingleSolution(i) % 100);
            //        WriteElemOfSubsToXml(sol[i], 1, i);
            //    }

            //}
            Parallel.For(0, size-1, index => {
                if (sol[index] == (byte)0)
                {
                    sol[index] = (byte)(SingleSolution(index) % 100);
                    WriteElemOfSubsToXml(sol[index], 1, index);
                }
            });

            return sol;
        }

        /// <summary>
        /// получение элемента по индесу
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static int SingleSolution(int i)
        {
            BigInteger temp = new BigInteger();
            temp = 0;

            BigInteger compareValue = BigInteger.Pow(2, 100);
            int n = 0;
            while (temp <= compareValue)
            {

                temp = 101 * n + i + 1;
                int pow = In2Pow(temp);//(int)BigInteger.Log(temp, 2d);
                if (pow != -1)
                    return pow;
                n++;
            }

            throw new Exception(string.Format(CultureInfo.InvariantCulture, "Не найдено решение для i = {0}", i));
        }

        /// <summary>
        /// Проверка является ли число степенью числа 2
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static int In2Pow(BigInteger value)
        {
            for (int i = 1; i <= 100; i++)
            {
                if (value == BigInteger.Pow(2, i))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Чтение перестановок из xml (не использовать только на тестах)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="numofsub"></param>
        /// <param name="size"></param>
        /// <param name="readedCount"></param>
        /// <returns></returns>
        private static bool ReadSubsFromXml(ref byte[] s, int numofsub, int size, ref int readedCount)
        {
            try
            {
                readedCount = 0;

                XmlDocument doc = new XmlDocument();

                doc.LoadXml(Resources.repos);

                XmlNode root = doc.SelectSingleNode("Repos");
                if (root == null)
                    return false;

                XmlNode subs = root.SelectSingleNode("Substitutions");
                if (subs == null)
                    return false;

                XmlNode Ssubs = subs.SelectSingleNode("S" + numofsub + "Subs");
                if (Ssubs == null)
                    return false;

                XmlNodeList S = Ssubs.SelectNodes("S" + numofsub);
                if (S == null || S.Count <= 0)
                    return false;

                s = new byte[size];
                foreach (XmlNode item in S)
                {
                    int i = int.Parse(item.Attributes["i"].Value);
                    string value = item.InnerText;
                    if (i >= size || string.IsNullOrWhiteSpace(value))
                        return false;

                    s[i] = (byte)int.Parse(value);
                    readedCount++;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Запись перестановок в xml
        /// </summary>
        /// <param name="s"></param>
        /// <param name="numofsub"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static bool WriteSubsToXml(byte[] s, int numofsub, int size)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(Resources.repos);

                XmlNode root = doc.SelectSingleNode("Repos");
                if (root == null)
                    return false;

                XmlNode subs = root.SelectSingleNode("Substitutions");
                if (subs == null)
                    return false;

                XmlNode Ssubs = subs.SelectSingleNode("S" + numofsub + "Subs");
                if (Ssubs == null)
                    return false;

                for (int i = 0; i < size; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(CultureInfo.InvariantCulture, "<S{2} i=\"{0}\">{1}</S{2}>",
                        i, (int)s[i], numofsub);
                    Ssubs.InnerXml += sb.ToString();
                }
                doc.Save(@"C:\Users\Альберт\source\repos\PinCreator\Pin.PinCreator\Resourses\repos.xml");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Запись одного элемента(не использовать только на тестах)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="numofsub"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool WriteElemOfSubsToXml(byte s, int numofsub, int index)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                doc.Load(@"C:\Users\Альберт\source\repos\PinCreator\Pin.PinCreator\Resourses\repos.xml");

                XmlNode root = doc.SelectSingleNode("Repos");
                if (root == null)
                    return false;

                XmlNode subs = root.SelectSingleNode("Substitutions");
                if (subs == null)
                    return false;

                XmlNode Ssubs = subs.SelectSingleNode("S" + numofsub + "Subs");
                if (Ssubs == null)
                    return false;


                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(CultureInfo.InvariantCulture, "<S{2} i=\"{0}\">{1}</S{2}>",
                    index, (int)s, numofsub);
                Ssubs.InnerXml += sb.ToString();
                doc.Save(@"C:\Users\Альберт\source\repos\PinCreator\Pin.PinCreator\Resourses\repos.xml");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

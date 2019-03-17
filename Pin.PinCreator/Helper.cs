using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Pin
{
    public static class Helper
    {
        /// <summary>
        /// Проверка входных параметров для пина
        /// </summary>
        /// <param name="s"></param>
        /// <returns>результат проверки(true/false</returns>
        public static bool InputValidate(string s)
        {
            if (s.Length != 16)
                return false;

            if (!IsDigit(s))
                return false;

            return true;
        }

        /// <summary>
        /// Проверка строки на число
        /// </summary>
        /// <param name="s"></param>
        /// <returns>результат проверки(true/false)</returns>
        public static bool IsDigit(string s)
        {
            char[] c = s.ToCharArray();
            foreach (char item in c)
            {
                if (!char.IsDigit(item))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Сдвиг на 2 разряда вправо
        /// </summary>
        /// <param name="s">входной массив</param>
        /// <returns>результат</returns>
        public static T[] TwoElemToRight<T>(T[] s)
        {
            if (s.Length < 3)
                throw new Exception("Длина массива должна быть не меньше 3");
            T[] c = new T[s.Length]; //новый массив

            for (int i = 0; i < s.Length; i++)
            {
                c[i] = i < 2 ? s[s.Length - 2 + i] : s[i - 2];
            }
            return c;
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            var c = a;
            a = b;
            b = c;
        }

        public static void Swap<T>(this T[] array, int i, int j)
        {
            var temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            var result = new T[source.Length - 1];

            if (index > 0)
                Array.Copy(source, 0, result, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, result, index, source.Length - index - 1);

            return result;
        }      

        public static bool IsChengePosition<T>(this T[] array1, T[] array2)
        {
            int size = array1.Length;
            if (size != array2.Length)
                throw new Exception("Размеры массивов не равны");
            int repositionCount = 0;
            for (int i = 0; i < size; i++)
            {
                if (!array1[i].Equals(array2[i]))
                    repositionCount++;
            }

            return repositionCount == size;
        }

        public static int NOD(int m, int n)
        {
            int nod = 0;
            for (int i = 1; i < (n * m + 1); i++)
            {
                if (m % i == 0 && n % i == 0)
                {
                    nod = i;
                }
            }
            return nod;
        }

        /// <summary>
        /// Проверка равны ли матрицы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool Equal<T>(this T[,] A, T[,] B, int height, int width)
        {
            if (A.Length != B.Length || A.LongLength != B.LongLength)
                return false;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (A[i, j].Equals(B[i, j]))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Перемножение матриц
        /// </summary>
        /// <param name="A">Левое значение</param>
        /// <param name="B">Правое значение</param>
        /// <returns></returns>
        public static byte[,] MatrixMult(byte[,] A, byte[,] B)
        {
            if (A.GetLength(1) != B.GetLength(0))
                throw new Exception("Размерность матрицы не совпадает");
            byte[,] R = new byte[A.GetLength(0), B.GetLength(1)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < B.GetLength(1); j++)
                {
                    for (int k = 0; k < B.GetLength(0); k++)
                    {
                        R[i, j] += (byte)((int)A[i, k] * (int)B[k, j]);
                    }
                }
            }
            return R;
        }

        /// <summary>
        /// Умножение матрицы на вектор
        /// </summary>
        /// <param name="A">Левое значение</param>
        /// <param name="B">Правое значение</param>
        /// <returns></returns>
        public static byte[] MatrixVectorMult(byte[,] A, byte[] b)
        {
            byte[] r = new byte[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                    r[i] += (byte)((int)A[i,j] * (int)b[j]);
            }
            return r;
        }

        public static byte[,] GaussJordanDownTriangleMethod(byte[,] A, int n) 
        {
            // byte[] key = Encoding.UTF8.GetBytes("personalbankfinistreportfinistsoftnorma");
            //  RC4 RC4 = new RC4(key);
            byte[,] E = new byte[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                   E[i, j] = i == j ? (byte)1 : (byte)0;
                }
            }
            for (int i = 0; i <n; i++)
            {
                byte deletel = A[i, i];
                for (int j = 0; j <= i; j++)
                {
                    A[i, j] = (byte)(((int)A[i, j] * (int)(BigInteger.Pow(new BigInteger((int)deletel), 19) % 100)) % 100);
                }

            }
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    E[j, i] = (byte)(100-(int)A[j, i]);
                    A[j, i] = 0;
                }
            }

            return E;
        }

        public static byte[,] GaussJordanUpTriangleMethod(byte[,] A, int n)
        {
            // byte[] key = Encoding.UTF8.GetBytes("personalbankfinistreportfinistsoftnorma");
            //  RC4 RC4 = new RC4(key);
            byte[,] E = new byte[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    E[i, j] = i == j ? (byte)1 : (byte)0;
                }
            }

            for (int i = 0; i < n; i++)
            {
                double deletel = A[i, i];
                for (int j = i; j < n; j++)
                {
                    A[i, j] = (byte)(((int)A[i, j] * (int)(BigInteger.Pow(new BigInteger((int)deletel), 19) % 100)) % 100);
                }
            }
            for (int i = n - 1; i > 0; i++)
            {
                for (int j = i - 1; j >= 0; j++)
                {
                    E[j, i] = (byte)(100 - (int)A[j, i]);
                    A[j, i] = 0;
                }
            }

            return E;
        }
    }
}

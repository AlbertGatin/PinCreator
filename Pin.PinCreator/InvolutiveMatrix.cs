using System;
using System.Numerics;

namespace Pin
{
    public class InvolutiveMatrix
    {
        public InvolutiveMatrix(RC4 rc)
        {
            RC4 = rc;
            InitAB();
            CommonInit();
        }
        public InvolutiveMatrix(RC4 rc, byte[] aVal, byte[] bVal)
        {
            RC4 = rc;
            aArr = aVal;
            bArr = bVal;
            CommonInit();
        }
        byte[] aArr;
        byte[] bArr;
        byte[,] M;
        byte[,] Mab1;
        byte[,] Mab2;
        byte[,] Mab3;
        byte[,] Mab4;
        byte[,] V;
        byte[,] W;
        byte[,] Vobr;
        byte[,] Wobr;
       public byte[,] A8;
        RC4 RC4;
        private void CommonInit()
        {
            InitMabs();
            BuildM();
            buildVW();
            buildVWobr();
            BuildA8();
        }
        
        /// <summary>
        /// Инициализация a и b для матриц порядка 2
        /// </summary>
        private void InitAB()
        {
            aArr = new byte[4];
            bArr = new byte[4];

            for(int i=0; i< 4; i++)
            {
                aArr[i] = RC4.RC4M(99);
                bArr[i] = RC4.CreateSimpleByte();
            }
        }
        /// <summary>
        /// Построение матриц порядка 2
        /// </summary>
        /// <param name="Mab"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void InitMabs()
        {
            InitMab(ref Mab1, aArr[0], bArr[0]);
            InitMab(ref Mab2, aArr[1], bArr[1]);
            InitMab(ref Mab3, aArr[2], bArr[2]);
            InitMab(ref Mab4, aArr[3], bArr[3]);
        }
        /// <summary>
        /// Построение матрицы порядка 2
        /// </summary>
        /// <param name="Mab"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void InitMab(ref byte[,] Mab, byte a, byte b)
        {
            Mab = new byte[2, 2];
            Mab[0, 0] = a;
            Mab[0, 1] = b;
            BigInteger temp = BigInteger.Pow(new BigInteger((int)b), 19);
            Mab[1, 0] = (byte)(((temp % 100) * (1 + (100 - ((int)a * (int)a) % 100)) % 100));
            Mab[1, 0] = (byte)((100 - (int)a) % 100);
        }
        /// <summary>
        /// Построение матрицы порядка 8 в диагонали которой матрицы порядка 2
        /// </summary>
        private void BuildM()
        {
            M = new byte[8, 8];

            for(int i=0; i<8; i++)
            {
                for(int j = 0; j<8; j++)
                {
                    M[i, j] = 0;
                }
            }
            InsertMab(Mab1, 0, 0);
            InsertMab(Mab2, 2, 2);
            InsertMab(Mab3, 4, 4);
            InsertMab(Mab4, 6, 6);
        }

        /// <summary>
        /// Вставка матрицы порядка 2 в матрицу порядка 8
        /// </summary>
        /// <param name="Mab">матрицы порядка 2</param>
        /// <param name="ibeg">начальный номер строки</param>
        /// <param name="jbeg">начальный номер столбца</param>
        private void InsertMab(byte[,] Mab, int ibeg, int jbeg)
        {
            for(int i = 0; i<2; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    M[i + ibeg, j + jbeg] = Mab[i, j];
                }
            }
        }
        /// <summary>
        /// Обратной нижним и верхним треугольным матрицам
        /// </summary>
        private void buildVWobr()
        {
            Vobr = Helper.GaussJordanDownTriangleMethod(V, 8);
            Wobr = Helper.GaussJordanDownTriangleMethod(V, 8);
        }
            /// <summary>
            /// Построение нижней и верхней треугольной матрицы
            /// </summary>
        private void buildVW()
        {
            V = new byte[8, 8];
            W = new byte[8, 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if(j>i)
                    {
                        V[i,j] = RC4.RC4M(99);
                        W[i, j] = 0;
                    }
                    else if(i>j)
                    {
                        V[i, j] =0;
                        W[i, j] = RC4.RC4M(99);
                    }
                    else
                    {
                        V[i, j] = RC4.CreateSimpleByte();
                        W[i, j] = RC4.CreateSimpleByte();
                    }
                }
            }
        }

        private void BuildA8()
        {
            try
            {
                A8 = Helper.MatrixMult(Helper.MatrixMult(Helper.MatrixMult(Helper.MatrixMult(V, W), M), Wobr), Vobr);
            }
            catch(Exception e)
            {
                throw new Exception("Ошибка при построении искомой инволютивной матрицы", e);
            }
        }
    }
}

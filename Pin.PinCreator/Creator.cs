using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pin
{
    public static class Creator
    {
        private static string CreateSingleKey(RC4 rc, byte[] c, InvolutiveMatrix matrix, Substitutions subs)
        {
            
            byte[] b = new byte[8];
            byte[] k = new byte[8]; //какай то ключ
            byte[] ke = new byte[31];
            int[] res = new int[16];
            for (int i = 0; i < 31; i++)
            {
                if (i < 8)
                    k[i] = rc.RC4M(99);
                ke[i] = rc.RC4M(99);
            }
            //c преобразуется в 8ми-разрядное число b по основанию 100
            for (int i = 0; i < 8; i++)
            {
                b[i] = (byte)(((int)c[2 * i]) + (int)c[2 * i + 1] * 10);
            }

            //(начальное забеливание)складываем вектора по модулю 100
            for (int i = 0; i < 8; i++)
            {
                b[i] = (byte)(((int)b[i] + (int)k[i]) % 100);
            }

            //8 раунов преобразований G
            for (int i = 0; i < 7; i++)
            {
                Feistel.GEncode(b[7], ke[2 * i], b[2], ke[4], ref b[0], ref b[1], subs);
                b = Helper.TwoElemToRight(b);
            }
            Feistel.GEncode(b[7], ke[14], b[2], ke[15], ref b[0], ref b[1], subs);
            //умножаем b  на инволютивную матрицу A8
            Helper.MatrixVectorMult(matrix.A8, b);
            //8 раундов преобразований G
            for (int i = 8; i < 15; i++)
            {
                Feistel.GDecode(b[7], ke[2 * i], b[2], ke[2 * i + 1], ref b[0], ref b[1], subs);
                //GTransformation(b[7], ke[2*i], b[2], ke[2*i+1], ref b[0], ref b[1]);
                b = Helper.TwoElemToRight(b);
            }
            Feistel.GDecode(b[7], ke[14], b[2], ke[15], ref b[0], ref b[1], subs);
            //GTransformation(b[7], ke[14], d[2], ke[15], ref b[0], ref b[1]);

            //заключительное отбеливание
            for (int i = 0; i < 8; i++)
            {
                b[i] = (byte)((int)b[i] + (100 - k[i]));
            }

            for (int i = 0; i < 8; i++)
            {
                res[2 * i] = (int)b[i] % 10;
                res[2 * i + 1] = ((int)b[i] / 10) % 10;
            }
            string result = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                result += res[i].ToString();
            }
            return result;
        }

        public static string[] Create(string s, string sKey, int keyCount)
        {
            byte[] bKey = Convert.FromBase64String(sKey);
            RC4 rc = new RC4(bKey);
            InvolutiveMatrix matrix = new InvolutiveMatrix(rc);
            Substitutions subs = new Substitutions(rc);
            //byte[] c = new byte[16];
            //for(int i=0;i<16; i++)
            //{
            //    c[i] = rc.RC4M(9);
            //}
            //Провераем входную строку
            if (!Helper.InputValidate(s))
                throw new Exception("Входная строка должна состоять из 16 десятичных символов");
            int temp;
            byte[] c = s.ToCharArray()
                .Where(x => (int.TryParse(x.ToString(), out temp)))
                .Select(x => (byte)(int.Parse(x.ToString()))).
                ToArray();//входная строка 
            string[] res = new string[keyCount];
            for(int i = 0; i < keyCount; i++)
            {
                res[i] = CreateSingleKey(rc, c,matrix,subs);
            }
            return res;
        }
    }
}

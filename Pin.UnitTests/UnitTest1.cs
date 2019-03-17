using Pin;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Pin.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FullTest()
        {
            //byte[] key = Encoding.UTF8.GetBytes("personalbankfinistreportfinistsoftnorma");
            //string res = Creator.Create("1234321345612345", Convert.ToBase64String(key));

        }
        
        [TestMethod]
        public void GaussJordanUpTriangleMethod()
        {
           // byte[] key = Encoding.UTF8.GetBytes("personalbankfinistreportfinistsoftnorma");
          //  RC4 RC4 = new RC4(key);
            int n = 3;
            double[,] A = { { 1, 0, 0}, { 2, 1, 0 }, { 3, 2, 1 } };
            double[,] E = { { 1, 0, 0}, { 0, 1, 0 }, { 0, 0, 1 } };
            for (int i = 0; i < n; i++)
            {
                double deletel = A[i, i];
                for (int j = 0; j <= i; j++)
                {
                    A[i, j] /= deletel;
                }

            }
            for (int i = 0; i < n - 1; i++)
            {
                for(int j = i + 1; j < n; j++)
                {
                    E[j, i] = -A[j, i];
                    A[j, i] = 0;
                }
            }
        }

        [TestMethod]
        public void GaussJordanDownTriangleMethod()
        {
            // byte[] key = Encoding.UTF8.GetBytes("personalbankfinistreportfinistsoftnorma");
            //  RC4 RC4 = new RC4(key);
            int n = 3;
            double[,] A = { { 1, 2, 1 }, { 0, 1, 2 }, { 0, 0, 1 } };
            double[,] E = { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            for (int i = 0; i < n; i++)
            {
                double deletel = A[i, i];
                for (int j = i; j < n; j++)
                {
                    A[i, j] /= deletel;
                }
            }
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    E[j, i] = -A[j, i];
                    A[j, i] = 0;
                }
            }
        }
        #region RC4

        [TestMethod]
        public void Test_RC4()
        {
            string sTestInput = "Hello! My name is Albert!";
            string sDecodedResponse = string.Empty;
            try
            {
                byte[] key = Encoding.UTF8.GetBytes("Key");

                RC4 rcEncoder = new RC4(key);
                
                byte[] bTestInput = Encoding.UTF8.GetBytes(sTestInput);
                byte[] bResponse = rcEncoder.Encoder(bTestInput, bTestInput.Length);

                RC4 rcDecoder = new RC4(key);
                byte[] bDecodedResponse = rcDecoder.Decoder(bResponse, bResponse.Length); 
                sDecodedResponse = Encoding.UTF8.GetString(bDecodedResponse);
            }
            catch(Exception ex)
            {
                throw new Exception("Ошибка RC4" + ex.ToString());
            }
            if (sTestInput == sDecodedResponse)
            {
                throw new Exception("Успех");
            }
            throw new Exception("Ошибка, resp = " + sDecodedResponse);
        }


        #endregion RC4

        #region SubstitutionsBuilder
        [TestMethod]
        public void Test_Subs()
        {
            try
            {
                byte[] key = Encoding.UTF8.GetBytes("personalbankfinistreportfinistsoftnorma");
                int size = 100;
                Substitutions s = new Substitutions(key, size);
                
            }
            catch(Exception e)
            {
                throw new Exception("Ошибка теста", e);
            }
        }
        #endregion SubstitutionsBuilder

        #region Helper 

        [TestMethod]
        public void MultMatrixTest()
        {
            try
            {
                byte[,] A = { { 1, 2, 3 }, { 3, 6, 2 }, { 21, 5, 7 } };
                byte[,] B = { { 5, 5, 3 }, { 3, 2, 72 }, { 1, 2, 5 } };
                byte[,] Res = Helper.MatrixMult(A, B);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка теста перемножения матриц", ex);
            }
        }

        [TestMethod]
        public void MultMatrixVectorTest()
        {
            try
            {
                byte[,] A = { { 1, 2, 3 }, { 3, 6, 2 }, { 21, 5, 7 } };
                byte[] b = { 7, 1, 2 };
                byte[] Res = Helper.MatrixVectorMult(A, b);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка теста перемножения матрицы на вектор", ex);
            }
        }

        [TestMethod]
        public void Test_InputValidate()
        {
            try
            {
                string testS = "123";
                if (Helper.InputValidate(testS))
                    throw new Exception("Test_InputValidate");
                testS = "asd";
                if (Helper.InputValidate(testS))
                    throw new Exception("Test_InputValidate");
                testS = "asddsaasddsasdaa";
                if (Helper.InputValidate(testS))
                    throw new Exception("Test_InputValidate");
                testS = "123dsaasddsasdaa";
                if (Helper.InputValidate(testS))
                    throw new Exception("Test_InputValidate");
                testS = "+123saasddsasdaa";
                if (Helper.InputValidate(testS))
                    throw new Exception("Test_InputValidate");
                testS = "+123456789123456";
                if (Helper.InputValidate(testS))
                    throw new Exception("Test_InputValidate");
                testS = "-123456789123456";
                if (Helper.InputValidate(testS))
                    throw new Exception("Test_InputValidate");
                testS = "123456789-123456";
                if (Helper.InputValidate(testS))
                    throw new Exception("Test_InputValidate");
                testS = "1234567891+23456";
                if (Helper.InputValidate(testS))
                    throw new Exception("Test_InputValidate");
                testS = "1123456789123456";
                if (!Helper.InputValidate(testS))
                    throw new Exception("Test_InputValidate");
            }
            catch (Exception e)
            {
                throw new Exception("some exception", e);
            }
            throw new Exception("Success");
        }
        [TestMethod]
        public void Test_IsDigit()
        {
            try
            {
                string testS = "123";
                if (!Helper.IsDigit(testS))
                    throw new Exception("Test_InputValidate");
                testS = "asd";
                if (Helper.IsDigit(testS))
                    throw new Exception("Test_InputValidate");
                testS = "asddsaasddsasdaa";
                if (Helper.IsDigit(testS))
                    throw new Exception("Test_InputValidate");
                testS = "123dsaasddsasdaa";
                if (Helper.IsDigit(testS))
                    throw new Exception("Test_InputValidate");
                testS = "+123saasddsasdaa";
                if (Helper.IsDigit(testS))
                    throw new Exception("Test_InputValidate");
                testS = "+123456789123456";
                if (Helper.IsDigit(testS))
                    throw new Exception("Test_InputValidate");
                testS = "-123456789123456";
                if (Helper.IsDigit(testS))
                    throw new Exception("Test_InputValidate");
                testS = "123456789-123456";
                if (Helper.IsDigit(testS))
                    throw new Exception("Test_InputValidate");
                testS = "1234567891+23456";
                if (Helper.IsDigit(testS))
                    throw new Exception("Test_InputValidate");
                testS = "1123456789123456";
                if (!Helper.IsDigit(testS))
                    throw new Exception("Test_InputValidate");
            }
            catch (Exception e)
            {
                throw new Exception("some exception", e);
            }
            throw new Exception("Success");

        }
        [TestMethod]
        public void Test_TwoElemToRight()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
               

                char[] testS = { '1', '2', '3' };
                char[] resS = Helper.TwoElemToRight(testS);
                sb.AppendLine("1");
                sb.AppendLine(testS.ToString());
                sb.AppendLine(resS.ToString());

                testS = null;
                resS = null;
                testS = new char[8] { '0', '1', '2', '3', '4', '5', '6', '7' };
                resS = new char[8];
                resS = Helper.TwoElemToRight(testS);
                sb.AppendLine("2");
                sb.AppendLine(testS.ToString());
                sb.AppendLine(resS.ToString());

                testS = null;
                resS = null;
                testS = new char[11] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
                resS = new char[11];
                resS = Helper.TwoElemToRight(testS);
                sb.AppendLine("3");
                sb.AppendLine(testS.ToString());
                sb.AppendLine(resS.ToString());

                testS = null;
                resS = null;
                testS = new char[2] { '0', '1'};
                resS = new char[2];
                resS = Helper.TwoElemToRight(testS);
                sb.AppendLine("4");
                sb.AppendLine(testS.ToString());
                sb.AppendLine(resS.ToString());

                testS = null;
                resS = null;
                testS = new char[1] { '0' };
                resS = new char[1];
                resS = Helper.TwoElemToRight(testS);
                sb.AppendLine("5");
                sb.AppendLine(testS.ToString());
                sb.AppendLine(resS.ToString());

                testS = null;
                resS = null;
                testS = new char[0] { };
                resS = new char[0];
                resS = Helper.TwoElemToRight(testS);
                sb.AppendLine("6");
                sb.AppendLine(testS.ToString());
                sb.AppendLine(resS.ToString());              
            }
            catch (Exception e)
            {
                throw new Exception("some exception", e);
            }
            throw new Exception("Success" + sb.ToString());
        }
        #endregion Helper
    }
}

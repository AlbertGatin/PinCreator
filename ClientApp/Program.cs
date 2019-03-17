using Pin;
using System;
using System.Text;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                byte[] key = Encoding.UTF8.GetBytes("keykeykeykeykey");
                string[] res = Creator.Create("1848576927564916", Convert.ToBase64String(key), 100);
                Console.WriteLine(res);
                for (int i = 0; i < 100; i++)
                {
                   // res = Creator.Create(res, Convert.ToBase64String(key));
                    Console.WriteLine(res[i]); 
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка теста" + e.ToString());
            }
        }
    }
}

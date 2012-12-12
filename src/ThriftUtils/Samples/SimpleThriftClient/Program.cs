/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System;
using System.Threading;

namespace ThriftUtils.Samples.SimpleClient
{
    class Program
    {
        private static SimpleClient client;

        static void Main(string[] args)
        {
            client = new SimpleClient("localhost", 9999);

            while (true)
            {
                new Thread(ThreadFunc).Start();
                Thread.Sleep(500);
            }

        }

        private static void ThreadFunc()
        {
            string s = client.InvokeTest();
            Console.WriteLine("Server response = " + s);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace ThriftUtils.Samples.PooledThriftClient
{
    class Program
    {
        private static PooledClient client;

        static void Main(string[] args)
        {
            client = new PooledClient("localhost:9999,localhost:9998");

            while (true)
            {
                new Thread(ThreadFunc).Start();
                Thread.Sleep(500);
            }
        }

        private static void ThreadFunc()
        {
            try
            {
                string s = client.InvokeTest();
                Console.WriteLine("Server response = " + s);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}

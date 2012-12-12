/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System;

namespace ThriftUtils.Samples.SimpleServer
{
    /// <summary>
    /// Simple thrift server example entry point.
    /// </summary>
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:\tSimpleThriftServer <port number>");
                return 1;
            }

            var port = Convert.ToInt32(args[0]);
            new SimpleServer(port).Run();
            Console.WriteLine("Server running on " + port);
            return 0;
        }
    }
}

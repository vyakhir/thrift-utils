/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System;
using System.Threading;
using Thrift.Server;
using Thrift.Transport;

namespace ThriftUtils.Samples.SimpleServer
{
    /// <summary>
    /// Simple thrift server implementation example.
    /// </summary>
    internal class SimpleServer
    {
        private readonly TServer server;

        internal SimpleServer(int port)
        {
            TSampleService.Iface remoteService = new SampleService(port);

            TServerTransport transport = new TServerSocket(port);
            var processor = new TSampleService.Processor(remoteService);
            server = new TSimpleServer(processor, transport);
        }

        internal void Run()
        {
            new Thread(ThreadFunc).Start();
        }

        private void ThreadFunc()
        {
            try
            {
                server.Serve();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private class SampleService : TSampleService.Iface
        {
            private readonly int port;

            public SampleService(int port)
            {
                this.port = port;
            }

            public string invokeTest()
            {
                return "Reply from " + port;
            }
        }

    }
}

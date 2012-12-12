/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using Thrift.Protocol;
using Thrift.Transport;

namespace ThriftUtils.Samples.SimpleClient
{
    internal class SimpleClient
    {
        private readonly string host;
        private readonly int port;

        public SimpleClient(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        internal string InvokeTest()
        {
            TTransport transport = new TSocket(host, port);
            var client = new TSampleService.Client(new TBinaryProtocol(transport));

            try
            {
                transport.Open();
                return client.invokeTest();
            }
            finally
            {
                if (transport.IsOpen)
                {
                    transport.Close();
                }
            }
        }
    }
}

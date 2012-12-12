using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Thrift.Protocol;
using Thrift.Transport;
using ThriftUtils.Common;
using ThriftUtils.Pooling;

namespace ThriftUtils.Samples.PooledThriftClient
{
    internal class PooledClient : IDisposable
    {
        private const int SocketTimeout = 100;

        private readonly ThriftFactory<TSampleService.Iface> thriftFactory;

        public PooledClient(string endpointList)
        {
            var endpoints = IpUtils.ParseEndpointList(endpointList);
            thriftFactory = new ThriftFactory<TSampleService.Iface>(endpoints, SocketTimeout, 
                socket => new TSampleService.Client(new TBinaryProtocol(socket)));
        }

        internal string InvokeTest()
        {
            using (var thrift = thriftFactory.GetThift())
            {
                return thrift.Client.invokeTest();
            }
        }

        public void Dispose()
        {
            thriftFactory.Dispose();
        }
    }
}

/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Thrift.Transport;
using ThriftUtils.Pooling.Balancing;
using ThriftUtils.Pooling.Extensions;

namespace ThriftUtils.Pooling
{
    public class ThriftFactory<T> : IDisposable
    {
        private readonly ICollection<ThriftEndpoint> thriftEndpoints;
        private readonly Func<TSocket, T> clientBuilder;
        private readonly ILoadBalancer<ThriftEndpoint> loadBalancer;

        public ThriftFactory(IEnumerable<IPEndPoint> remoteHosts, int socketTimeout, Func<TSocket, T> clientBuilder)
        {
            this.clientBuilder = clientBuilder;

            thriftEndpoints = new List<ThriftEndpoint>(remoteHosts.Select(it => new ThriftEndpoint(it, new TSocketPool(it, socketTimeout))));
            loadBalancer = new ThriftRoundRobinBalancer(thriftEndpoints);
        }

        public Thrift<T> GetThift()
        {
            TSocket socket;
            while (true)
            {
                lock (loadBalancer)
                {
                    var thriftEndpoint = loadBalancer.Offer();
                    socket = thriftEndpoint.SocketPool.Acquire();
                    if (socket.TryOpen())
                    {
                        loadBalancer.OnOfferSucceeded();
                        break;
                    }

                    loadBalancer.OnOfferFailed();
                }
            }

            return new Thrift<T>(socket, clientBuilder(socket));
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            foreach (var thriftEndpoint in thriftEndpoints)
            {
                thriftEndpoint.Dispose();
            }
            loadBalancer.Dispose();
        }

        #endregion
    }
}

/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Thrift.Transport;
using ThriftUtils.Common;
using ThriftUtils.Pooling.Extensions;

namespace ThriftUtils.Pooling.Balancing
{
    public class ThriftRoundRobinBalancer : ILoadBalancer<ThriftEndpoint>
    {
        private const int PingSocketTimeout = 5;        //  in seconds
        private const int PingSocketInterval = 5;       //  in seconds

        private int enabledCount;
        private readonly IList<LoadBalancerItemWrapper<ThriftEndpoint>> wrappedItems;
        private readonly CircularIterator<LoadBalancerItemWrapper<ThriftEndpoint>> iterator;

        private readonly IDictionary<ThriftEndpoint, Thread> pingingThreadMap = new Dictionary<ThriftEndpoint, Thread>();

        public ThriftRoundRobinBalancer(IEnumerable<ThriftEndpoint> endpoints)
        {
            wrappedItems = new List<LoadBalancerItemWrapper<ThriftEndpoint>>(endpoints.Select(it => new LoadBalancerItemWrapper<ThriftEndpoint>(it)));
            iterator = new CircularIterator<LoadBalancerItemWrapper<ThriftEndpoint>>(wrappedItems);
            enabledCount = wrappedItems.Count;
        }

        #region Implementation of ILoadBalancer

        public ThriftEndpoint Offer()
        {
            if (enabledCount == 0)
            {
                throw new LoadBalancerException("No available remote servers found");
            }

            while (iterator.Current == null || !iterator.Current.Enabled)
            {
                iterator.MoveNext();
            }
            return iterator.Current.Item;
        }

        public void OnOfferSucceeded()
        {
            iterator.MoveNext();
        }

        public void OnOfferFailed()
        {
            Disable(iterator.Current);
            StartMonitoring(iterator.Current);
            iterator.MoveNext();
        }

        #endregion

        private void StartMonitoring(LoadBalancerItemWrapper<ThriftEndpoint> wrappedThriftEndpoint)
        {
            var endpoint = wrappedThriftEndpoint.Item.Endpoint;
            var thread = new Thread(() =>
                {
                    for (;;)
                    {
                        var socket = new TSocket(endpoint.Address.ToString(), endpoint.Port, PingSocketTimeout);
                        if (socket.TryOpen())
                        {
                            socket.Close();
                            Enable(wrappedThriftEndpoint);
                            RemovePingingThread(wrappedThriftEndpoint.Item);
                            return;
                        }

                        Thread.Sleep(PingSocketInterval*1000);
                    }
                });

            RegisterPingingThread(wrappedThriftEndpoint.Item, thread);
            thread.Start();
        }

        private void Enable(LoadBalancerItemWrapper<ThriftEndpoint> wrappedThriftEndpoint)
        {
            lock (this)
            {
                wrappedThriftEndpoint.Enabled = true;
                enabledCount++;
            }
        }

        private void Disable(LoadBalancerItemWrapper<ThriftEndpoint> wrappedThriftEndpoint)
        {
            lock (this)
            {
                wrappedThriftEndpoint.Enabled = false;
                enabledCount--;
            }
        }

        private void RegisterPingingThread(ThriftEndpoint thriftEndpoint, Thread thread)
        {
            lock (pingingThreadMap)
            {
                pingingThreadMap[thriftEndpoint] = thread;
            }
        }

        private void RemovePingingThread(ThriftEndpoint thriftEndpoint)
        {
            lock (pingingThreadMap)
            {
                pingingThreadMap.Remove(thriftEndpoint);
            }
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            lock (pingingThreadMap)
            {
                foreach (var key in pingingThreadMap.Keys)
                {
                    pingingThreadMap[key].Abort();
                }
            }
        }

        #endregion
    }
}

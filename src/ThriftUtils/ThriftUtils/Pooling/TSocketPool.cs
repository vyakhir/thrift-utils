/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System.Net;
using Pooling;
using Pooling.Storage;
using Pooling.Management;
using Thrift.Transport;

namespace ThriftUtils.Pooling
{
    public class TSocketPool : IPool<TSocket>
    {
        private const int DefaultCapacity = 100;
        private readonly IPool<TSocket> socketPool;

        public TSocketPool(IPEndPoint endpoint, int timeout, int capacity = DefaultCapacity)
        {
            var itemStore = new QueueStore<TSocket>();
            var poolManager = new LazyManager<TSocket>(itemStore, () => new TSmartSocket(socketPool, endpoint, timeout));
            socketPool = new Pool<TSocket>(poolManager, capacity);
        }
       
        #region Implementation of IDisposable

        public void Dispose()
        {
            socketPool.Dispose();
        }

        #endregion

        #region Implementation of IPool<TTransport>

        public TSocket Acquire()
        {
            return socketPool.Acquire();
        }

        public int Release(TSocket item)
        {
            return socketPool.Release(item);
        }

        public bool IsDisposed
        {
            get { return socketPool.IsDisposed; }
        }

        public int Capacity
        {
            get { return socketPool.Capacity; }
        }

        public int Count 
        { 
            get { return socketPool.Count; }
        }

        #endregion
    }
}

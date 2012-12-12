/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System;
using System.Net;
using Pooling;
using Thrift.Transport;

namespace ThriftUtils.Pooling
{
    internal class TSmartSocket : TSocket, IDisposable
    {
        private readonly IPool<TSocket> pool;

        public TSmartSocket(IPool<TSocket> pool, IPEndPoint host, int timeout)
            : base(host.Address.ToString(), host.Port, timeout)
        {
            this.pool = pool;
        }

        void IDisposable.Dispose()
        {
            if (pool.IsDisposed)
            {
                base.Dispose();
            }
            else
            {
                pool.Release(this);
            }
        }
    }
}

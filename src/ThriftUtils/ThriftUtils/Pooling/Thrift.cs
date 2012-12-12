/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System;
using Thrift.Transport;

namespace ThriftUtils.Pooling
{
    public class Thrift<T> : IDisposable
    {
        private readonly TSocket socket;

        internal Thrift(TSocket socket, T client)
        {
            this.socket = socket;
            Client = client;
        }

        public T Client { get; private set; }

        #region Implementation of IDisposable

        public void Dispose()
        {
            ((IDisposable)socket).Dispose();
        }

        #endregion
    }
}

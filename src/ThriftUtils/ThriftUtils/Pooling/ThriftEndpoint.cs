/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System;
using System.Net;

namespace ThriftUtils.Pooling
{
    public class ThriftEndpoint : IDisposable
    {
        public IPEndPoint Endpoint { get; private set; }
        public TSocketPool SocketPool { get; private set; }

        internal ThriftEndpoint(IPEndPoint endpoint, TSocketPool socketPool)
        {
            Endpoint = endpoint;
            SocketPool = socketPool;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            SocketPool.Dispose();
        }

        #endregion

        public override bool Equals(object obj)
        {
            var tep = obj as ThriftEndpoint;
            if (tep == null)
            {
                return false;
            }

            return Endpoint.Equals(tep.Endpoint);
        }

        public override int GetHashCode()
        {
            return Endpoint.GetHashCode();
        }
    }
}

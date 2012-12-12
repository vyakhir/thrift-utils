/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System.Net.Sockets;
using Thrift.Transport;

namespace ThriftUtils.Pooling.Extensions
{
    internal static class TSocketExtetsions
    {
        internal static bool TryOpen(this TSocket socket)
        {
            try
            {
                if (!socket.IsOpen)
                {
                    socket.Open();
                }
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }

    }
}

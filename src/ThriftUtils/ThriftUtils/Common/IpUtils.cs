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
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace ThriftUtils.Common
{
    /// <summary>
    /// Common purpose IP related utilities.
    /// </summary>
    public static class IpUtils
    {
        private const string IpAddressMask = @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}";
        private const char DefaultEndpointSeparator = ',';

        /// <summary>
        /// Parses endpoint from a string.
        /// </summary>
        /// <param name="endpoint">endpoint string</param>
        /// <returns>an instance of <see cref="IPEndPoint"/> class</returns>
        /// <exception cref="ArgumentException">if parsing endpoint fails</exception>
        public static IPEndPoint ParseEndpoint(string endpoint)
        {
            var parts = endpoint.Trim().Split(':');

            //  if endpoint is an IP, just parse it
            if (Regex.IsMatch(parts[0], IpAddressMask))
            {
                return new IPEndPoint(IPAddress.Parse(parts[0]), Convert.ToInt32(parts[1]));
            }

            //  otherwise try to resolve a DNS entry to a collection of IP addresses and look for an IPv4 one
            var address = Dns.GetHostAddresses(parts[0]).SingleOrDefault(it => it.AddressFamily == AddressFamily.InterNetwork);
            if (address != null)
            {
                return new IPEndPoint(address, Convert.ToInt32(parts[1]));
            }

            throw new ArgumentException("Invalid host " + endpoint, "endpoint");
        }

        /// <summary>
        /// Parses a list of endpoints from a string using given separator.
        /// </summary>
        /// <param name="endpoints">endpoint list</param>
        /// <param name="separator">optional separator; default is comma(<c>,</c>)</param>
        /// <returns>a list of <see cref="IPEndPoint"/> instances</returns>
        /// <exception cref="ArgumentException">if parsing endpoint fails</exception>
        public static IList<IPEndPoint> ParseEndpointList(string endpoints, char separator = DefaultEndpointSeparator)
        {
            return endpoints.Split(separator).Select(ParseEndpoint).ToList();
        }
    }
}

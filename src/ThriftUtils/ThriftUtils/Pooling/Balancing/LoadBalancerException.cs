/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System;

namespace ThriftUtils.Pooling.Balancing
{
    public class LoadBalancerException : Exception
    {
        public LoadBalancerException(string message) 
            : base(message)
        {
        }
    }
}

/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System;

namespace ThriftUtils.Pooling.Balancing
{
    public interface ILoadBalancer<out T> : IDisposable
    {
        T Offer();

        void OnOfferSucceeded();
        void OnOfferFailed();
    }
}

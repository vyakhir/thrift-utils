/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

namespace ThriftUtils.Pooling.Balancing
{
    internal class LoadBalancerItemWrapper<T>
    {
        internal LoadBalancerItemWrapper(T item)
            : this(item, true)
        {
        }

        internal LoadBalancerItemWrapper(T item, bool enabled)
        {
            Item = item;
            Enabled = enabled;
        }

        public T Item { get; private set; }

        public bool Enabled { get; set; }
    }
}

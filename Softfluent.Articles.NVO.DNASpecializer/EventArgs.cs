using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Softfluent.Articles.NVO.DNASpecializer
{
    public class EventArgs<T> : EventArgs
    {
        public T Item { get; set; }
        public EventArgs(T item) : base()
        {
            Item = item;
        }
    }
}

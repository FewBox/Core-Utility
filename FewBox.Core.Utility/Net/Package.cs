using System.Collections.Generic;

namespace FewBox.Core.Utility.Net
{
    public class Package<T>
    {
        public IList<Header> Headers { get; set; }
        public T Body { get; set; }
    }
}
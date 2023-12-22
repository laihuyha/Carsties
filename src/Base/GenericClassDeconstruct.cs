using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base
{
    public class GenericClassDeconstruct<T1, T2, TRest>
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
        public TRest RestValue { get; set; }

        public (T1, T2, TRest) Deconstruct()
        {
            return (Value1, Value2, RestValue);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Base
{
    public class GenericClassDeconstruct<T1, T2, T3, T4, T5, T6, T7, TRest>
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
        public T3 Value3 { get; set; }
        public T4 Value4 { get; set; }
        public T5 Value5 { get; set; }
        public T6 Value6 { get; set; }
        public T7 Value7 { get; set; }
        public TRest RestValue { get; set; }

        public (T1, T2, T3, T4, T5, T6, T7, TRest) Deconstruct()
        {
            return (Value1, Value2, Value3, Value4, Value5, Value6, Value7, RestValue);
        }
    }
}
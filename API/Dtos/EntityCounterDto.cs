using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodDelivery.Dtos
{
    public class EntityCounterDto<T>
    {
        public required T Obj { get; set; }
        public int Count { get; set; }
    }
}
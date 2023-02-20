using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Dtos
{
    public class RecommendationResult<TEntity>
    {
        public required List<TEntity> Similar { get; set; }
        public required List<EntityCounterDto<TEntity>> Frequented { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodDelivery.Repository
{
    public interface IGenericRepository<TEntity, TCreateDto, TUpdateDto>
        where TEntity : class
        where TCreateDto : class
        where TUpdateDto : class
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity> CreateAsync(TCreateDto newObjDto);
        Task<TEntity?> UpdateAsync(int id, TUpdateDto updateObjDto);
        Task<TEntity?> DeleteAsync(int id);
        Task<bool> Exists(int id);
    }
}
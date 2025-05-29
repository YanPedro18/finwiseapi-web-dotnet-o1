using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IRepository<TInputDto, TOutputDto, TEntity>
    {
        Task<TOutputDto> CreateAsync(TInputDto dto);
        Task<List<TOutputDto>> GetAllAsync();
        Task<TOutputDto> GetByIdAsync(Guid id);
        Task<List<TOutputDto>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate);
        Task UpdateAsync(TInputDto dto);
        Task DeleteAsync(Guid id);
    }
}

using AutoMapper;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class Repository<TInputDto, TOutputDto, TEntity> : IRepository<TInputDto, TOutputDto, TEntity>
        where TEntity : class
    {
        protected readonly DbContext _dbContext;
        protected readonly IMapper _mapper;
        private readonly DbSet<TEntity> _dbSet;

        //injeção depedencia repository padrão
        public Repository(DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _dbSet = _dbContext.Set<TEntity>();
        }


        public async Task<TOutputDto> CreateAsync(TInputDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<TOutputDto>(entity);
        }


        public async Task<List<TOutputDto>> GetAllAsync()
        {
            var entities = await _dbSet.AsNoTracking().ToListAsync();
            return _mapper.Map<List<TOutputDto>>(entities);
        }

        public async Task<TOutputDto> GetByIdAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            return _mapper.Map<TOutputDto>(entity);
        }

        public async Task<List<TOutputDto>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await _dbSet
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync();

            return _mapper.Map<List<TOutputDto>>(entities);
        }

        public async Task UpdateAsync(TInputDto dto)
        {
            //var entity = _mapper.Map<TEntity>(dto);
            //_dbSet.Update(entity);
            //await _dbContext.SaveChangesAsync();

            var keyProperty = dto.GetType().GetProperty("Id");

            if (keyProperty == null)
                throw new Exception("DTO não contém a propriedade 'Id'.");

            var keyValue = keyProperty.GetValue(dto);

            if (keyValue is not Guid id)
                throw new Exception("A propriedade 'Id' não é do tipo Guid.");

            var entity = await _dbSet.FindAsync(keyValue);
            if (entity == null)
                throw new Exception("Entidade não encontrada para atualização");

            // Aplica o mapeamento do DTO para a entidade existente
            _mapper.Map(dto, entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}

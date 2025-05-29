using AutoMapper;
using DataAccess.Context;
using DataAccess.Interfaces;
using DTOs.Transaction;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class TransactionRepository : Repository<TransactionRequest, TransactionResponse, Transaction>, ITransactionRepository
    {
        private readonly MainContext _dbContext;
        private readonly IMapper _mapper;

        public TransactionRepository(MainContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        //crate
        public async Task<TransactionResponse> Create(TransactionRequest transaction)
        {
            var resultTransaction = new TransactionResponse();
            try
            {
                resultTransaction = await CreateAsync(transaction);
            }
            catch (Exception)
            {

                throw;
            }

            return resultTransaction;
        }
    }
}

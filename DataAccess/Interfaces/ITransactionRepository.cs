using DTOs.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface ITransactionRepository
    {
        Task<TransactionResponse> Create(TransactionRequest transaction);
    }
}

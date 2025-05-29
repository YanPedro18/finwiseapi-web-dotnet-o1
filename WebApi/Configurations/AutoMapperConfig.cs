using AutoMapper;
using DTOs.Transaction;
using System.Transactions;

namespace WebApi.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Transaction, TransactionRequest>().ReverseMap();
            CreateMap<Transaction, TransactionResponse>().ReverseMap();
        }
    }
    
}

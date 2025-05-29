using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Transaction
{
    public class TransactionRequest
    {
        [Key]
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public bool Type { get; set; } // Entrada ou Saída

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}

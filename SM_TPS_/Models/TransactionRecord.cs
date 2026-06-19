using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SM_TPS_.Models
{
    public class TransactionRecord
    {
        public int Id { get; set; }

        public string TransactionDate { get; set; }

        public decimal Total { get; set; }
    }
}
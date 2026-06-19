using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM_TPS_.Data;
using SM_TPS_.Models;
using System.Collections.ObjectModel;

namespace SM_TPS_.ViewModels
{
    public class SalesHistoryViewModel : ViewModelBase
    {
        public ObservableCollection<TransactionRecord>
            Transactions
        { get; set; }

        public SalesHistoryViewModel()
        {
            TransactionRepository repository =
                new TransactionRepository();

            Transactions =
                new ObservableCollection<TransactionRecord>(
                    repository.GetAllTransactions());
        }
    }
}
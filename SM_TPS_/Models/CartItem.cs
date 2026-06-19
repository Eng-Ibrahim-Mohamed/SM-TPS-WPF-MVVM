using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SM_TPS_.Models
{
    public class CartItem : INotifyPropertyChanged
    {
        private Product _product;
        private int _quantity;

        public Product Product
        {
            get => _product;
            set { _product = value; OnPropertyChanged(); OnPropertyChanged(nameof(Total)); }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Total));
            }
        }

        public decimal Total => (Product?.Price ?? 0) * Quantity;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

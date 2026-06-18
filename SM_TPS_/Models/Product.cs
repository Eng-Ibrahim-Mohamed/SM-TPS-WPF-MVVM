using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SM_TPS_.Models
{
    public class Product : INotifyPropertyChanged
    {
        private string _name;
        private decimal _price;
        private int _quantity = 1;
        private int _stockQuantity;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Total));
            }
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

        public int StockQuantity
        {
            get => _stockQuantity;
            set
            {
                _stockQuantity = value;
                OnPropertyChanged();
                // 🚨 ممتاز! السطر ده بيخلي الشاشة تحس بتغيير حالة المخزون فوري
                OnPropertyChanged(nameof(IsLowStock));
            }
        }

        // خاصية ذكية ترجع true لو المنتج قارب على النفاد (3 قطع أو أقل)
        public bool IsLowStock
        {
            get => StockQuantity <= 3;
        }

        // حساب إجمالي السعر للمنتج في السلة
        public decimal Total
        {
            get => Price * Quantity;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
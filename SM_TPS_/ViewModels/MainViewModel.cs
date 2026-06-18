using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM_TPS_.Commands;
using SM_TPS_.Models;
using SM_TPS_.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SM_TPS_.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _productName;
        private decimal _productPrice;
        private decimal _total;
        private Product _selectedProduct;

        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> CartProducts { get; set; }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged();
            }
        }

        public decimal ProductPrice
        {
            get => _productPrice;
            set
            {
                _productPrice = value;
                OnPropertyChanged();
            }
        }

        public decimal Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }
        public ICommand CheckoutCommand { get; set; }

        public MainViewModel()
        {
            Products = new ObservableCollection<Product>
            {
                new Product { Name = "Milk", Price = 20 },
                new Product { Name = "Bread", Price = 15 },
                new Product { Name = "Rice", Price = 50 }
            };

            CartProducts = new ObservableCollection<Product>();

            AddProductCommand = new RelayCommand(AddProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
            AddToCartCommand = new RelayCommand(AddToCart);
            CheckoutCommand = new RelayCommand(Checkout);
        }

        private void AddProduct(object obj)
        {
            if (string.IsNullOrWhiteSpace(ProductName))
                return;

            Products.Add(new Product
            {
                Name = ProductName,
                Price = ProductPrice
            });

            ProductName = "";
            ProductPrice = 0;

            OnPropertyChanged(nameof(ProductName));
            OnPropertyChanged(nameof(ProductPrice));
        }

        private void DeleteProduct(object obj)
        {
            if (SelectedProduct != null)
            {
                Products.Remove(SelectedProduct);
            }
        }

        private void AddToCart(object obj)
        {
            if (SelectedProduct == null)
                return;

            CartProducts.Add(new Product
            {
                Name = SelectedProduct.Name,
                Price = SelectedProduct.Price
            });

            CalculateTotal();
        }

        private void Checkout(object obj)
        {
            CartProducts.Clear();
            Total = 0;
        }

        private void CalculateTotal()
        {
            Total = CartProducts.Sum(x => x.Price);
        }
    }
}
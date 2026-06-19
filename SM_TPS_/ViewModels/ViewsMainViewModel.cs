using SM_TPS_.Commands;
using SM_TPS_.Data;
using SM_TPS_.Models;
using SM_TPS_.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SM_TPS_.ViewModels
{
    public class ViewsMainViewModel : ViewModelBase
    {
        private string _productName;
        private string _productBarcode;
        private decimal _productPrice;
        private int _stockQuantity;
        private decimal _total;
        private decimal _subtotal;
        private decimal _vat;
        private Product _selectedProduct;
        private Transaction _selectedTransaction;
        private readonly ProductRepository _repository;
        private readonly TransactionRepository _transactionRepository;
        private Product _selectedCartProduct;
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> CartProducts { get; set; }

        // ✅ القائمة معرفة هنا بشكل صحيح كـ Transaction للـ History العميق
        public ObservableCollection<Transaction> TransactionHistory { get; set; } = new ObservableCollection<Transaction>();

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;

                if (_selectedProduct != null)
                {
                    ProductName = _selectedProduct.Name;
                    ProductPrice = _selectedProduct.Price;
                    StockQuantity = _selectedProduct.StockQuantity;
                }

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
        public string ProductBarcode
        {
            get => _productBarcode;
            set
            {
                _productBarcode = value;
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

        public int StockQuantity
        {
            get => _stockQuantity;
            set
            {
                _stockQuantity = value;
                OnPropertyChanged();
            }
        }
        public Product SelectedCartProduct
        {
            get => _selectedCartProduct;
            set
            {
                _selectedCartProduct = value;
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
        public int LowStockCount
        {
            get
            {
                return Products.Count(x => x.StockQuantity <= 3);
            }
        }

        public decimal Subtotal
        {
            get => _subtotal;
            set
            {
                _subtotal = value;
                OnPropertyChanged();
            }
        }

        public decimal VAT
        {
            get => _vat;
            set
            {
                _vat = value;
                OnPropertyChanged();
            }
        }

        public Transaction SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                _selectedTransaction = value;
                OnPropertyChanged();

                // 🚨 أول ما المستخدم يضغط على سطر في الـ History يعرض التفاصيل فوراً
                if (_selectedTransaction != null)
                {
                    string details = $"🧾 تفاصيل الفاتورة رقم: {_selectedTransaction.InvoiceId}\n";
                    details += $"📅 التاريخ: {_selectedTransaction.Timestamp}\n";
                    details += "--------------------------------------\n";

                    foreach (var item in _selectedTransaction.PurchasedItems)
                    {
                        details += $"🔹 {item.Name} | الكمية: {item.Quantity} | الإجمالي: {item.Total} EGP\n";
                    }

                    details += "--------------------------------------\n";
                    details += $"💰 الإجمالي النهائي: {_selectedTransaction.FinalTotal} EGP";

                    MessageBox.Show(details, "تفاصيل العملية القديمة", MessageBoxButton.OK, MessageBoxImage.Information);

                    // تصفير الاختيار عشان يقبل الضغط المتكرر بنجاح
                    _selectedTransaction = null;
                    OnPropertyChanged(nameof(SelectedTransaction));
                }
            }
        }

        public ICommand AddProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand UpdateProductCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }
        public ICommand CheckoutCommand { get; set; }
        public ICommand SearchBarcodeCommand { get; set; }
        public ICommand RemoveFromCartCommand { get; set; }

        public ViewsMainViewModel()
        {
            // بيانات تجريبية (Mock Data)
            _repository = new ProductRepository();

            _transactionRepository = new TransactionRepository();

            Products = new ObservableCollection<Product>(
                _repository.GetAllProducts());

            CartProducts = new ObservableCollection<Product>();

            // ✅ تم إزالة السطر المتضارب القديم هنا وتثبيت الـ Transaction النظيف
            TransactionHistory = new ObservableCollection<Transaction>();

            AddProductCommand = new RelayCommand(AddProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
            UpdateProductCommand = new RelayCommand(UpdateProduct);
            AddToCartCommand = new RelayCommand(AddToCart);
            CheckoutCommand = new RelayCommand(Checkout);
            SearchBarcodeCommand = new RelayCommand(SearchByBarcode);
            RemoveFromCartCommand =new RelayCommand(RemoveFromCart);
        }

        private void AddProduct(object obj)
        {
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                MessageBox.Show("اسم المنتج مطلوب.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool isProductExist = Products.Any(p => p.Name.Equals(ProductName.Trim(), StringComparison.OrdinalIgnoreCase));

            OnPropertyChanged(nameof(LowStockCount));

            if (isProductExist)
            {
                MessageBox.Show("هذا المنتج موجود بالفعل في المخزن!\nإذا كنت تريد تعديل سعره أو كميته، اختره من القائمة واستخدم زرار التعديل.",
                                "منتج مكرر",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            if (ProductPrice <= 0)
            {
                MessageBox.Show("يجب أن يكون السعر أكبر من الصفر.", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (StockQuantity < 0)
            {
                MessageBox.Show("لا يمكن أن تكون كمية المخزون سالبة.", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Product newProduct = new Product
            {
                Barcode = ProductBarcode,
                Name = ProductName.Trim(),
                Price = ProductPrice,
                StockQuantity = StockQuantity,
                Quantity = 1
            };

            _repository.AddProduct(newProduct);

            Products.Add(newProduct);

            ProductName = "";
            ProductBarcode = "";
            ProductPrice = 0;
            StockQuantity = 0;

            OnPropertyChanged(nameof(ProductName));
            OnPropertyChanged(nameof(ProductPrice));
            OnPropertyChanged(nameof(StockQuantity));

            MessageBox.Show("تم إضافة المنتج بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void SearchByBarcode(object obj)
        {
            var product = Products.FirstOrDefault(
                p => p.Barcode == SearchBarcode);

            if (product == null)
            {
                MessageBox.Show("Barcode Not Found");
                return;
            }

            SelectedProduct = product;

            AddToCart(null);

            SearchBarcode = "";
        }
        private void DeleteProduct(object obj)
        {
            if (SelectedProduct == null)
                return;

            if (MessageBox.Show("Delete Product?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _repository.DeleteProduct(SelectedProduct.Name);

                Products.Remove(SelectedProduct); 

                OnPropertyChanged(nameof(LowStockCount));
            }
        }

        private void UpdateProduct(object obj)
        {
            if (SelectedProduct == null)
                return;

            if (string.IsNullOrWhiteSpace(ProductName) ||
                ProductPrice <= 0 ||
                StockQuantity < 0)
            {
                MessageBox.Show(
                    "تأكد من إدخال بيانات صحيحة (الاسم، السعر، والكمية).",
                    "خطأ",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            SelectedProduct.Name = ProductName.Trim();
            SelectedProduct.Price = ProductPrice;
            SelectedProduct.StockQuantity = StockQuantity;

            _repository.UpdateProduct(SelectedProduct);

            OnPropertyChanged(nameof(LowStockCount));

            MessageBox.Show(
                "Product Updated Successfully",
                "نجاح",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            OnPropertyChanged(nameof(Products));
        }
        private string _searchBarcode;

        public string SearchBarcode
        {
            get => _searchBarcode;
            set
            {
                _searchBarcode = value;
                OnPropertyChanged();
            }
        }

        private void AddToCart(object obj)
        {
            if (SelectedProduct == null)
                return;

            if (SelectedProduct.StockQuantity <= 0)
            {
                MessageBox.Show("Out Of Stock!", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SelectedProduct.StockQuantity--;

            var existingItem = CartProducts.FirstOrDefault(p => p.Name == SelectedProduct.Name);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                var existingProduct = CartProducts
                     .FirstOrDefault(p => p.Name == SelectedProduct.Name);

                if (existingProduct != null)
                {
                    existingProduct.Quantity++;
                }
                else
                {
                    CartProducts.Add(new Product
                    {
                        Name = SelectedProduct.Name,
                        Price = SelectedProduct.Price,
                        Quantity = 1
                    });
                }
            }

            CalculateTotal();
        }
        private void RemoveFromCart(object obj)
        {
            if (SelectedCartProduct == null)
                return;

            var originalProduct = Products.FirstOrDefault(
                p => p.Name == SelectedCartProduct.Name);

            if (originalProduct != null)
            {
                originalProduct.StockQuantity++;
            }

            if (SelectedCartProduct.Quantity > 1)
            {
                SelectedCartProduct.Quantity--;
            }
            else
            {
                CartProducts.Remove(SelectedCartProduct);
            }

            CalculateTotal();
        }

        private void Checkout(object obj)
        {
            if (!CartProducts.Any())
            {
                MessageBox.Show("السلة فارغة!", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // إنشاء كائن Random متوافق مع كل إصدارات الـ .NET القديمة والجديدة
            Random rnd = new Random();

            // 🚨 الترتيب الصحيح والآمن بدون أي ميثودز غريبة
            var newTransaction = new Transaction
            {
                InvoiceId = rnd.Next(1000, 9999).ToString(), // رقم فاتورة عشوائي
                Timestamp = DateTime.Now,
                FinalTotal = this.Total,
                PurchasedItems = CartProducts.ToList() // أخذ نسخة كربونية نظيفة ومنفصلة تماماً عن السلة
            };

            // إضافة الفاتورة الذكية لتاريخ العمليات
            TransactionHistory.Add(newTransaction);

            MessageBox.Show("Transaction Completed!", "نجاح الدفع", MessageBoxButton.OK, MessageBoxImage.Information);

            _transactionRepository.AddTransaction(Total);

            MessageBox.Show("Transaction Completed!");
            PdfService.CreateInvoice(
            "Invoice.pdf",
            CartProducts,
            Subtotal,
            VAT,
            Total);
            // الآن نقدر نصفر السلة بأمان تام
            CartProducts.Clear();
            Subtotal = 0;
            VAT = 0;
            Total = 0;
        }

        private void CalculateTotal()
        {
            Subtotal = CartProducts.Sum(x => x.Total);
            VAT = Subtotal * 0.14m;
            Total = Subtotal + VAT;
        }
    }
}
using System;
using System.Collections.Generic;

namespace SM_TPS_.Models
{
    public class Transaction
    {
        public string InvoiceId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal FinalTotal { get; set; }

        // 🚨 هنا العمق: قايمة شايلة نسخة من المنتجات اللي اتباعت في الفاتورة دي بالظبط
        public List<Product> PurchasedItems { get; set; } = new List<Product>();

        // الدالة دي بتخلي الـ ListBox يعرض السطر ده بشكل شيك تلقائياً
        public override string ToString()
        {
            return $"Invoice #{InvoiceId} | {Timestamp:yyyy-MM-dd HH:mm:ss} | Total: {FinalTotal} EGP";
        }
    }
}
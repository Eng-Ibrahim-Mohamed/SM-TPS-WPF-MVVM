using iTextSharp.text;
using iTextSharp.text.pdf;
using SM_TPS_.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM_TPS_.Services
{
    public static class PdfService
    {
        public static void CreateInvoice(
                  string fileName,
                  IEnumerable<Product> products,
                 decimal subtotal,
                 decimal vat,
                 decimal total)
                    {
            Document document =
                new Document(PageSize.A4);

            PdfWriter.GetInstance(
                document,
                new FileStream(fileName, FileMode.Create));

            document.Open();

            document.Add(
                new Paragraph("SM TPS Invoice"));

            document.Add(
                new Paragraph(" "));

            PdfPTable table = new PdfPTable(4);

            table.AddCell("Product");
            table.AddCell("Qty");
            table.AddCell("Price");
            table.AddCell("Total");

            foreach (var item in products)
            {
                table.AddCell(item.Name);
                table.AddCell(item.Quantity.ToString());
                table.AddCell(item.Price.ToString());
                table.AddCell(item.Total.ToString());
            }

            document.Add(table);

            document.Add(new Paragraph(" "));

            document.Add(
                new Paragraph($"Subtotal : {subtotal} EGP"));

            document.Add(
                new Paragraph($"VAT : {vat} EGP"));

            document.Add(
                new Paragraph($"Total : {total} EGP"));

            document.Close();
        }
    }
}
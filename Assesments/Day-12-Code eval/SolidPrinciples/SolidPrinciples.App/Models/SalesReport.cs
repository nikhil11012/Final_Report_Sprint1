using SolidPrinciples.App.Interfaces;

namespace SolidPrinciples.App.Models
{
    public class SalesReport : Report, IReportExportable
    {
        public decimal TotalSales { get; set; }

        public SalesReport(string title, string content, decimal totalSales)
            : base(title, content)
        {
            TotalSales = totalSales;
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Total Sales: {TotalSales:C}");
        }

        public void Export(string destination)
        {
            File.WriteAllText(destination, $"{Title}\n{Content}\nTotal Sales: {TotalSales:C}");
        }
    }
}

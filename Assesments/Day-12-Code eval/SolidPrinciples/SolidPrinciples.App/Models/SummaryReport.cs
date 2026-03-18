using SolidPrinciples.App.Models;

namespace SolidPrinciples.App.Models
{
    public class SummaryReport : Report
    {
        public int TotalItems { get; set; }

        public SummaryReport(string title, string content, int totalItems)
            : base(title, content)
        {
            TotalItems = totalItems;
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Total Items: {TotalItems}");
        }
    }
}

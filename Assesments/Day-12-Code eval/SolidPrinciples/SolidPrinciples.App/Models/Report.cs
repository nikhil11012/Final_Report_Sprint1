using SolidPrinciples.App.Interfaces;

namespace SolidPrinciples.App.Models
{
    public abstract class Report : IReportPrintable
    {
        public string Title { get; set; }
        public string Content { get; set; }

        protected Report(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public virtual void Print()
        {
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Content: {Content}");
        }
    }
}
